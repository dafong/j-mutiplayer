import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
var pcx=207
export default class GamePage{

    constructor(){
        this.cur = 0
    }

    show(){
        g.h.clearRect(0,0,0)
        this.refresh_profile()
        this.refresh_member()
        this.refresh_indicator()
        g.h.showLayer([0,1,2,3])
    }

    refresh_profile(){
        g.h.drawImage("images/back.png",0,15,20,.5,.5)
        g.h.drawImage("images/roomno.png",0,100,20,.5,.5).then(function(){
            return g.h.drawText("房间："+1237,0,100,20,18,'#fff')
        })

        g.h.drawText("消耗:100  底分:400",1,80,50,15,'#29965e')
    }

    refresh_member(){
        var self = this
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
    }

    refresh_timmer(){

    }

    refresh_indicator(){
        g.h.clearLayer(3)
        var left = this.cur * (80+24) + 52
        g.h.drawImage("images/turnarrow.png",3,left,240,.5,0)
    }

    refresh(){

    }

    hide(){
        g.h.hideall()
    }

    ontouchstart(t,x,y){

    }

    ontouchend(t,x,y){


    }

    ontouchmove(t,x,y){

    }


}
