
var State = {
	Close : "Close",
	Open : "Open",
	Opening: "Opening"
}

var Type = {
	Heartbeat : 101,
	Auth : 102
}

export default class Network{

	constructor(){
		this.state = State.Close
		this.cache = {}
		this.register_cmd()
	}

	register_cmd(){
		this.use(Type.Auth,this.onauth)
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
		console.log("[network succ] ")
		var self = this
		this.state = State.Open
		this.socket.onMessage(function(data){
			self.onmessage(data)
		})

		this.socket.onError(function(err){
			console.log("err:" + err)
			self.close()
			self.connect()
		})

		this.socket.onClose(function(err){
			console.log(" on closed ")
		})
		this.heartbeat()
		this.auth()
	}

	auth(){
	    this.send({t: Type.Auth,token : g.user.token})
	}

	onauth(data){
		if(data.ec == 0){
			g.ui.toast({
				title : "长连接 ID:" + data.sid,
				icon : 'success',
				duration : 2000
			})
			console.log("[auth succ] sid="+data.sid)
			this.sid = data.sid
		}else{
			console.log("[auth failed] "+ data.ec)
		}
	}

	onmessage(o){
		var data = JSON.parse(o.data)
		var func = this.cache["c_"+data.cmd]
		if(data.cmd == Type.Heartbeat){
			return
		}

		if(func)
			func(data)
		else
			console.log(data.cmd + " not have a handler")
	}

	onfail(){
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
				console.log('send success')
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
