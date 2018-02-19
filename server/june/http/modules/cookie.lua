local M={}
local utils  = require"utils"
local cookie = require"resty.cookie"
function M:process(req,resp)
    local c = cookie:new()
    req:reg_module(self.name,c)
    resp:reg_module(self.name,c)
end

setmetatable( M , { __call = function(t,name,conf)
    local ins = {name=name}
    setmetatable(ins,{ __index = M })
    return ins
end})

return M
