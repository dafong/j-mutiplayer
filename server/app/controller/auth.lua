local M={}
function M:index(req,resp)
    ngx.say("you are in auth")
end

return M
