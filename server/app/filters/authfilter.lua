local log = require"june.log"
return function(req,resp)
    if req.query.user == nil then
        ngx.status = 403
        return false
    end
end
