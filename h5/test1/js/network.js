export default class Network{

	connect(){
		var st = wx.connectSocket({
			url : 'ws://127.0.0.1:4000/ws',
			header : {},
			success : function(){
				console.log('success')
			},
			fail:function(){
				console.log('failed')
			}
		})
		this.socket = st
		var self = this
		this.socket.onMessage(function(data){
			self.onmessage(data)
		})
		this.socket.onError(function(err){
			console.log("err: " + err)
		})
	}

	onmessage(data){
		console.log('recv msg')
		console.log(data)
	}

	send(data){
		this.socket.send({
			data : data,
			success : function(){
				console.log('send success')
			},
			fail:function(){
				console.log('send failed')
			}
		})
	}

	close(){
		this.socket.close()
	}

	get(){

	}

	post(){

	}


}
