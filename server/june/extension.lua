local function split(str,sp)
    if sp == nil or sp=="" then return {str} end
    if str == nil then return {} end
    local t = {}
    for m in ngx.re.gmatch(str,"[^"..sp.."]+") do
        table.insert(t,m[0])
    end
    return t
end

local function start_with(str,sp)
    if sp == nil or sp=="" then return false end
    if str == nil then return false end
    return ngx.re.match(str,"^"..sp) ~= nil
end

local function dir_name(str)
    if str == nil then return nil end
    str = ngx.re.sub(str,"/+$","")
    local from,to = ngx.re.find(str,"(.*)/[^/]+","",nil,1)
    if from == nil then return "" end
    return str:sub(from,to)
end

string.split = split
string.start_with = start_with
string.dir_name = dir_name
