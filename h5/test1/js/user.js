export default class User{
	constructor(){
		this.sessionId = undefined
		this.token = wx.getStorageSync("token")
		this.uid   = wx.getStorageSync("uid")
	}

	init(){

	}


	login(cb){
		var self = this
		this.reSessionId(function(succ){
			if(!succ){
				console.log("[user] session get failed")
				return
			}
			wx.getUserInfo({
				success : function(data){
					console.log(data)
				}
			})
			g.network.post('/step/init',{
				sid : self.sessionId
			},function(e){

			})
		})
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
