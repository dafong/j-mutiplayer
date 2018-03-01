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
	local room = roommgr:get_room(data.room_id)
	if room == nil then
		return session:send_json{
			cmd = 1105,
			ec  = 1002
		}
	end
	room:join(session.uid)
end)

-- prepare game
handler:use(105,function(data,session)
	local room = roommgr:get_room(session.rid)
	if room == nil then
		return session:send_json{
			cmd = 1106,
			ec  = 1002
		}
	end
	room:prepare(session.uid)
end)

-- jump start
handler:use(106,function(data,session)
	local room = roommgr:get_room(session.rid)
	if room == nil then
		return session:send_json{
			cmd = 1106,
			ec  = 1002
		}
	end
	room:jump_start(session.uid)
end)

-- jump end
handler:use(107,function(data,session)
	local room = roommgr:get_room(session.rid)
	if room == nil then
		return session:send_json{
			cmd = 1106,
			ec  = 1002
		}
	end
	room:jump_end(session.uid,data)

	-- if landing successful then next send room state change

	-- if landing fail  , end game and send room state change
end)
