local utils = require"utils"
local log = require"june.log"
local mysql = require"resty.mysql"
local june = require"june.june"

local function query(this,statement,est_nrows)
    local db, err = mysql:new()
    assert(db,err)
    if not db then
        log:e("mysql create failed")
    end
    local conf = june.conf.mysql
    --time out 1sec
    db:set_timeout(1000)
    local res, err = db:connect(conf)
    if not res then
        log:e("mysql connect failed")
    end
    res, err, errno, sqlstate =  db:query(statement, est_nrows)

    if res ~= nil then
        local ok, err = db:set_keepalive(60000, 50)
        if not ok then
            log:e("mysql connection can't return pool,try close")
            db:close()
        end
    else
        log:e("mysql query with error "..err)
    end
    return res
end
return {
    query = query
}
