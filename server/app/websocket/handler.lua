local log = require"june.log"
local json = require"cjson"

local M = {
	cache = {}
}

function M:handle(session)
	session.handler = self
end

function M:process(data,session)
	local func = self.cache[data.t]
	if func then
		func(data,session)
	end
end

function M:use(cmd,func)
	self.cache[cmd] = func
end

return M
