local M = {}

local util = require"utils"
local db = require"june.utils.mysql"
local redis = require"june.utils.redis"
local http = require"http"
local log = require"june.log"
local json = require"cjson"
local md5 = require"resty.md5"

-- local sid = req:arg('sid')
-- log:i('sid=%s',req:arg('sid'))
-- local data = http:get('https://api.weixin.qq.com/sns/jscode2session',{

-- 	appid   = 'wx2b3304784ab26368',
-- 	secret  = '8ee24638da73c816fc185ab65d9c69c3',
-- 	js_code = sid,

-- 	grant_type = 'authorization_code'
-- })
-- log:i(json.encode(data))

function M:init(req,resp)
	if req.player.id == nil then
		-- create user
		local row   = db:query("insert into player(score) values (0)")
		local token = util:md5(row.insert_id .. "")
		local ret = db:query(string.format('update player set token = "%s" where id = %s',token,row.insert_id))
		ngx.say(json.encode(ret))
		req.cookie:set({
			key='token',
			value=token,
			path="/",
			max_age = 3600
		})
		-- row.insert_id
		-- db.query("")
		-- row.insert_id
	else
		log:i("logined user with id %s",id)
		-- old user
	end

	-- get the user info and udid
	-- if the udid already in database ,the user is old
	-- format like userid openid other info
	-- if the udid is new, init the user data in database
	-- login success return userid and a token to client
	-- https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code
end

function M:ping(req,resp)
	req.cookie:set({
		key='token',
		value='1111',
		domain='127.0.0.1:8000',
		path="/",
		max_age = 3600
	})
	log:i("cookie: %s",req.cookie:get("token"))
end


function M:createroom(req,resp)

	--room info save in redis
end

return M
