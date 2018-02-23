
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
		console.log("[network] [success] ")
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

	onmessage(data){
		console.log('recv msg')
		console.log(data)
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
		wx.request({
			url: g.config.ajax + url,
			method: "GET",
			data: param || {},
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
		wx.request({
			url: g.config.ajax + url,
			method: "POST",
			data: param || {},
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
