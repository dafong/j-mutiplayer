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
            g.state.go("prepareing")
        }else if(evt == "room.create"){
            g.ui.showRoomPage()
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
                g.ui.showRoomPage()
            }else{
                g.state.go("idle")
            }
        }else if (evt == "room.memberchanged"){
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
            if(g.ui.page && g.ui.page.onMemberChanged){
                g.ui.page.onMemberChanged()
            }
        }
    }
}

//local player is play
class Play{

}
//local player is sync
class Sync{

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
        this.state  = undefined
	}

    go(stateName,para){
        this.state = this.states[stateName]
        this.state.onEnter(para)
    }

    msg(evtname,para){
        this.state.msg(evtname,para)
    }
}