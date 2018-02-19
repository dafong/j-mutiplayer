local log = require"june.log"
return function(req,resp,invoke)
    if req.router == nil then return end
    if req.router.handler_func == nil or req.router.handler == nil or type(req.router.handler_func) ~= "function" then
        log:e("june default filter can't find a mapping_func of uri of "..req.path)
        ngx.status = 404
        return
    end
    req.router.handler_func(req.router.handler,req,resp)
    invoke()
end
