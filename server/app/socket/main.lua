

local sock = assert(ngx.req.socket())
ngx.say("receivedqq: ")
ngx.log(ngx.ERR,"aaaaaa")
sock:settimeout(1000)
while true do
    local line, err, part = sock:receive()
    ngx.log(ngx.ERR,line)
    if line then
        ngx.say("receivedqq: ",line)
    else
        ngx.say("failed to receive a line: ", err, "[" , part , "]")
        break
    end
end
