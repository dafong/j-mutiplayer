import * as t from 'libs/three.js'
import CameraController from 'cameracontroller.js'
import StepHigh from 'stephigh.js'
let ctx = canvas.getContext('webgl')
let renderer
let scene
let camera
let mesh

export default class App{
	constructor() {
		this.init()
	}

	init(){
		console.log('app init...')
		console.log('window.devicePixelRatio= ' + window.devicePixelRatio)
		renderer = new t.WebGLRenderer({
			antialias: true,
			canvas: canvas,
			preserveDrawingBuffer: true
		});

		renderer.setSize( window.innerWidth, window.innerHeight)

		var cam = CameraController.get()
		this.root = new StepHigh()
		this.root.init()
		this.loop()
	}

	update(){
		if(this.root)
			this.root.update()
	}

	render() {
		var cam = CameraController.get()
		renderer.render(this.root.scene, cam.camera)
	}

	loop() {
	  this.update()
	  this.render()
	  window.requestAnimationFrame(
		this.loop.bind(this)
	  )
	}

}
