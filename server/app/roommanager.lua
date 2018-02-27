
local json = require"cjson"
local log = require"june.log"
local util = require"utils"
local redis = require"june.utils.redis"
local sessionmgr = require"websocket.sessionmanager"

local Room = {}

local RoomState = {
    Prepare = 0,
    Start   = 1,
    End     = 2
}

local MemberState = {
    None      = 0,
    Prepared  = 1,
    Idle      = 2,
    Playing   = 3
}

function Room:new(rid)
    local ins = {
        key = table.concat({"room.", rid}),
        id  = rid,
        owner = nil,
        state = RoomState.Prepare,
        score = 100,
        total = 0,
        curr  = 0,
        members = {
            -- {id,state}
        },
        memcache={}
    }
    setmetatable(ins , { __index = self })
    return ins
end

function Room:start_game()
    self.state = RoomState.Start
    self:next_round()
end

function Room:next_round()
    self.curr = self.curr + 1
    if self.curr > #self.members then self.curr = 1 end
    self:sync({
        ec   = 0,
        cmd  = 1106,
        state= self.state ,
        curr = self.members[self.curr].id
    })
end

function Room:prepare(uid)
    if self.state ~= RoomState.Prepare then
        self:send(uid,{
            cmd = 1105,
            ec  = 1005
        })
    end

    local info = self.memcache[uid]
    if info == nil then
        self:send(uid,{
            cmd = 1105,
            ec  = 1002
        })
    end

    info.state = MemberState.Prepared

    if uid == self.owner then
        local allprepared = true
        for _,m in ipairs(self.members) do
            allprepared = allprepared or m.state == MemberState.Prepared
        end
        if not allprepared then
            self:send(uid,{
                cmd = 1105,
                ec = 1006
            })
        else
            self:start_game()
        end
    else
        self:sync({
            ec  = 0,
            cmd = 1105,
            room_id = self.rid,
            owner = self.owner,
            total = self.total,
            members = self.members
        })
    end

end

function Room:chage_state()

end

function Room:send(uid,data)
    local session = sessionmgr:get_session(uid)
    if session == nil then
        log:i("no session %s",uid)
    end
    session:send_json(data)
end

function Room:join(uid)

    if self.state ~= RoomState.Prepare then
        return self:send(uid,{ cmd = 1105, ec = 1004 })
    end

    if #self.members >= 4 then
        return self:send(uid,{ cmd = 1105, ec = 1003 })
    end
    log:i("[room join] %s %s",self.id,uid)
    if self.owner == nil then
        self.owner = uid
    end
    local info  = {
        id = uid,
        state = MemberState.None
    }
    local members = self.members or {}
    members[#members + 1] = info
    self.memcache[uid] =  info
    self.members = members

    self.total = #members * self.score
    local session = sessionmgr:get_session(uid)

    session:room(self.id)

    self:sync({
        ec  = 0,
        cmd = 1105,
        room_id = self.rid,
        owner = self.owner,
        total = self.total,
        members = members
    })
end

function Room:sync(data)
    for _,m in ipairs(self.members) do
        local session = sessionmgr:get_session(m.id)
        if session ~= nil then
            session:send_json(data)
        end
    end
end

function Room:leave(uid)
    self.memcache[uid] = nil
    local idx = nil

    for i,v in pairs(self.members) do
        if v.id == uid then
            idx = i
        end
    end

    if idx ~= nil then
        table.remove(self.members,idx)
    end
    --
    if #self.members == 0 then
        require"roommanager":del_room(self.id)
    end
    self.total = #self.members * self.score

    -- deal with the owner
    if self.owner == uid then
        self.owner = self.members[1].id
    end

    self:sync({
        ec  = 0,
        cmd = 1105,
        room_id = self.rid,
        owner = self.owner,
        total = self.total,
        members = self.members
    })

end

local M = {
    rooms = {}
}

function M:get_room(rid)
    return self.rooms[rid]
end

function M:del_room(rid)
    self.rooms[rid] = nil
end

function M:create_room(uid)
    local rid  = redis:incr("room.req")
    local room = Room:new(rid,uid)
    self.rooms[rid] = room
    return room
end

function M:exit_room(uid,rid)
    if uid == nil then return end

    if rid  == nil then

        return
    end

    local room = self:get_room(rid)
    if room == nil then  return end
    room:leave(uid)
end


return M
