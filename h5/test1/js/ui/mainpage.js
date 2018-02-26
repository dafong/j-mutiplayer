import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'

export default class MainPage{

    constructor(){

    }

    show(){
        g.h.clearRect(0,0,0)
        var self = this
        g.h.drawImage("images/bonusbt.png",0,10,10,0,0)
        g.h.drawImage("images/commonbt.png",0,207,600,.5,.5).then(function(){
            return g.h.drawText("创建房间",0,207,600,20,'#fff')
        })
        g.h.drawImage("images/name.png",0,207,200,.5,.5)
        g.h.showLayer([0,1])

        //temp test
        g.h.drawImage("images/commonbt.png",0,207,700,.5,.5).then(function(){
            return g.h.drawText("reset",0,207,700,20,'#fff')
        })
    }

    refresh(){

    }

    hide(){
        g.h.clearall()
    }

    ontouchstart(t,x,y){
        if(x > 146 && x < 265 && y > 578 && y < 621 ){
            this.evt = "create_room"
            return true;
        }

        if(x > 146 && x < 265 && y > 678 && y < 720 ){
            this.evt = "reset"
            return true;
        }

        this.evt = undefined
    }

    ontouchend(t,x,y){
        if(this.evt == "create_room"){
            g.network.createroom()
        }
        if(this.evt == "reset"){
            g.step.reset()
        }
    }

    ontouchmove(t,x,y){

    }


}
