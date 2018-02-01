import './js/libs/weapp-adapter'
import './js/libs/symbol'
import config from './js/config.js'
import util from './js/util.js'
GameGlobal['g'] = {
    config : config,
    util   : util
}

// import Main from './js/main'
//
// new Main()
//
import App from './js/app'

new App()
