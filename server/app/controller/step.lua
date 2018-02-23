local M = {}

local util = require"utils"
local db = require"june.utils.mysql"
local redis = require"june.utils.redis"
local http = require"http"
local log = require"june.log"
local json = require"cjson"

function M:init(req,resp)
	local sid = req:arg('sid')
	log:i('sid=%s',req:arg('sid'))
	local data = http:get('https://api.weixin.qq.com/sns/jscode2session',{
		appid   = 'wx2b3304784ab26368',
		secret  = '8ee24638da73c816fc185ab65d9c69c3',
		js_code = sid,
		grant_type = 'authorization_code'
	})
	log:i(json.encode(data))
	-- get the user info and udid
	-- if the udid already in database ,the user is old
	-- format like userid openid other info
	-- if the udid is new, init the user data in database
	-- login success return userid and a token to client
	-- https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code
end


function M:createroom(req,resp)
	
	--room info save in redis
end

return M
