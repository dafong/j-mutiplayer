import * as THREE from 'libs/three.js'

let instance


export default class CameraController{

	static get(){
		if(instance) return instance
		const winWidth = window.innerWidth
		const winHeight = window.innerHeight
		this.camera =  new THREE.OrthographicCamera( winWidth / - 2, winWidth / 2, winHeight / 2, winHeight / - 2, 1, 1000 );
		this.camera.position.set(0,30,0)
		instance = this
		console.log('camera init...size['+winWidth+'x'+winHeight+']')
		return instance
	}

	

}
