import './js/libs/weapp-adapter'
import './js/libs/symbol'
import config from './js/config.js'
GameGlobal['g'] = {
    config : config
}

// import Main from './js/main'
//
// new Main()
//
import App from './js/app'

new App()
