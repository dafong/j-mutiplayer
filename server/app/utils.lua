local M={}
local resty_md5 = require"resty.md5"
local resty_string = require"resty.string"


function M:md5(str)
    local md5 = resty_md5:new()
    md5:update(str)
    return resty_string.to_hex(md5:final())
end

function M:merge(...)
    local t = {}
    for _,o in ipairs({...}) do
        for k,v in pairs(o) do
            if type(v)~="table" then
                t[k] = v
            else
                t[k]=self:merge(t[k],v)
            end
        end
    end
end

local function tbl_2_str(self,t)
    local address = {}
    if type(t) == "userdata" then
        return tbl_2_str(self,getmetatable(t))
    end
    if type(t) ~= "table" then
        print(t)
        return
    end
    address[t]=0
    local ret = ""
    local space, deep = string.rep(' ', 4), 0
    local function _dump(t)
        local temp = {}
        for k,v in pairs(t) do
            local key = tostring(k)
            if type(v) == "table" and not address[v] then
                address[v] = 0
                deep = deep + 2
                ret = ret .. string.format("%s[%s] => Table\n%s(\n",string.rep(space, deep - 1),key,string.rep(space, deep))
                _dump(v)
                ret = ret ..string.format("%s)\n",string.rep(space, deep))
                deep = deep - 2
            else
                if type(v) ~= "string" then v = tostring(v) end
                ret = ret ..string.format("%s[%s] => %s\n",string.rep(space, deep + 1),key,v)
            end
        end
    end
    ret = ret ..(string.format("Table\n(\n"))
    _dump(t)
    ret = ret ..(string.format(")\n"))
    return ret
end

M.tbl_2_str = tbl_2_str

return M
