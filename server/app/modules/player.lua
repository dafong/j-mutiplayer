local M={}
local utils  = require"utils"
local log = require"june.log"
local redis = require"june.utils.redis"

function M:process(req,resp)

    local token = req.cookie:get("token")
    local id = redis:get(table.concat({'token.',token}))

    if id == ngx.null then id = nil end
    req:reg_module(self.name,{
        id = id
    })
end

setmetatable( M , { __call = function(t,name,conf)
    local ins = {name=name}
    setmetatable(ins,{ __index = M })
    return ins
end})

return M
