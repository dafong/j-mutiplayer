

export default class User{
	constructor(){
		this.sessionId = undefined
		this.token = wx.getStorageSync("token")
		this.uid   = wx.getStorageSync("uid")
		this.result= undefined
		this.exitRoom()
	}

	exitRoom(){
		this.roomId   = -1
		this.members  = undefined
		this.roomState= RoomState.None
		this.score    = 0
		this.totalScore = 0
		this.ownerId  = -1
		this.curId  =  -1
		this.dir = undefined
		this.dis = undefined
		this.isLocalRound = false
	}

	penddingResult(){
		if(this.result == undefined){
			this.result = {
				pendding : true,
				callback : undefined,
				data : undefined,
				oncomplete : function(func){
					if(this.pendding == false){
						func(this.data)
					}else{
						this.callback = func
					}
				},
				notify : function(data){
					this.pendding = false
					this.data = data
					if(this.callback){
						this.callback(this.data)
						this.callback = undefined
					}
				}
			}
		}
		this.result.pendding = true
	}

	notifyResult(data){
		if(this.result == undefined) return;
		this.result.notify(data)
	}

	getResult(){
		return this.result;
	}

	initRoom(data){
		console.log(`[room init] id = ${data.room_id} score = ${data.score}` )
		this.roomId = data.room_id
		this.score  = data.score
		this.roomState = RoomState.Preparing
	}

	onMemberChanged(data){
		console.log("[member changed]")
		this.ownerId = data.owner
		this.members = data.members
		this.totalScore = data.total
		if(g.ui.page && g.ui.page.onMemberChanged){
			g.ui.page.onMemberChanged()
		}
	}

	onNtfJumpStart(data){
		if(data.uid != this.curId) return
		if(data.uid == this.uid){
			//it's me do nothing
		}else{
			//it's other
			g.step.onSimJumpStart(data)
		}
	}

	onNtfJumpEnd(data){
        this.notifyResult(data)
	}

	onRoomChanged(data){

		if(data.type == Type.GameInit){
			return this.startGame(data)
		}else if(data.type == Type.RoundChange){
			return this.nextRound(data)
		}
	}

	startGame(data){
		if(this.roomState != RoomState.Preparing) return
		console.log(`[room start] dir=${data.dir} dis=${data.dis}`)
		this.dir = data.dir
		this.dis = data.dis
		this.curId = data.curr
		this.roomState = RoomState.Start
		this.isLocalRound = this.curId == this.uid
		if(g.ui.page && g.ui.page.startGame){
			g.ui.page.startGame()
		}
		g.step.reset(1)
		g.step.startgame()
	}

	nextRound(data){
		this.curId = data.curr
		this.isLocalRound = this.curId == this.uid
	}

	login(cb){
		var self = this
		if(this.uid != undefined && this.token != undefined){
			g.network.post('/step/init',{
				sid : self.sessionId
			},function(e){
				var data = e.data
				if(data.ec == 0){
					self.onLoginSuccess(data.data)
					if(cb)cb()
				}else{
					self.onLoginFailed(data.data)
				}
			})
		}else{
			this.reSessionId(function(succ){
				if(!succ){
					console.log("[user] session get failed")
					return
				}
				wx.setStorageSync("session_id",self.sessionId)
				g.network.post('/step/init',{
					sid : self.sessionId
				},function(e){
					var data = e.data
					if(data.ec == 0){
						self.onLoginSuccess(data.data)
						if(cb)cb()
					}else{
						self.onLoginFailed(data.data)
					}

				})
			})
		}
	}

	onLoginFailed(data){

	}

	onLoginSuccess(data){
		this.uid = data.uid
		this.token = data.token
		wx.setStorageSync("uid",data.uid)
		wx.setStorageSync("token",data.token)
		g.network.connect()
		console.log("[login succ] " + this.uid + " " + this.token)
	}


	ping(){
		g.network.post('/step/ping',{},function(e){

		})
	}

	reSessionId(cb){
		var self = this
		wx.login({
			success : function(data){
				console.log(data)
				console.log("[use login] " + data.code)
				self.sessionId = data.code
				if(cb) {cb(true)}
			},
			fail : function(){
				if(cb) {cb(false)}
			}
		})
	}
}
