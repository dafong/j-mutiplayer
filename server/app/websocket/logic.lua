local handler = require"websocket.handler"
local json = require"cjson"
local log = require"june.log"
local util = require"utils"
local redis = require"june.utils.redis"
local roommgr = require"roommanager"
--echo the uid sid
handler:use(1,function(data,session)
	session:send_json({
		cmd = 1,
		ec = 0,
		uid= session.uid
	})
end)

--heartbeat
handler:use(101,function(data,session)
	session:update_heartbeat()
	session:send_cmd(101)
end)

--auth
handler:use(102,function(data,session)
	local token = data.token
	local uid = redis:get(table.concat({"token.",token}))
	if uid == ngx.null then
		session:send_json({
			cmd = 102,
			ec  = 1001 -- user token invalid
		})
		return
	end
	log:i("[auth succ] %s %s",session.sid,uid)
	session:auth(uid)
	session:send_json({
		cmd = 102,
		ec  = 0,
		sid = session.sid
	})
end)

-- create room
handler:use(103,function(data,session)
	local room = roommgr:create_room()
	session:send_json({
		cmd = 103,
		score = room.score,
		ec  = 0,
		room_id = room.id,
	})
	room:join(session.uid)
end)


-- join room
handler:use(104,function(data,session)
	-- join the room and broadcast msg through socket
	local key = table.concant({"room." , data.room_id})
	local owner_id = redis:hget(key,"owner")
	if owner_id == ngx.null then
		return session:send_json{
			cmd = 104,
			ec  = 1002
		}
	end
	local state = redis:hget(key,"state")
	local members = json.decode(redis:hget(key,"members"))

	if state ~= 0 then
		return session:send_json{
			cmd = 104,
			ec = 1004
		}
	end


	if #members >= 4 then
		return session:send_json{
			cmd = 104,
			ec = 1003
		}
	end

	members[#members + 1] = session.uid
	redis:hset(key,"members",json.encode(members))
	session:send_json{
		cmd = 104,
		ec = 0
	}

	local room = roommgr:get_room(data.room_id)
	room:join(session.uid)

end)
