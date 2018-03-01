require"june.extension"
local sformat  = string.format
local dgetinfo = debug.getinfo

local FILE_NAME= "app.log"
local shorts = {
   [ngx.STDERR] = "f",
   [ngx.EMERG] = "f",
   [ngx.ALERT] = "f",
   [ngx.CRIT] = "f",
   [ngx.ERR] = "e",
   [ngx.WARN] = "w",
   [ngx.NOTICE] = "n",
   [ngx.INFO] = "i",
   [ngx.DEBUG] = "d"
}

local _M = {
    level  = ngx.DEBUG,  -- same as https://github.com/openresty/lua-nginx-module#nginx-log-level-constants ngx.ERR = 4  ngx.DEBUG=8
    rolling= false,      -- if make the log rolling
    path   = "logs",     -- log path
    filename= FILE_NAME
}
local fd     = nil

local function get_log_path()
    local fn = _M.filename
    local fp = _M.path
    if not fp:start_with("/") then
        fp = sformat("%s/%s",ngx.var.document_root,fp)
    end
    if _M.rolling then
        local nt = ngx.localtime():sub(1,10)
        fn = sformat("%s.%s",fn,nt)
    end
    return sformat("%s/%s",fp,fn)
end

local function write(lv,s)
    local info=dgetinfo(3,"Sl")
    -- ngx.worker.id()
    local s = sformat("[%s][%s]%s:%d %s\n",ngx.localtime():sub(6,19),shorts[lv],info.short_src,info.currentline,s)
    if fd == nil then
        fd  = io.open(get_log_path(),"a+")
    end
    fd:write(s)
    fd:flush()
end

function _M:i(...)
    if ngx.INFO > self.level then return end
    write(ngx.INFO,sformat(...))
end

function _M:d(...)
    if ngx.DEBUG > self.level   then return end
    write(ngx.DEBUG,sformat(...))
end

function _M:trace (level)
    level = level or 2
    local buffer = {"\n"}
    while true do
        local info = debug.getinfo(level, "Sl")
        if not info then break end
        if info.what ~= "C" then
            buffer[#buffer+1] = string.format("[%s]:%d\n",info.short_src, info.currentline)
        end
        level = level + 1
    end
    return table.concat(buffer)
end

function _M:e(...)
    if ngx.ERR  > self.level then return end
    local trace = self:trace(3)
    write(ngx.ERR,sformat(...) .. trace)
end

function _M:override_ngx_log()
    ngx.log = function(lv,...)
        write(lv,...)
    end
end

return _M
