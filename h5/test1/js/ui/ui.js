import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
import MainPage from './mainpage.js'

export default class UI{
    constructor(){
        this.page = undefined

    }

    ontouchstart(t,x,y){
        if(self.page){
            self.page.ontouchstart(t,x,y)
        }
    }

    ontouchmove(t,x,y){
        if(this.page){
            this.page.ontouchmove(t,x,y)
        }
    }

    ontouchend(t,x,y){
        if(this.page){
            this.page.ontouchend(t,x,y)
        }
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
