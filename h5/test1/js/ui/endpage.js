import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
var pcx=207
export default class EndPage{

    constructor(){

    }

    show(options){

        g.h.clearRect(0,0,0)
        var self = this
        g.h.drawImage("images/back.png",0,15,20,.5,.5)
        g.h.drawImage("images/roomno.png",0,100,20,.5,.5).then(function(){
            return g.h.drawText("房间："+1237,0,100,20,18,'#fff')
        })

        //good game
        g.h.drawText("GOOD GAME!",0,207,190,60,'#29965e')
        // the result

        var base = 260
        var drawRank = function(j){
            var top = base  + j * 54
            g.h.drawImage("images/endinfo.png",0,pcx,top + 7,.5,.5,380,40).then(function(){
                return g.h.drawText("大飘飘飘来飘去",0,80,top + 7,12,'#fff')
            }).then(function(){

            })
        }

        for(var i = 0;i<4;i++){
            drawRank(i)
        }

        //temp test
        g.h.drawImage("images/commonbt.png",0,pcx,600,.5,.5).then(function(){
            return g.h.drawText("再来一局",0,pcx,600,20,'#fff')
        })


        g.h.showLayer([0])

    }

    refresh(){

    }

    hide(){
        g.h.hideall()
    }

    ontouchstart(t,x,y){
        // if(x > 146 && x < 265 && y > 578 && y < 621 ){
        //     this.evt = "create_room"
        //     return true;
        // }
        //
        // if(x > 146 && x < 265 && y > 678 && y < 720 ){
        //     this.evt = "reset"
        //     return true;
        // }
        //
        // this.evt = undefined
    }

    ontouchend(t,x,y){
        // if(this.evt == "create_room"){
        //     console.log("create room clicked")
        // }
        // if(this.evt == "reset"){
        //     g.step.reset()
        // }
    }

    ontouchmove(t,x,y){

    }


}
