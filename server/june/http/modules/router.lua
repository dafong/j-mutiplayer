local M={}
local utils = require"utils"
local log = require"june.log"
function M:process(req,resp)
    local path = ngx.re.sub(req.origin_uri, "^/+", "")
    path       = ngx.re.sub(path, "\\?.*", "")
    local pathinfo = path:split("/")
    local ok,e,func = pcall(function()
        local c = require("controller."..pathinfo[1])
        if type(c[pathinfo[2]]) ~= "function" then
            -- error("not a function")
            return nil,nil
        end
        return c,c[pathinfo[2]]
    end)

    if ok then
        req:reg_module(self.name,{
            handler = e,
            handler_func = func
        })
    else
        ngx.status = 404
        log:e(e)
    end
end

setmetatable( M , { __call = function(t,name,conf)
    local ins = { name = name}
    setmetatable(ins,{ __index = M })
    return ins
end})

return M
