import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
import MainPage from './mainpage.js'
import RoomPage from './roompage.js'
import EndPage from './endpage.js'
import GamePage from './gamepage.js'
export default class UI{
    constructor(){
        this.page = undefined

    }

    ontouchstart(t,x,y){
        if(this.page){
            return this.page.ontouchstart(t,x,y)
        }
    }

    ontouchmove(t,x,y){
        if(this.page){
            return  this.page.ontouchmove(t,x,y)
        }
    }

    ontouchend(t,x,y){
        if(this.page){
            return this.page.ontouchend(t,x,y)
        }
    }

    update(delta){

    }

    showMainPage(options){
        if(this.page)
            this.page.hide()
        this.page = new MainPage(options)
        this.page.show()
    }

    showBonusPage(){

    }

    showRoomPage(options){
        if(this.page)
            this.page.hide()
        this.page = new RoomPage(options)
        this.page.show()
    }

    showGamePage(options){
        if(this.page)
            this.page.hide()
        this.page = new GamePage(options)
        this.page.show()
    }

    showEndPage(options){
        if(this.page)
            this.page.hide()
        this.page = new EndPage(options)
        this.page.show()
    }

    showWarnDialog(){

    }
}
