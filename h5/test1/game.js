import './js/libs/weapp-adapter'
import './js/libs/symbol'
GameGlobal['g'] = {}
import config from './js/config.js'
g.config = config
import util from './js/util.js'
g.util = util
import helper from './js/ui/help.js'
g.h = helper

g.h.init()


// import Main from './js/main'
//
// new Main()
//
import App from './js/app'

new App()
