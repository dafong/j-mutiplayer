

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
		this.curId    = -1
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
		log('room',`[room] [create success] 房间号=${data.room_id}`)
		this.roomId = data.room_id
		this.score  = data.score
		this.roomState = RoomState.Preparing
	}

	onMemberChanged(data){
		log('room',"[room] [member changed]")
		this.ownerId = data.owner
		this.members = data.members
		this.totalScore = data.total

	}

	nextRound(data){
		log('room',`[room] [next round] player_idx=${data.curr}`)
		this.curr = data.curr
		var id = this.members[this.curr-1].id
		this.isLocalRound = id == this.uid
	}


	startGame(data){
		if(this.roomState != RoomState.Preparing) return
	    log('room',`[room] [start] dir=${data.dir} dis=${data.dis}`)
		this.dir  = data.dir
		this.dis  = data.dis
		this.curr = data.curr
		this.roomState = RoomState.Start
		var id = this.members[this.curr-1].id
		this.isLocalRound = id == this.uid
		if(g.ui.page && g.ui.page.startGame){
			g.ui.page.startGame()
		}
		g.step.reset(1)
		g.step.startgame()
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
		this.uid   = data.uid
		this.token = data.token
		wx.setStorageSync("uid",  data.uid)
		wx.setStorageSync("token",data.token)
		console.log("[login] 用户ID=" + this.uid + " " + this.token)
	}


	ping(){
		g.network.post('/step/ping',{},function(e){

		})
	}

	reSessionId(cb){
		var self = this
		wx.login({
			success : function(data){
				console.log("[user] [login] " + data.code)
				self.sessionId = data.code
				if(cb) {cb(true)}
			},
			fail : function(){
				if(cb) {cb(false)}
			}
		})
	}
}
