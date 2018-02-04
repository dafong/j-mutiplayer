import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'

export default class MainPage{

    constructor(){

    }

    show(){
        g.h.clearRect(0,0,0)
        var self = this
        g.h.drawImage("images/bonusbt.png",0,10,10,0,0)
        g.h.drawImage("images/commonbt.png",0,207,500,.5,.5).then(function(){
            return g.h.drawText("创建房间",0,207,500,20,'#fff')
        })
        g.h.drawImage("images/name.png",0,207,200,.5,.5)
        g.h.showLayer([0,1])
    }

    refresh(){

    }

    hide(){
        g.h.hideall()
    }

    ontouchstart(t,x,y){

    }

    ontouchend(t,x,y){
        if(x > 146 && x < 265 && y > 476 && y <520 ){
            console.log("btn clicked create room ")
            return;
        }else{
            console.log("start game")
        }


    }

    ontouchmove(t,x,y){

    }


}
