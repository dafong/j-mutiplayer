class Idle {

    onEnter(para){
        this.para = para
    }

    msg(evt,para){
        if(evt == "resume.roomId"){
            g.state.go("waitjoin",para)
        }else if (evt == "connecting") {
            g.ui.loading({ title : '连接中' })
        }else if (evt == "auth.complete"){
            g.ui.hideLoading()
        }else if (evt == "room.memberchanged"){
            g.user.onMemberChanged(para)
            g.state.go("prepareing")
        }else if(evt == "room.create"){
            if(para.ec == 0){
                g.user.initRoom(para)
                console.log("[room] [create success] 房间号="+para.room_id)
                g.ui.showRoomPage()
            }else{
                console.log("[room] [create failed] ")
            }

        }
    }

}

class WaitJoin{

    onEnter(para){
        this.para = para
    }

    msg(evt,para){
        if (evt == "connecting") {
            g.ui.loading({ title : '进入房间中..' })
        }else if (evt == "auth.complete"){
            if(para.ec == 0){
                g.network.join(this.para.roomId)
            }else{
                g.ui.hideLoading()
                g.state.go("idle")
            }
        }else if (evt == "room.join"){
            g.ui.hideLoading()
            if(para.ec == 0){
            	g.user.initRoom(para)
                g.ui.showRoomPage()
            }else{
                g.state.go("idle")
            }
        }else if (evt == "room.memberchanged"){
            g.user.onMemberChanged(para)
            g.state.go("prepareing")
        }
    }
}

class Prepareing{
    onEnter(para){
        this.para = para
        if(g.ui.page && g.ui.page.onMemberChanged){
            g.ui.page.onMemberChanged()
        }
    }

    msg(evt,para){
        if(evt == "room.memberchanged"){
            g.user.onMemberChanged(para)
            if(g.ui.page && g.ui.page.onMemberChanged){
                g.ui.page.onMemberChanged()
            }
        }else if(evt == "room.changed"){
            if(para.type == Type.GameInit){
                if(para.ec == 0){
                    g.user.startGame(para)
                    g.state.go('play')
                }
    		}
        }
    }
}

//local player is play
class Play{
    onEnter(para){
        this.para = para
        if(!g.user.isLocalRound){
            g.state.go('sync')
        }
    }

    msg(evt,para){
        if(evt == "room.jumpstart"){
            //do nothing
        }
        if(evt == "room.jumpend"){
            if(para.ec == 0){
                this.jumpdata = para
                g.user.notifyResult(para)
            }
        }
        if(evt == "local.roundover"){
            g.user.nextRound(this.jumpdata)
            if(!g.user.isLocalRound){
                g.state.go('sync')
            }
        }
    }
}
//local player is sync
class Sync{
    onEnter(para){
        this.para = para
    }
    msg(evt,para){
        if(evt == "room.jumpstart"){
            if(para.ec == 0){
                g.step.onSimJumpStart()
            }
        }
        if(evt == "room.jumpend"){
            if(para.ec == 0){
                g.user.notifyResult(para)
                this.jumpdata = para
                g.step.onSimJumpEnd(para)
            }
        }
        if(evt == "local.roundover"){
            g.user.nextRound(this.jumpdata)
            if(g.user.isLocalRound){
                g.state.go('play')
            }
        }
    }
}


//game end
class End{

}



export default class StateManager{
	constructor(){
        this.states = {}
        this.states["idle"] = new Idle()
        this.states["waitjoin"] = new WaitJoin()
        this.states["prepareing"] = new Prepareing()
        this.states["play"] = new Play()
        this.states["sync"] = new Sync()
        this.states['end'] = new End()
        this.state  = undefined
        this.name   = undefined
	}

    go(stateName,para){
        console.log(`%c [STATE] ${this.name} => ${stateName}`,'background: #222;color:#bada55')
        this.name  = stateName
        this.state = this.states[stateName]
        this.state.onEnter(para)
    }

    msg(evtname,para){
        this.state.msg(evtname,para)
    }
}
