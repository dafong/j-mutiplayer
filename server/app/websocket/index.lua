require"june.log":override_ngx_log()
-- require"june.log".rolling = true
math.randomseed(os.time())

require"config":init()
require"websocket.logic"
local session = require"websocket.sessionmanager":open()
require"websocket.handler":handle(session)
