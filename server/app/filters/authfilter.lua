local log = require"june.log"
return function(req,resp)
    local token = req.cookie.get("token")
    if token then
        
    end
    if req.query.user == nil then
        ngx.status = 403
        return false
    end
end
