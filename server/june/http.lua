
local Request  = require"june.http.request"
local Response = require"june.http.response"
local log = require"june.log"

local M={}

function M:new(conf)
    local ins = {
        modules = {},
        conf    = conf.http or {},
        base_dir= ""
    }
    setmetatable(ins,{__index = self})
    ins:_init()
    return ins
end

function M:_init()
    local mf = self.conf.modules
    if mf == nil then
        self:_check_default_module()
        return
    end
    if type(mf) ~= "function" then
        return
    end
    mf(self)
end

function M:_check_default_module()
    if self.modules.router == nil then
        self:use("router","june.http.modules.router")
    end

    if self.modules.filter == nil then
        self:use("filter","june.http.modules.filter")
    end

    if self.modules.cookie == nil then
        self:use("cookie","june.http.modules.cookie")
    end
end

function M:use(name,luafile)
    if self.modules[name] ~= nil then
        error("module with same name " .. name .." already exist")
    end
    self.modules[name] = require(luafile)(name,self.conf)
end

function M:run()
    local req  = Request:new(self)
    local resp = Response:new(self)
    --remove leading /
    local path = ngx.re.sub(req.path, "^/+", "")
    self.base_url = path:dir_name()


    -- iterator all modules
    local i,handlers =0, {}

    for n,m in pairs(self.modules) do
        local handler = m:process(req,resp)
        if type(handler) == "function" then
            i=i+1
            handlers[i] = {m=m,h=handler}
        end
    end

    for _,handler in ipairs(handlers) do
        handler.h(handler.m,req,resp)
    end

end


return M
