local handler = require"websocket.handler"
local json = require"cjson"
local log = require"june.log"
local util = require"utils"
local redis = require"june.utils.redis"

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
	session.isAuth = true
	session.uid = uid
	session:send_json({
		cmd = 102,
		ec  = 0,
		sid = session.sid
	})
end)
