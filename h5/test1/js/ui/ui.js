import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
import MainPage from './mainpage.js'

export default class UI{
    constructor(){
        this.page = undefined
        var self = this
        canvas.addEventListener("touchstart",function(t) {
            var x = ~~tx(t.changedTouches[0].clientX)
            var y = ~~ty(t.changedTouches[0].clientY)
            console.log("[touch] start " + x +" " + y)
            if(self.page)
                self.page.ontouchstart(t,x,y)
        })
        canvas.addEventListener("touchend",function(t) {
            var x = ~~tx(t.changedTouches[0].clientX)
            var y = ~~ty(t.changedTouches[0].clientY)
            console.log("[touch] end " + x +" " + y)
            if(self.page)
                self.page.ontouchend(t,x,y)
        })
        canvas.addEventListener("touchmove",function(t) {
            var x = ~~tx(t.changedTouches[0].clientX)
            var y = ~~ty(t.changedTouches[0].clientY)
            console.log("[touch] end " + x +" " + y)
            if(self.page)
                self.page.ontouchmove(t,x,y)
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
