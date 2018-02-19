local server = require "resty.websocket.server"
local log = require"june.log"

local M = {}

function M:new(sessionid)
	local ins = {
		sid       = sessionid,
		uid       = 0,
		wb        = nil,
		connected = false
    }
    setmetatable(ins,{__index = self})
    ins:_init()
    return ins
end

function M:_init()
	local wb, err = server:new{
		max_payload_len = 65535,
	}
	if not wb then
		log:e( "failed to new websocket: " .. err)
		return ngx.exit(444)
	end
	self.connected = true
	wb:set_timeout(30*1000)
	self.wb = wb
end

function M:recv_frame()
	return self.wb:recv_frame()
end

function M:send_pong(data)
	local bytes,err = self.wb:send_pong(data)
	if bytes == nil then
		log:e(err)
	end
	return bytes
end

function M:close(...)
	local bytes,err = self.wb:send_close(...)
	self.connected = false
	if bytes == nil then
		log:e(err)
	end
	require "websocket.sessionmanager":remove_session(self)
	return bytes
end

function M:send_text(data)
	local bytes, err = self.wb:send_text(data)
	if bytes == nil then
		log:e(err)
	end
	return bytes
end

function M:send_binary(data)
	local bytes, err = self.wb:send_binary(data)
	if bytes == nil then
		log:e(err)
	end
	return bytes
end


return M
