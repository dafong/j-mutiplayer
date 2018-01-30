import * as t from 'libs/three.js'

import CameraController from 'cameracontroller.js'
import StepHigh from 'stephigh.js'
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
		console.log('[window.devicePixelRatio] ' + window.devicePixelRatio)
		console.log('[model] ' + info.model)
		console.log('[platform] ' + info.platform)
		console.log('[system] ' + info.system)
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

	init(){

		this.setupRender()


		this.scene = new t.Scene();
		var cam = CameraController.get()

		var ui = new UI
		var t1 = new Text("你好12344",{
			size:.5
		})
		ui.root.add(t1.root)


		this.t1 = t1
		t1.root.position.x = -4.5
		cam.camera.add(ui.root)
		this.scene.add(cam.camera)

			
		this.loop()


	}

	update(){
		// if(this.root)
		// 	this.root.update()

		// this.t1.root.rotation.y += 0.01
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
