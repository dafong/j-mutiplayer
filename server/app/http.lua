local http = require "resty.http"
local cfg  = require "config"
local cjson= require "cjson"
local log = require "june.log"
local M = {}
local function paramize(params)
    local t = {}
    for k,v in pairs(params) do
        t[#t+1] = table.concat({ k, "=", v })
    end
    return table.concat(t,'&')
end

function M:post(url,params)
    local hc = http.new()
    local res,err = hc:request_uri(url,{
        method = "POST",
        body = paramize(params)
    })
	if not res then
        return { ec = -1,em = err}
	end
    local data = rcjson.decode(res.body)
    return data
end

function M:get(url,params)
    local hc = http.new()
    local p  = "?"..paramize(params)
    log:i(p)
    local res,err = hc:request_uri(string.format("%s%s",url,#p > 1 and p or ""),{
        method = "GET"
    })
    if not res then
        return { ec = -1,em = err}
    end
    local data = cjson.decode(res.body)
    return data
end



return M
