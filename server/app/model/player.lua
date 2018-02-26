local json = require"cjson"
local log = require"june.log"
local util = require"utils"
local redis = require"june.utils.redis"
local db = require"june.utils.mysql"

local M={}

function M:get(uid)
    local ret = db.query(string.format("select * from player where id = %s", uid))
    return ret[1]
end

return M
