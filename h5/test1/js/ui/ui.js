import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
import MainPage from './mainpage.js'
import RoomPage from './roompage.js'
import EndPage from './endpage.js'
import GamePage from './gamepage.js'
export default class UI{
    constructor(){
        this.page = undefined
        this.toastQueue = []
        this.toastShowing = false
    }

    ontouchstart(t,x,y){
        if(this.blockTouch) return true
        if(this.page){
            return this.page.ontouchstart(t,x,y)
        }
    }

    ontouchmove(t,x,y){
        if(this.blockTouch) return true
        if(this.page){
            return  this.page.ontouchmove(t,x,y)
        }
    }

    ontouchend(t,x,y){
        if(this.blockTouch) return true
        if(this.page){
            return this.page.ontouchend(t,x,y)
        }
    }

    update(delta){

    }

    toast(param){
        if(param == undefined) return
        param.icon = param.icon || 'success'
        param.duration = param.duration || 2000
        if(!this.toastShowing){
            this.showToast(param)
        }else{
            this.toastQueue.push(param)
        }
    }

    loading(param){
        this.blockTouch = true
        wx.showLoading(param)
    }

    hideLoading(){
        this.blockTouch = false
        wx.hideLoading()
    }


    nextToast(){
        var param = this.toastQueue.shift()
        if(param)
            this.showToast(param)
        else
            this.toastShowing = false
    }

    showToast(param){
        this.toastShowing = true
        wx.showToast(param)
        setTimeout(this.nextToast.bind(this),param.duration)
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
