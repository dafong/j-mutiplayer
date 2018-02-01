import * as t from 'libs/three.js'

let instance
let D2A = 2 * Math.PI / 360.0
let OFFSET =  new t.Vector3(0, 80, 0)

export default class CameraController{

	static get(){
		if(instance) return instance
		instance = new CameraController()
		instance.init()
		return instance
	}

	init(){

		var ratio = g.config.ratio
		//design size is 414 * 735 iphone 6
		 // window.innerHeight / window.innerWidth / 736 * 414 * 60
		// new o.OrthographicCamera(t * i / -2, t * i / 2, t / 2, t / -2, -10, 85),
		var size = g.config.frustumsize
		this.camera =  new t.OrthographicCamera( - size * ratio ,  size * ratio, size, -size, -10, 85 );
		this.camera.position.set(-17, 30, 26);
		this.camera.lookAt(new t.Vector3(13, 0, -4));
		this.camera.name = "camera"
		instance = this

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
