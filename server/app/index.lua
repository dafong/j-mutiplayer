local function main()
    require"june.log":override_ngx_log()
    require"config":init()
    -- require"june.log".rolling = true
    local june = require"june.june"
    local http = june:create_http(require"config")
    http:run()
end
xpcall(main,function(error)
    pcall(function()
        local log  = require"june.log"
        log:e("error occured ! request uri:"..ngx.var.request_uri.."\n trace: "..error)
    end)
    ngx.status = 500
    ngx.say("server 500 error occured,trace:"..error)
end)
