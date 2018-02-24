local session = require"websocket.session"

local ngx_worker_id = ngx.worker.id()
local _incr_id = 0

local _gen_session_id = function()
	_incr_id = _incr_id + 1
	return (ngx_worker_id + 1) * 100000 + _incr_id
end

local M = {
	sessions = {}
}

function clear()
	self.sessions = {}
end

function M:open()
	local sid = _gen_session_id()
	local se  = session:new(sid)
	if se then
		self.sessions[sid] = se
	end
	return se
end



function M:remove_session(s)
	self.sessions[s.sid] = nil
end



return M
