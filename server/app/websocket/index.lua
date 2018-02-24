require"june.log":override_ngx_log()
-- require"june.log".rolling = true
require"config":init()
require"websocket.logic"
local session = require"websocket.sessionmanager":open()
require"websocket.handler":handle(session)
