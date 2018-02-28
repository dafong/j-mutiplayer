import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
var pcx=207
export default class RoomPage{

    constructor(){

    }

    show(){
        this.refreshRoomInfo()
        g.h.showLayer([0,1,2])
    }

    refreshRoomInfo(){
        var self = this
        g.h.drawImage("images/back.png",0,15,20,.5,.5)
        g.h.drawImage("images/roomno.png",0,100,20,.5,.5).then(function(){
            return g.h.drawText("房间："+g.user.roomId,0,100,20,18,'#fff')
        })
    }

    refreshScore(){
        g.h.drawText(`消耗:${g.user.score}  底分:${g.user.totalScore}`,0,80,50,15,'#29965e')
    }

    refreshMember(){
        var self = this
        var drawmember = function(j){
            var name = "空"
            var score= 0
            var id = -1
            if(j <= g.user.members.length - 1){
                id = g.user.members[j].id
                name = g.user.members[j].id
                score= 100000
            }
            var left = j * (80+24) + 12
            g.h.drawImage("images/playerinfo-"+(j+1)+".png",1,left,80,0,0,80,134).then(function(){
                return g.h.drawText(`${name}`,1,left + 10 + 80/2,160,10,'#fff')
            }).then(function(){
                return g.h.drawText(`${score}`,1,left + 10 + 80/2,175,10,'#fff')
                //draw money
            }).then(function(){
                var txt = g.user.ownerId == id ? "房主" : ""
                return g.h.drawText(txt,1,left + 10 + 80/2,185,10,'#fff')
            })
            return id == g.user.curId
        }
        var idx = -1
        for(var i=0;i<4;i++){
            if(drawmember(i))
                idx = i
        }

        if(g.user.roomState == RoomState.Preparing){
            g.h.drawImage("images/commonbt.png",2,pcx-100,600,.5,.5).then(function(){
                return g.h.drawText("邀请朋友",2,pcx - 100,600,20,'#fff')
            })

            g.h.drawImage("images/commonbt.png",2,pcx+100,600,.5,.5).then(function(){
                return g.h.drawText("准备",2,pcx + 100,600,20,'#fff')
            })
        }else{
            g.h.clearLayer(2)
            var x = idx * (80 + 24) + (80 + 24) /2
            g.h.drawImage("images/turnarrow.png",2,x,230)
        }
    }

    onMemberChanged(){
        var mems = g.user.members
        this.refreshScore()
        this.refreshMember()
    }

    startGame(){
        this.refreshMember()
        
    }

    nextRound(){

    }

    refresh(){

    }

    hide(){
        g.h.clearall()
    }

    ontouchstart(t,x,y){
        if(g.user.roomState == RoomState.Preparing){
            if(x > 245 && x < 365 && y > 577 && y < 620){
                this.evt ="prepare"
                return true
            }
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
        if(this.evt == "prepare"){
            g.network.prepare()
        }
    }

    ontouchmove(t,x,y){

    }


}
