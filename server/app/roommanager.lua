
local json = require"cjson"
local log = require"june.log"
local util = require"utils"
local redis = require"june.utils.redis"
local sessionmgr = require"websocket.sessionmanager"

local Room = {}

function Room:new(rid)
    local ins = {
        key = table.concat({"room.", rid}),
        id  = rid,
        owner = nil,
        score = 100,
        members = {
            -- {id,state}
        }
    }
    setmetatable(ins , { __index = self })
    ins:init()
    return ins
end

function Room:init()
    redis:hset(self.key,"state",0) -- state 0 is prepareing
    redis:expire(self.key,5 * 60)
end

function Room:chage_state()

end


function Room:join(uid)
    if self.owner == nil then
        self.owner = uid
        redis:hset(self.key,"owner",self.owner)
    end

    local members = redis:hget(self.key,"members")
    if members == ngx.null then
        members = {}
    else
        members = json.decode(members)
    end
    members[#members + 1] = uid
    self.members = members
    redis:hset(self.key,"members",json.encode(members))
    self:sync({
        ec  = 0,
        cmd = 1105,
        room_id = self.rid,
        owner = self.owner,
        members = members
    })
end

function Room:sync(data)
    for _,id in ipairs(self.members) do
        local session = sessionmgr:get_session(id)
        if session ~= nil then
            session:send_json(data)
        end
    end
end

function Room:leave()

end

local M = {
    rooms = {}
}

function M:get_room(rid)
    return self.rooms[rid]
end

function M:create_room(uid)
    local rid  = redis:incr("room.req")
    local room = Room:new(rid,uid)
    
    return room
end



return M
