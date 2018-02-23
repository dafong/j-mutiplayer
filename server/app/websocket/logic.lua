local handler = require"websocket.handler"
local json = require"cjson"
handler:use("echo",function(data,session)

	session.uid = session.uid  + 1
	session:send_text(session.uid)
end)
--heartbeat
handler:use(101,function(data,session)
	session:send_cmd(101)
end)
--login
handler:use(102,function(data,session)

end)
