local req = ngx.req

local M = {}

function M:new(http)
    req.read_body()
    local ins = {
        __modules = {},
        origin_uri = ngx.var.request_uri,
        path   = ngx.var.uri,
        headers= ngx.req.get_headers(),
        method = req.get_method(),
        post   = req.get_post_args(),
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
