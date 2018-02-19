local log = require"june.log"
local M={}

local function handler(self,req,resp)
    local filters = req[self.name].all
    if #filters == 0 then return end
    local idx = 0
    local invoke
    invoke = function()
        idx = idx + 1
        if idx > #filters then return end
        filters[idx](req,resp,invoke)
    end
    invoke()
end

function M:process(req,resp)
    local fs = {}
    for _,f in ipairs(self.filters) do
        if f.pattern == nil or ngx.re.match(req.origin_uri,f.pattern) then
            table.insert(fs, f.handler)
        end
    end
    req:reg_module(self.name,{
        all = fs
    })
    return handler
end

local utils = require"utils"

function M:init(conf)
    local c = conf.filters or {}
    if type(c) == "function" then
        c(self)
    end
end

function M:add(...)

    local p = {...}
    if #p == 0 then
        return
    end

    local check = function(p)
        if type(p) ~= "function" then
            error("error when add global filter #:"..#self.filters + 1)
        end
    end

    if #p == 1 then
        local func = require(p[1])
        check(func)
        table.insert(self.filters,{pattern = nil , handler = func })
    else
        local func = require(p[2])
        check(func)
        table.insert(self.filters,{pattern = p[1] , handler = func })
    end
end


setmetatable( M , {
    __tostring = function(t)

    end,

    __call = function(t,name,conf)
        local ins = {
            name = name,
            filters = {}
        }
        setmetatable(ins,{ __index = M })
        ins:init(conf)
    return ins
end})

return M
