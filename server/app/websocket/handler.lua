local log = require"june.log"
local json = require"cjson"

local M = {
	cache = {}
}

function M:handle(session)

	if not  session.connected then
		return
	end
	while true do
		local data, typ, err = session:recv_frame()
		while err == 'again' do
			local cut_data
	        cut_data, _, err = session:recv_frame()
	        data = data .. cut_data
		end

		if not data then
			log:e("rev frame err:" .. err)
			break
		end
		log:i("rec data: %s %s",session.sid,data )


		xpcall(function()
			local msg = json.decode(data)
			local func = self.cache[msg.cmd]
			if func then
				func(msg,session)
			end
		end,function(error)
			log:e(error)
		end)

		-- local bytesend = nil
		-- if typ == 'ping' then
		-- 	bytesend = session:send_pong(data)
		-- elseif typ == 'close' then
		-- 	break
		-- elseif typ == 'text' then
		-- 	bytesend = session:send_text(data)
		-- elseif typ == 'binary' then
		-- 	bytesend = session:send_binary(data)
		-- else
		--
		-- end
	end
	session:close()
end

function M:use(cmd,func)
	self.cache[cmd] = func
end

return M
