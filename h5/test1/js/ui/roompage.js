import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'
var pcx=207
export default class RoomPage{

    constructor(){

    }

    show(){
        this.refreshRoomInfo()
        g.h.showLayer([0,1,2,3])
    }

    refreshRoomInfo(){
        var self = this
        g.h.clearLayer(0)
        g.h.drawImage("images/back.png",0,15,20,.5,.5)
        g.h.drawImage("images/roomno.png",0,100,20,.5,.5).then(function(){
            return g.h.drawText("房间："+g.user.roomId,0,100,20,18,'#fff')
        })
    }

    refreshScore(){
        g.h.clearLayer(1)
        g.h.drawText(`消耗:${g.user.score}  底分:${g.user.totalScore}`,1,80,50,15,'#29965e')
    }

    refreshMember(){
        var self = this
        g.h.clearLayer(2)
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
            g.h.drawImage("images/playerinfo-"+(j+1)+".png",2,left,80,0,0,80,134).then(function(){
                return g.h.drawText(`${name}`,2,left + 10 + 80/2,160,10,'#fff')
            }).then(function(){
                return g.h.drawText(`${score}`,2,left + 10 + 80/2,175,10,'#fff')
                //draw money
            }).then(function(){
                var txt = g.user.ownerId == id ? "房主" : ""
                return g.h.drawText(txt,2,left + 10 + 80/2,185,10,'#fff')
            })
            return id == g.user.curId
        }
        var idx = -1
        for(var i=0;i<4;i++){
            if(drawmember(i))
                idx = i
        }

        if(g.user.roomState == RoomState.Preparing){
            g.h.clearLayer(3)
            g.h.drawImage("images/commonbt.png",3,pcx-100,600,.5,.5).then(function(){
                return g.h.drawText("邀请朋友",3,pcx - 100,600,20,'#fff')
            })

            g.h.drawImage("images/commonbt.png",3,pcx+100,600,.5,.5).then(function(){
                return g.h.drawText("准备",3,pcx + 100,600,20,'#fff')
            })
        }else{
            g.h.clearLayer(3)
            var x = idx * (80 + 24) + (80 + 24) /2
            g.h.drawImage("images/turnarrow.png",3,x,230)
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

            if(x > 45 && x < 165 && y > 578 && y < 620){
                this.evt = "invite"
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
        if(this.evt == "invite"){
            wx.shareAppMessage({
                title : '步步高升',
                query : `roomid=${g.user.roomId}`
    		})
        }
    }

    ontouchmove(t,x,y){

    }


}
