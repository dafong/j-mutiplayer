local M = {}

local util = require"utils"
local db = require"june.utils.mysql"
local redis = require"june.utils.redis"

local log = require"june.log"

function M:redis(req,resp)
	ngx.say(redis:get("name"))
end

function M:mysql(req,resp)
	local row = db:query("select * from test.user")
	ngx.say(json.encode(row))
	row = db:query("update test.user set name='asd' where id = 1 ")
	ngx.say(json.encode(row))
end

function M:json(req,resp)
	resp:json({a = req:arg("a","defaulta")})
end

function M:redirect(req,resp)
	resp:redirect("http://www.zhihu.com")
end

function M:html(req,resp)
	resp:render("main/index.html",{
		wl  = "hi",
		name= "fxl"
	})
end



return M
