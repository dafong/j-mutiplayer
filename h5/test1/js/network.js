
var State = {
	Close : "Close",
	Open : "Open",
	Opening: "Opening"
}

var Type = {
	Heartbeat : 101,
	Auth : 102,
	CreateRoom : 103,
	JoinRoom : 104,
	Prepare : 105,
	JumpStart:106,
	JumpEnd:107,
	NtfMemberChanged : 1105, // contains member join or leave and owner changed
	NtfRoomStateChanged : 1106,// room state changed
}

export default class Network{

	constructor(){
		this.state = State.Close
		this.cache = {}
		this.register_cmd()
		this.isReconnected = false
	}

	register_cmd(){
		this.use(Type.Auth, this.onauth)
		this.use(Type.CreateRoom, this.onroomcreate)
		this.use(Type.JoinRoom, this.onroomjoin)
		this.use(Type.NtfMemberChanged,this.onmemberchanged)
		this.use(Type.NtfRoomStateChanged,this.onroomchanged)
		this.use(Type.JumpStart,this.onjumpstart)
		this.use(Type.JumpEnd,this.onjumpend)
	}

	use(cmd,func){
		this.cache["c_"+cmd] = func.bind(this)
	}

	connect(){

		if(this.state != State.Close) return;

		this.state = State.Opening
		var self = this
		var st = wx.connectSocket({
			url : g.config.socket,
			header : {},
			fail:function(){
				self.onfail()
			}
		})
		st.onOpen(function(){
			self.socket = st
			self.onsuccess()
		})

	}

	onsuccess(){
		if(this.isReconnected) {
			g.ui.hideLoading()
		}
		var self = this
		this.state = State.Open
		this.socket.onMessage(function(data){
			self.onmessage(data)
		})

		this.socket.onError(function(err){
			console.log("[socket] err:" + err)
			self.close()
			this.reconnect()
		})

		this.socket.onClose(function(err){
			console.log(" on closed ")
		})
		this.heartbeat()
		this.auth()
	}

	reconnect(){
		this.isReconnected = true
		self.connect()
		g.ui.loading({title : '重连中...'})
	}

	auth(){
	    this.send({t: Type.Auth,token : g.user.token})
	}

	createroom(){
		this.send({t: Type.CreateRoom})
	}

	prepare(){
		this.send({t: Type.Prepare})
	}

	join(rid){
		this.send({t: Type.JoinRoom , room_id : rid})
	}

	jumpstart(){
		this.send({t: Type.JumpStart })
	}

	jumpend(time){
		this.send({t: Type.JumpEnd,time : time })
		g.user.penddingResult()
	}


	onauth(data){
		g.state.msg("auth.complete",data)
		if(data.ec == 0){
			this.sid = data.sid
		}
	}

	onroomcreate(data){
		g.state.msg("room.create",data)
	}

	onroomjoin(data){
		g.state.msg("room.join",data)
	}

	onmemberchanged(data){
		g.state.msg("room.memberchanged",data)
	}

	onroomchanged(data){
		g.state.msg("room.changed",data)
	}

	onjumpstart(data){
		g.state.msg("room.jumpstart",data)
	}

	onjumpend(data){
		g.state.msg("room.jumpend",data)
	}

	onmessage(o){
		var data = JSON.parse(o.data)
		var func = this.cache["c_"+data.cmd]
		if(data.cmd == Type.Heartbeat){
			return
		}

		console.log(data)

		if(func)
			func(data)
		else
			console.log(data.cmd + " not have a handler")
	}

	onfail(){
		console.log("[network] [connect failed] ")
		var self = this
		setTimeout(function(){
			self.connect()
		},3000)
	}


	heartbeat(){
		this.send({ t: Type.Heartbeat })
		var self = this
		this.tid = setTimeout(function(){
			self.heartbeat()
		},5000)
	}

	clearheartbeat(){
		clearTimeout(this.tid)
	}


	send(data){
		this.socket.send({
			data : JSON.stringify(data),
			success : function(){
				// console.log('send success')
			},
			fail:function(){
				console.log('send failed')
			}
		})
	}

	close(){
		this.state = State.Close
		this.socket.close({
			code : 1000
		})
		this.clearheartbeat()
	}

	get(url,param,succ,failf){
		failf = failf || g.config.noop
		param = param || {}
		param.token = g.user.token
		wx.request({
			url: g.config.ajax + url,
			method: "GET",
			data: param,
			success: function(e){
				if(e.statusCode == 200){
					succ(e)
				}else{
					console.log(url + " request failed " + e.statusCode)
					failf()
				}
			},
			fail: function(e){
				console.log(url + " request failed ")
				failf()
			}
		})
	}

	post(url,param,succ,failf){
		failf = failf || g.config.noop
		param = param || {}
		param.token = g.user.token
		wx.request({
			url: g.config.ajax + url,
			method: "POST",
			data: param,
			success: function(e){
				if(e.statusCode == 200){
					succ(e)
				}else{
					console.log(url + " request failed " + e.statusCode)
					failf()
				}
			},
			fail: function(e){
				console.log(url + " request failed ")
				failf()
			}
		})
	}

}
