import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
import MainPage from './mainpage.js'
export default class UI{

    constructor(){
        this.page = undefined
        var self = this
        canvas.addEventListener("touchstart",function(t) {
            if(self.page)
                self.page.ontouchstart(t)
        })
        canvas.addEventListener("touchend",function(t) {
            if(self.page)
                self.page.ontouchend(t)
        })
        canvas.addEventListener("touchmove",function(t) {
            if(self.page)
                self.page.ontouchmove(t)
        })
    }

    showMainPage(options){
        if(this.page)
            this.page.hide()
        this.page = new MainPage(options)
        this.page.show()
    }

    showBonusPage(){

    }

    showRoomPage(){

    }

    showGamePage(){

    }

    showEndPage(){

    }

    showWarnDialog(){

    }
}
