import './js/libs/weapp-adapter'
import './js/libs/symbol'
GameGlobal['g'] = {}
import config from './js/config.js'
g.config = config
import util from './js/util.js'
g.util = util
import helper from './js/ui/help.js'
g.h = helper
var RoomState = {
	None : 0,
	Preparing : 1,
	Start : 2,
	End : 3
}

var Type = {
	GameInit : 0,
	RoundChange : 1
}
GameGlobal.RoomState = RoomState
GameGlobal.Type = Type


g.h.init()


// import Main from './js/main'
//
// new Main()
//
import App from './js/app'

new App()
