local handler = require"websocket.handler"

handler:use("echo",function(data,session)

	session.uid = session.uid  + 1
	session:send_text(session.uid)
end)

handler:use("auth",function(data,session)

end)
