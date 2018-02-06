import * as t from 'libs/three.js'

import CameraController from 'cameracontroller.js'
// import StepHigh from 'stephigh.js'
import Step from 'step.js'
import UI from 'ui/ui.js'
import Text from 'ui/text.js'

let ctx = canvas.getContext('webgl')
let renderer
let scene
let camera
let mesh

export default class App{
	constructor() {
		this.init()
	}

	setupRender(){
		var info = wx.getSystemInfoSync()
		console.log('---------------------------------------')
		console.log('[devicePixelRatio] ' + g.config.devicePixelRatio)
		console.log('[model] ' + g.config.model)
		console.log('[platform] ' + g.config.platform)
		console.log('[system] ' + g.config.system)
		console.log('[resolution] ' + g.config.width + ' x ' + g.config.height)
		console.log('---------------------------------------')

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
	}

	setupEvent(){
			var handler = function(func,log,t){
				var x = ~~_tx(t.changedTouches[0].clientX)
				var y = ~~_ty(t.changedTouches[0].clientY)
				console.log("[touch] "+ log + " " + x +" " + y)
				var isd = false
				if(g.ui){
					isd = g.ui[func](t,x,y)
				}
				if(isd) return;
				g.step[func](t,x,y)
			}

			canvas.addEventListener("touchstart",function(t) {
				handler("ontouchstart","start",t)
			})
			canvas.addEventListener("touchend",function(t) {
				handler("ontouchend","end",t)
			})
			canvas.addEventListener("touchmove",function(t) {
				handler("ontouchmove","end",t)
			})
	}

	init(){
		this.setupRender()
		this.setupEvent()
		this.scene = new t.Scene();
		this.scene.name="scene"
		var cam = CameraController.get()
		this.scene.add(cam.camera)
		g.ui = new UI
		g.ui.showMainPage()
		g.step = new Step(this.scene)
		g.step.reset()

		this.loop()
	}


	update(){

		var cam = CameraController.get()
	}

	render() {
		var cam = CameraController.get()
		renderer.render(this.scene, cam.camera)
	}

	loop() {
		this.update()
		this.render()
		window.requestAnimationFrame(
		   this.loop.bind(this)
		)
	}

}
