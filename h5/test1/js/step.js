import * as t from 'libs/three.js'
import Camera from 'cameracontroller.js'
import Player from 'player.js'
var scene
var cols = [
	["rgba(215, 219, 230, 1)", "rgba(188, 190, 199, 1)"],
	["rgba(255, 231, 220, 1)", "rgba(255, 196, 204, 1)"],
	["rgba(255, 224, 163, 1)", "rgba(255, 202, 126, 1)"],
	["rgba(255, 248, 185, 1)", "rgba(255, 245, 139, 1)"],
	["rgba(218, 244, 255, 1)", "rgba(207, 233, 210, 1)"],
	["rgba(219, 235, 255, 1)", "rgba(185, 213, 235, 1)"],
	["rgba(216, 218, 255, 1)", "rgba(165, 176, 232, 1)"],
	["rgba(207, 207, 207, 1)", "rgba(199, 196, 201, 1)"]
]
var State = {
	Start : 0,
	Charge : 1,
}
export default class Step{

	constructor(s){
		scene = s
		this.init()
		var cam = Camera.get()
		cam.camera.add(this.root)
		g.util.dump_3d(scene)
	}

	init(){
		this.root = new t.Object3D
		this.root.name = "world"
		this.addground()
		this.addfloor()
		this.addlight()
		this.addplayer()
	}

	addground(){
		var size = g.config.frustumsize * 2
		var p = new t.PlaneGeometry(g.config.ratio * size ,size)
		this.mats = []
	    for(var i = 0;i < 7; i++){
			var l = new t.Texture(g.util.get_world_canvas(cols[i][0],cols[i][1]))
			l.needsUpdate = true
			var m = new t.MeshBasicMaterial({
				map :l,
				opacity : 1,
				transparent : true
			})
			this.mats.push(m)
			var m = new t.Mesh(p,m)
			m.position.z = .1 * -(i + 1)
			m.name = 'bg_'+i
			this.root.add(m)
		}
		this.root.position.z = -84
		this.cur = 0
		for(var i=1;i<7;i++){
			this.root.children[i].visible = false
		}
	}
	// radius: 5,
	// width: 10,
	// minRadiusScale: .8,
	// maxRadiusScale: 1,
	// height: 5.5,
	// radiusSegments: [4, 50],
	// floatHeight: 0,
	// minDistance: 1,
	// maxDistance: 17,
	// minScale: r.minScale,
	// reduction: r.reduction,
	// moveDownVelocity: .07,
	// fullHeight: 5.5 / 21 * 40
	addfloor(){
		var o = new t.MeshLambertMaterial({
			color: 0x619066
		})
		var s = new t.BoxGeometry(2 , 1, 2 );
		var m = new t.Mesh(s, o)
		scene.add(m)
		m.name="floor"
		m.position.set(12,_py(1,0),0)
	}

	stargame(){
		this.state = State.Start
	}
	addtable(){

	}

	addplayer(){
		this.player = new Player()
		scene.add(this.player.root)
		this.player.root.position.set(8,_py(2,0),0)
	}

	 addlight(){
	    var e = new t.AmbientLight(0xffffff, .8);
	    e.name = "ambient light"
	    scene.add(e)
	}

	changecolor(){
		var n = this.cur + 1 >=7 ? 0 : this.cur + 1
		// o.customAnimation.to(this.materials[this.current], 5, {
		// 	opacity: 0,
		// 	onComplete: function() {
		// 		e.obj.children[i].visible = !1
		// 	}
		// }), this.obj.children[t].visible = !0, o.customAnimation.to(this.materials[t], 4, {
		// 	opacity: 1
		// })
		this.cur = n
	}

	reset(){
		// g.util.dump_3d(this.scene)
	}

	ontouchstart(t,x,y){
		if(State.Start == this.state){
			this.state = State.Charge
		}
		console.log("game start")
    }

    ontouchend(t,x,y){
		
    }

    ontouchmove(t,x,y){

    }
}
