import * as THREE from 'libs/three.js'
import CameraController from 'cameracontroller.js'
import StepHight from 'stephigh.js'
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
		renderer = new THREE.WebGLRenderer({ context: ctx });
		renderer.setSize( window.innerWidth, window.innerHeight)
		var cam = CameraController.get()
		this.root = new StepHight()
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
