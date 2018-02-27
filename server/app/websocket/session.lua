local server = require "resty.websocket.server"
local log  = require"june.log"
local json = require"cjson"
local sessionmgr = require "websocket.sessionmanager"
local M = {}

function M:new(sessionid)
	local ins = {
		sid       = sessionid,
		uid       = nil,
		rid       = 0,
		wb        = nil,
		connected = false,
		lastheart = 0,
		is_auth   = false
    }
    setmetatable(ins,{__index = self})
    ins:_init()
    return ins
end

function M:update_heartbeat()
	self.lastheart = os.time()
end

function M:_init()
	local wb, err = server:new{
		max_payload_len = 65535,
	}
	if not wb then
		log:e( "failed to new websocket: " .. err)
		return ngx.exit(444)
	end
	log:i("[session open] %s",self.sid)
	self.connected = true
	self.lastheart = os.time()
	wb:set_timeout(30*1000)
	self.wb = wb
end

function M:auth(uid)
	self.uid = uid
	self.is_auth = true
	sessionmgr:onauth(self)
end

function M:room(rid)
	self.rid = rid
end

function M:is_alive()
	return self.connected  and os.time() - self.lastheart < 30
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
	log:i("[session close] %s",self.sid )
	local bytes,err = self.wb:send_close(...)
	self.connected = false
	if bytes == nil then
		log:e(err)
	end
	self:on_close()
	require "websocket.sessionmanager":remove_session(self)
	return bytes
end

function M:on_close()
	require"roommanager":exit_room(self.uid,self.rid)
end

function M:send_text(data)
	local bytes, err = self.wb:send_text(data)
	if bytes == nil then
		log:e(err)
	end
	return bytes
end

function M:send_json(tbl)
	self:send_text(json.encode(tbl or {}))
end


function M:send_cmd(cmd,tbl)
	self:send_json({
		cmd = cmd,
		data = tbl or {}
	})
end


function M:send_binary(data)
	local bytes, err = self.wb:send_binary(data)
	if bytes == nil then
		log:e(err)
	end
	return bytes
end


return M
