import * as t from 'libs/three.js'
import * as tw from 'libs/tween.js'
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
		this.camera.name = "camera"
		instance = this
		this.reset()
	}

	reset(){
		this.camera.position.set(-17, 30, 26);
		this.camera.lookAt(new t.Vector3(13, 0, -4));
	}

	moveup(){
		var self = this
		var twn = new tw.Tween({ y :  this.camera.position.y })
		.to({ y : this.camera.position.y  + g.config.floor_height },0.5)
		.easing(tw.Easing.Back.Out)
		.onUpdate(function(){
			self.camera.position.y = this.y
		}).onComplete(function(){

		})
		twn.start()
	}

	lookAt(p){
		this.camera.lookAt(p)
	}


}
