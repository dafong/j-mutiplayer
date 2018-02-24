local M = {}

local util = require"utils"
local db = require"june.utils.mysql"
local redis = require"june.utils.redis"
local json = require"cjson"
local log = require"june.log"
local resty = require"resty.md5"
local cache = require"cache"

function M:redis(req,resp)
	ngx.say(redis:hget("testkey","property"))
end

function M:mysql(req,resp)

	local counter = cache:get_cache():get("counter")
	counter = counter or 0
	counter = counter + 1
	cache:get_cache():set("counter",counter)
	log:i("c = %d",counter)
	ngx.say(util:md5("1"))
	-- local row = db:query("insert into step.player(score) values (0)")
	-- insert_id
	-- ngx.say(json.encode(row))
	-- local row = db:query("select * from test.user")
	-- ngx.say(json.encode(row))
	-- row = db:query("update test.user set name='asd' where id = 1 ")
	-- ngx.say(json.encode(row))
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
