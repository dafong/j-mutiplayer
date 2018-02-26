

local ngx_worker_id = ngx.worker.id()
local _incr_id = 0

local _gen_session_id = function()
	_incr_id = _incr_id + 1
	return (ngx_worker_id + 1) * 100000 + _incr_id
end

local M = {
	sessions = {},
	utos = {}
}

function clear()
	self.sessions = {}
end

function M:open()
	local session = require"websocket.session"
	local sid = _gen_session_id()
	local se  = session:new(sid)
	if se then
		self.sessions[sid] = se
	end
	return se
end

function M:onauth(session)
	self.utos[session.uid] = session
end

function M:get_session(uid)
	return self.utos[uid]
end


function M:remove_session(s)
	self.utos[s.uid] = nil
	self.sessions[s.sid] = nil

end



return M
