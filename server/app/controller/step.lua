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
		redis:set("token."..token,row.insert_id.."")
		resp:json({
			ec = 0,
			data = {
				uid = row.insert_id,
				token = token
			}
		})
	else
		log:i("logined user with id %s",req.player.id)
		resp:json({
			ec = 0,
			data = {
				uid = req.player.id,
				token = util:md5(req.player.id)
			}
		})
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

end


function M:createroom(req,resp)
	local room_id = redis:incr("room.req")
	redis:hset("room."..room_id,"owner",req.player.id)
	resp:json({
		ec = 0,
		data = {
			room_id = room_id
		}
	})

	--save in redis and record the owner expire time is 60 * 5

	--room info save in redis
end

function M:joinroom(req,resp)
	-- join the room and broadcast msg through socket
end

return M
