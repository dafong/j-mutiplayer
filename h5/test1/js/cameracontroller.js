import * as THREE from 'libs/three.js'

let instance
let D2A = 2 * Math.PI / 360.0
let OFFSET =  new THREE.Vector3(0, 80, 0)
export default class CameraController{

	static get(){
		if(instance) return instance
		instance = new CameraController()
		instance.init()
		return instance
	}

	init(){
		const winWidth = window.innerWidth
		const winHeight = window.innerHeight
		var i = window.innerWidth/window.innerHeight;
		this.camera =  new THREE.OrthographicCamera( -20*i , 20*i , 20, -20, -10, 85 );
		 this.camera.position.set(-17, 30, 26);
		 this.camera.lookAt(new THREE.Vector3(13, 0, -4));
		instance = this
		console.log('camera init...size['+winWidth+'x'+winHeight+']')
	}

	lookAt(p){
		this.camera.lookAt(p)
		// var euler = new THREE.Euler( -D2A * 30 ,D2A * 30, 0,"YXZ");
		// var quat  = new THREE.Quaternion()
		// quat.setFromEuler(euler)
		// var dir = new THREE.Vector3(0,0,-1).applyQuaternion(quat).negate().normalize()
		// var pos = dir.multiplyScalar(10).add(OFFSET).add(p)
		// this.camera.position.set(pos.x,pos.y,pos.z)
		// this.camera.rotation.copy(euler)
		// console.log(euler)
	}


}
