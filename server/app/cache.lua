-- off work process level cache
local Cache = {}

function Cache:new(count)
    local inst = {
        inner = {}
    }
    setmetatable(inst , { __index = self })
    return inst
end

function Cache:get(key)
    return self.inner[key]
end

function Cache:set(key,val)
    self.inner[key] = val
end

function Cache:del(key)
    self.inner[key] = nil
end


local M = {}

local _caches = {}

function M:init_cache(name,count)
    count = count or 200
    if _caches[name] == nil then
        _caches[name] = Cache:new(count)
    end
end

function M:get_cache(name)
    name = name or "default"
    return _caches[name]
end


return M
