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
	Start : "start"
}


let _centerZ = 0
let _centerX = 9
export default class Step{

	constructor(s){
		scene = s
		this.init()
		var cam = Camera.get()
		cam.camera.add(this.root)

	}

	init(){
		this.root = new t.Object3D
		this.root.name = "root"
		this.addground()
		this.world= new t.Object3D
		this.world.name = "world"
		scene.add(this.world)
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
	addfloor(distance){
		var o = new t.MeshLambertMaterial({
			color: 0x619066
		})
		var s = new t.BoxGeometry(2 , g.config.floor_height, 2 );
		var m = new t.Mesh(s, o)
		this.world.add(m)
		m.name="floor"
		if(this.dir == 1){
			m.position.set((_centerX + distance/2 * this.dir),_py(g.config.floor_height,0),0)
		}else{
			m.position.set(_centerX,_py(g.config.floor_height,0),distance/2 * this.dir)
		}
	    m.position.y = 1

		this.targetpos = m.position.clone()
		this.targetpos.y += g.config.floor_height
	}

	startgame(){
		this.state = State.Start
	}

	addtable(distance){
		var o = new t.MeshLambertMaterial({
			color: 0x619066
		})

		var r = new t.Geometry

		var s = new t.ConeGeometry(1,g.config.floor_height,32)
		this.merge(r,s,0,{
			x:0,
			y:0.5,
			z:0

		})

		var c = new t.CylinderGeometry(1.5,1.5,0.2,32)
		this.merge(r,c,0,{
			x:0,
			y:0.9,
			z:0
		})

		var m = new t.Mesh(r,[o])
		m.name = "table"
		if(this.dir == 1){
			m.position.set((_centerX + distance/2 * this.dir),_py(g.config.floor_height,0),0)
		}else{
			m.position.set(_centerX,_py(g.config.floor_height,0),distance/2 * this.dir)
		}
		m.position.y = 1

		this.targetpos = m.position.clone()
		this.targetpos.y += g.config.floor_height
		this.world.add(m)
	}

	merge(src,dest,mat_i,pos){
		var o = dest.faces.length
		for(var i = 0; i < o; i++){
			dest.faces[i].materialIndex = 0
		}
		var r = new t.Mesh(dest)
		r.position.set(pos.x,pos.y,pos.z)
		r.updateMatrix()
		src.merge(r.geometry,r.matrix,mat_i)
	}

	update(delta){
		this.player.update(delta)
	}

	addbase(distance){
		var o = new t.MeshLambertMaterial({
			color: 0x000000
		})
		var p = new t.CylinderGeometry(.5,.5, g.config.floor_height,32)
		this.base = new t.Mesh(p,o)
		this.base.name = "base"
		this.world.add(this.base)
		if(this.dir == 1){
			this.base.position.set((_centerX - distance/2 * this.dir),_py(2,0),0)
		}else{
			this.base.position.set(_centerX ,_py(g.config.floor_height,0),- distance/2 * this.dir)
		}
		this.baseheight = g.config.floor_height
	}

	addplayer(distance){
		this.player = new Player()
		if(this.dir == 1){
			this.player.root.position.set((_centerX - distance/2 * this.dir),_py(g.config.floor_height,this.baseheight),0)
		}else{
		    this.player.root.position.set(_centerX ,_py(g.config.floor_height,this.baseheight),- distance/2 * this.dir)
		}
		this.world.add(this.player.root)
		this.player.init()
	}

	addlight(){
	    var e = new t.AmbientLight(0xffffff, .8);
	    e.name = "ambient light"
	    this.world.add(e)
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
		for( var i = this.world.children.length - 1; i >= 0; i--) {
			this.world.remove(this.world.children[i]);
		}

		this.dir = (parseInt(Math.random() * 2) - 0.5) * 2
		var distance =  3.5 + Math.random() * 2.5
		//this.addfloor(distance)
		this.addtable(distance)
		this.addbase(distance)
		this.addplayer(distance)
		this.addlight()
		this.startgame()
		g.util.dump_3d(scene)
	}

	ontouchstart(t,x,y){
		if(State.Start != this.state){
			return
		}
		this.player.prepare()
    }

    ontouchend(t,x,y){
		if(State.Start != this.state){
			return
		}
		this.player.jump()
    }

    ontouchmove(t,x,y){

    }
}
