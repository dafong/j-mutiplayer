import * as t from 'libs/three.js'
import * as tw from 'libs/tween.js'
import Network from 'network.js'
import User from 'user.js'
import CameraController from 'cameracontroller.js'
import StateManager from 'statemanager.js'
import Step from 'step.js'
import UI from 'ui/ui.js'

let ctx = canvas.getContext('webgl')
let renderer
let scene
let camera
let mesh

export default class App{
	constructor() {
		this.oldTime = undefined
		this.init()
	}

	setupRender(){
		var info = wx.getSystemInfoSync()
		log('---------------------------------------')
		log('[devicePixelRatio] ' + g.config.devicePixelRatio)
		log('[model] ' + g.config.model)
		log('[platform] ' + g.config.platform)
		log('[system] ' + g.config.system)
		log('[resolution] ' + g.config.width + ' x ' + g.config.height)
		log('---------------------------------------')

		renderer = new t.WebGLRenderer({
			antialias: true,
			canvas: canvas,
			preserveDrawingBuffer: true
		});

		renderer.setSize( window.innerWidth, window.innerHeight)
		var m = info.model
		var p = info.platform
		var s = info.system
		if(p == "ios"){
			//iphone4 5 6
			if(m.indexOf("iPhone 5") >= 0 || m.indexOf("iPhone 6") ){
				renderer.setPixelRatio(2)
			}else{
				renderer.setPixelRatio(
					window.devicePixelRatio ?
					Math.min(window.devicePixelRatio, 2) : 1 )
			}
		}else{
			renderer.setPixelRatio( window.devicePixelRatio ? window.devicePixelRatio : 1)
		}
		wx.triggerGC()
		wx.setPreferredFramesPerSecond(30)

	}


	setupEvent(){
		this.evthandler = undefined
		var self = this
		var handler = function(func,log,t){
			var x = ~~_tx(t.changedTouches[0].clientX)
			var y = ~~_ty(t.changedTouches[0].clientY)

			if(func != "ontouchstart" && self.evthandler){
				self.evthandler[func](t,x,y)
				return
			}
			var isd = false
			var hdl
			if(g.ui){
				hdl  = g.ui
				isd = hdl[func](t,x,y) || false
			}

			if(!isd) {
				hdl = g.step
				hdl[func](t,x,y)
				// console.log("[touch] "+ log + " " + x +" " + y)
			}

			if(func == "ontouchstart"){
				self.evthandler = hdl
			}

		}

		canvas.addEventListener("touchstart",function(t) {
			handler("ontouchstart","start",t)
		})
		canvas.addEventListener("touchend",function(t) {
			handler("ontouchend","end",t)
			this.evthandler = undefined
		})
		canvas.addEventListener("touchmove",function(t) {
			handler("ontouchmove","end",t)
		})
	}

	init(){
		wx.onShow(this.onGameResume.bind(this))
		this.setupRender()
		this.setupEvent()

		this.scene = new t.Scene();
		this.scene.name="scene"
		var cam = CameraController.get()
		this.scene.add(cam.camera)

		g.user    = new User
		g.network = new Network
		g.step    = new Step(this.scene)
		g.ui      = new UI
		g.state   = new StateManager
		this.oldTime = Date.now()

		g.ui.showMainPage()
		g.step.reset()
		this.loop()
		g.state.go('idle')
	}

	onGameResume(param){
		var self = this
		var query = param.query
		if(query.roomId != undefined){
			g.state.msg('resume.roomId',{
				roomId : query.roomId
			})
		}
		g.user.login(function(){
			g.network.connect()
			g.state.msg('connecting')
		})
	}

	update(delta){
		var cam = CameraController.get()
		g.ui.update(delta)
		g.step.update(delta)
		tw.update()
	}

	render() {
		var cam = CameraController.get()
		renderer.render(this.scene, cam.camera)
	}

	loop(time) {
		var now   = Date.now()
		var delta = (now  - this.oldTime)/1000
		this.update(delta)
		this.render()
		window.requestAnimationFrame(
		   this.loop.bind(this)
		)
		this.oldTime = now
	}

}
