local server = require "resty.websocket.server"
local log  = require"june.log"
local json = require"cjson"
local sessionmgr = require "websocket.sessionmanager"
local semaphore = require "ngx.semaphore"
local M = {}


local read_function = function(session)
	return function()
		if not  session.connected then return end
		while true do
			local data, typ, err = session:recv_frame()
			while err == 'again' do
				local cut_data
		        cut_data, _, err = session:recv_frame()
		        data = data .. cut_data
			end

			if not data or data == ""  then
				log:e("[session err]" .. err)
				break
			end
			-- log:i("[session recv] %s %s",session.sid,data )
			xpcall(function()
				local msg  = json.decode(data)
				session.handler:process(msg,session)
			end,function(error)
				log:e(error)
			end)
		end

		session:close()
	end
end

local write_function = function(session)
	return function()
		local sema = session.sema
		while session.connected do
			local ok,err = sema:wait(60)
			if ok then
				session:send_in_queue()
			end
		end
	end
end

function M:new(sessionid)
	local ins = {
		sid       = sessionid,
		uid       = nil,
		rid       = 0,
		wb        = nil,
		connected = false,
		lastheart = 0,
		is_auth   = false,
		towrite   = {},
		first     = 0,
		last      = -1
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
	self.wb   = wb
	self.sema = semaphore:new()
	self.read = ngx.thread.spawn(read_function(self))
	self.write= ngx.thread.spawn(write_function(self))
end

function M:auth(uid)
	self.uid = uid
	self.is_auth = true
	sessionmgr:onauth(self)
end

function M:send(data)
	if data == nil then return end
	self.last = self.last + 1
	self.towrite[self.last] = data
	self.sema:post(1)
end

function M:send_in_queue()
	while true do
		if self.first <= self.last then
			local m = self.towrite[self.first]
			if m ~= nil then self:_send_json(m) end
			self.first = self.first + 1
		else
			break
		end
	end
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

function M:_send_text(data)
	local bytes, err = self.wb:send_text(data)
	if bytes == nil then
		log:e(err)
	end
	return bytes
end

function M:_send_json(tbl)
	self:_send_text(json.encode(tbl or {}))
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


return M
