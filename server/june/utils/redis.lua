local utils = require"utils"
local log = require"june.log"
local redis = require"resty.redis"
local june = require"june.june"


local common_cmds = {
    "get",      "set",          "mget",     "mset",
    "del",      "incr",         "decr",                 -- Strings
    "llen",     "lindex",       "lpop",     "lpush",
    "lrange",   "linsert",                              -- Lists
    "hexists",  "hget",         "hset",     "hmget",
    --[[ "hmset", ]]            "hdel",                 -- Hashes
    "smembers", "sismember",    "sadd",     "srem",
    "sdiff",    "sinter",       "sunion",               -- Sets
    "zrange",   "zrangebyscore", "zrank",   "zadd",
    "zrem",     "zincrby",                              -- Sorted Sets
    "auth",     "eval",         "expire",   "script",
    "sort"                                              -- Others
}
local support = {}
for _,cmd in ipairs(common_cmds) do
    support[cmd] = true
end

local M={ }

setmetatable(M, { __index = function(t,k)
    if not support[k] then
        return log:e("redis wrapper not support cmd ".. k)
    end
    return function(self, ...)
        local c = redis:new()
        c:set_timeout(1000)
        local conf = require"config".redis
        local ok,err = c:connect(conf.host,conf.port)
        if not ok then log:e("redis connect error "..err) end
        local res,err = c[k](c , ... )
        if not res then
            log:e("failed to do cmd : ".. k)
            c:set_keepalive(60000,50)
            return nil
        end
        c:set_keepalive(60000,50)
        return res
    end
end  } )

return M
