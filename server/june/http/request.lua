local log = require"june.log"
local json= require"cjson"

local M = {}

function M:new(http)
    local req = ngx.req
    req.read_body()
    local header = req.get_headers()
    local ctype = header['Content-Type']
    local arg = nil
    if ctype == "application/json" then
        local data = ngx.req.get_body_data()
        arg = json.decode(data)
    else
        arg = req.get_post_args()
    end
    local ins = {
        __modules = {},
        origin_uri = ngx.var.request_uri,
        path   = ngx.var.uri,
        headers= req.get_headers(),
        method = req.get_method(),
        post   = arg,
        query  = req.get_uri_args(),
        http   = http
    }

    setmetatable(ins,{
        __index = function(t,k)
            if self[k] then t[k] = self[k]; return self[k] end
            if t.__modules[k] then return t.__modules[k] end
        end,
        __newindex = function(t,k,v)
            if t.__modules[k] ~= nil then
                error("error when override module: ".. k)
            end
            rawset(t,k,v)
        end})
    return ins
end

function M:arg(key,default)
    local v = self.query[key] or self.post[key]
    if v == nil or type(v) == "boolean" then
        return default
    end
    return v
end

function M:reg_module(name,mod)
    if self.__modules[name] then error("same module with name "..name.." exist") end
    self.__modules[name]=mod
end

function M:unreg_module(name,mod)
    self.__modules[name] = nil
end

return M
