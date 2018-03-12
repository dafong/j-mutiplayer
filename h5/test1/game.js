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

var schems = {
	default: ['#fff','#222'],
	state : ['#222','#bada55'],
	step: ['#222','#f79a30'],
	room  : ['#222','#f7f630'],
	player : ['#222','#30cdf7'],
	other: ['#222','#f74730'],
	other1: ['#222','#fb85de']

}

GameGlobal.RoomState = RoomState
GameGlobal.Type = Type
GameGlobal.log=function(){
	var c,s
	if(arguments.length > 1){
		c= arguments[0]
		s= arguments[1]
	}else{
		s= arguments[0]
	}

	var cols = schems[c] || schems.default
	console.log(`%c ${s}`,`background:${cols[0]} ; color: ${cols[1]};`)
}

g.h.init()

// import Main from './js/main'
//
// new Main()
//
import App from './js/app'

new App()
