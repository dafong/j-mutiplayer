
local json = require"cjson"
local log = require"june.log"
local util = require"utils"
local redis = require"june.utils.redis"
local sessionmgr = require"websocket.sessionmanager"
require"vector3"
require"vector2"

local config ={
    gravity = 144,
    basey   = 30,
    speedy  = 6,
    speedz  = 25,
    table_r = 1.5,
    floor_r = 0.5
}

local center= vector2(9,0)

local Room = {}

local RoomState = {
    None    = 0,
    Prepare = 1,
    Start   = 2,
    End     = 3
}

local MemberState = {
    None      = 0,
    Prepared  = 1,
    Idle      = 2,
    Playing   = 3
}

local Type = {
    GameInit = 0,
    RoundChange = 1
}

function Room:new(rid)
    local ins = {
        key = table.concat({"room.", rid}),
        id  = rid,
        owner = nil,
        state = RoomState.Prepare,
        score = 100,
        total = 0,
        seq   = 0,
        tseq  = 0,
        dir   = 0,
        dis   = 0,
        start = nil,
        dest  = nil, -- dest pos
        members = {
            -- {id,state}
        },
        history = {},
        memcache={}
    }
    log:i("[room create] id=%s",rid)
    setmetatable(ins , { __index = self })
    return ins
end

function Room:jump_start(uid)
    if self.state ~= RoomState.Start then
        return self:send(uid,{
            ec  = 1009,
            cmd = 106,
        })
    end

    local mem = self.members[self.cur]
    if mem == nil then
        return self:send(uid,{
            ec  = 1007,
            cmd = 106,
        })
    end

    if mem.id ~= uid then
        return self:send(uid,{
            ec = 1008,
            cmd= 106
        })
    end

    self:sync({
        cmd = 106,
        uid = uid
    })
end

function Room:jump_end(uid,data)
    local time = data.time
    local dir = self.dest - self.start
    dir = dir:Normalize()

    local speedy = config.basey + time * config.speedy
    speedy = speedy - speedy % 0.01

    local speedz = time * config.speedz
    speedz = speedz - speedz % 0.01

    local time = speedy / config.gravity * 2

    local dest = self.start + dir * time * speedz

    local r1 = config.table_r

    local r2 = config.floor_r

    if self.tseq > 1 then
        r1 = config.floor_r
    end

    local len1 = (r1 + r2) * (r1 + r2)

    local len2 = (dest - self.dest):Magnitude()

    local result = -1

    if len2 < len1 then
        result = 1
    end

    if len2 < (r1 * r1) then
        result = 0
    end


    log:i("[jump result] %s",result)

    self:sync({
        ec  = 0,
        cmd = 107,
        speedy = speedy,
        speedz = speedz,
        result = result,
        time   = time,
        start  = {x = self.start.x, y = self.start.y},
        dest   = {x = self.dest.x,  y = self.dest.y},
        destpos = { x = dest.x , y = dest.y }
    })

    if result == 0 then
        self.dest = destpos
        self:next_round()
    end
end

function Room:start_game()
    if self.state == RoomState.Start then
        return
    end
    self.state = RoomState.Start
    local rand1 = math.random() * 2
    rand1 = rand1 - rand1 % 1
    rand1 = (rand1 - 0.5) * 2
    self.dir  = rand1
    self.dis  = to_fixed(3.5 + math.random() * 2.5,2)
    self.seq  = 1
    self.tseq = 1
    local half = to_fixed(self.dis/2 * self.dir,2)
    if self.dir > 0 then
        self.start = vector2(center.x - half,0)
        self.dest  = vector2(center.x + half,0 )
    else
        self.start = vector2(center.x ,-  half)
        self.dest  = vector2(center.x , half)
    end
    self:sync({
        ec = 0,
        cmd= 1106,
        type = Type.GameInit,
        dir  = self.dir,
        dis  = self.dis,
        curr = self.members[self.seq].id
    })
end

function Room:next_round()
    self.seq = self.seq + 1
    self.tseq= self.tseq+ 1
    if self.seq > #self.members then self.seq = 1 end
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
            room_id = self.id,
            owner = self.owner,
            total = self.total,
            members = self.members
        })
    end
end


function Room:send(uid,data)
    local session = sessionmgr:get_session(uid)
    if session == nil then
        log:i("no session %s",uid)
        return
    end
    session:send(data)
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

    if self.owner ~= uid then
        self:send(uid,{
            ec = 0,
            cmd= 104,
            room_id = self.id,
            score = self.score
        })
    end

    self:sync({
        ec  = 0,
        cmd = 1105,
        room_id = self.id,
        owner = self.owner,
        total = self.total,
        members = members
    })
end

function Room:sync(data)
    for _,m in ipairs(self.members) do
        local session = sessionmgr:get_session(m.id)
        if session ~= nil then
            session:send(data)
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
        return require"roommanager":del_room(self.id)
    end
    self.total = #self.members * self.score

    -- deal with the owner
    if self.owner == uid then
        self.owner = self.members[1].id
    end

    self:sync({
        ec  = 0,
        cmd = 1105,
        room_id = self.id,
        owner = self.owner,
        total = self.total,
        members = self.members
    })

end

local M = {
    rooms = {}
}

function M:get_room(rid)
    rid = tonumber(rid)
    return self.rooms[rid]
end

function M:del_room(rid)
    rid = tonumber(rid)
    self.rooms[rid] = nil

end

function M:create_room(uid)
    -- local rid  = redis:incr("room.req")
    local rid  = 100
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
