var RoomState = {
	None : 0,
	Preparing : 1,
	Ready : 2,
	Start : 3
}

export default class User{
	constructor(){
		this.sessionId = undefined
		this.token = wx.getStorageSync("token")
		this.uid   = wx.getStorageSync("uid")

		this.exitRoom()
	}

	exitRoom(){
		this.roomId = -1
		this.members= undefined
		this.roomState= RoomState.None
		this.score = undefined
		this.isOwner= false
	}


	initRoom(data){
		console.log("[room init]")
		this.roomId = data.roomt_id
		this.score  = data.score
		this.roomState = RoomState.Preparing
	}

	onMemberChanged(data){
		console.log("[member changed]")
		console.log(data)
		this.isOwner = data.owner == this.uid
		this.members = data.members
		if(g.ui.page && g.ui.page.onMemberChanged){
			g.ui.page.onMemberChanged()
		}
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
