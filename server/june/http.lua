
local Request  = require"june.http.request"
local Response = require"june.http.response"
local log = require"june.log"

local M={}

function M:new(conf)
    local ins = {
        modules = {},
        list    = {},
        conf    = conf.http or {},
        base_dir= ""
    }
    setmetatable(ins,{__index = self})
    ins:_init()
    return ins
end

function M:_init()
    self:_check_default_module()
    for k,v in pairs(self.conf) do
        if type(v) == "table" then
            if v.lua ~= nil then
                self:use(k,v.lua)
            end
        end
    end
end

function M:_check_default_module()

    if self.modules.cookie == nil then
        self:use("cookie","june.http.modules.cookie")
    end

    if self.modules.router == nil then
        self:use("router","june.http.modules.router")
    end

    if self.modules.filter == nil then
        self:use("filter","june.http.modules.filter")
    end

end

function M:use(name,luafile)
    if self.modules[name] ~= nil then
        error("module with same name " .. name .." already exist")
    end
    self.modules[name] = require(luafile)(name,self.conf)
    self.list[#self.list+1] = name
end

function M:run()
    local req  = Request:new(self)
    local resp = Response:new(self)
    --remove leading /
    local path = ngx.re.sub(req.path, "^/+", "")
    self.base_url = path:dir_name()


    -- iterator all modules
    local i,handlers =0, {}

    for _,n in pairs(self.list) do
        local m = self.modules[n]
        local handler = m:process(req,resp)
        if type(handler) == "function" then
            i=i+1
            handlers[i] = {m = m, h = handler}
        end
    end

    for _,handler in ipairs(handlers) do
        handler.h(handler.m,req,resp)
    end

end


return M
