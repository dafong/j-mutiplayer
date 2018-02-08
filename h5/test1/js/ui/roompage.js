import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
var pcx=207
export default class RoomPage{

    constructor(){

    }

    show(){

        g.h.clearRect(0,0,0)
        // g.h.fillStyle(0,'#fcf3bd')
        // g.h.fillRect(0,0,0,g.config.design.x,g.config.design.y)

        var self = this
        g.h.drawImage("images/back.png",0,15,20,.5,.5)
        g.h.drawImage("images/roomno.png",0,100,20,.5,.5).then(function(){
            return g.h.drawText("房间："+1237,0,100,20,18,'#fff')
        })

        g.h.drawText("消耗:100  底分:400",1,80,50,15,'#29965e')

        var drawmember = function(j){
            var left = j * (80+24) + 12
            g.h.drawImage("images/playerinfo-"+(j+1)+".png",1,left,80,0,0,80,134).then(function(){
                return g.h.drawText("大飘飘飘来飘去",1,left + 10 + 80/2,160,10,'#fff')
            }).then(function(){
                return g.h.drawText("123",1,left + 10 + 80/2,175,10,'#fff')
                //draw money
            }).then(function(){
                var txt = j == 0 ? "房主" : "已准备"
                return g.h.drawText(txt,1,left + 10 + 80/2,185,10,'#fff')
            })
        }

        for(var i=0;i<4;i++){
            drawmember(i)
        }


        //temp test
        g.h.drawImage("images/commonbt.png",0,pcx-100,600,.5,.5).then(function(){
            return g.h.drawText("邀请朋友",0,pcx - 100,600,20,'#fff')
        })

        g.h.drawImage("images/commonbt.png",0,pcx+100,600,.5,.5).then(function(){
            return g.h.drawText("准备开始",0,pcx + 100,600,20,'#fff')
        })

        g.h.showLayer([0,1,2])
    }

    refresh(){

    }

    hide(){
        g.h.hideall()
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
            console.log("create room clicked")
        }
        if(this.evt == "reset"){
            g.step.reset()
        }
    }

    ontouchmove(t,x,y){

    }


}
