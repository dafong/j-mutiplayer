import './js/libs/weapp-adapter'
import './js/libs/symbol'
import config from './js/config.js'
import util from './js/util.js'
import helper from './js/ui/help.js'
GameGlobal['g'] = {
    config : config,
    util   : util,
    h   : helper
}
g.h.init()


// import Main from './js/main'
//
// new Main()
//
import App from './js/app'

new App()
