import * as t from 'libs/three.js'
import Camera from 'cameracontroller.js'
import Player from 'player.js'
import * as tw from 'libs/tween.js'
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
	Start : 0
}

var Mode = {
	Offline : 0,
	Online : 1
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

		mode(m){
			this.mode = m
			console.log(`set mode ${this.mode}`)
		}

		init(){
			this.idx = 0
			this.root = new t.Object3D
			this.root.name = "root"
			this.mode = Mode.Offline

			this.addground()
			this.world= new t.Object3D
			this.world.name = "world"
			scene.add(this.world)
		}

		dump_world(){
			g.util.dump_3d(this.world)
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

		startgame(){
			if(this.mode == Mode.Online){
				this.state = State.Start
			}
		}

		reset(mode){
			if(mode == undefined) mode = Mode.Offline
			this.mode = mode
			for( var i = this.world.children.length - 1; i >= 0; i--) {
				this.world.remove(this.world.children[i]);
			}
			this.idx = 0
			this.dir = g.user.dir
			var distance =  g.user.dis

			if(this.mode == Mode.Offline){
				this.dir = (parseInt(Math.random() * 2) - 0.5) * 2
				distance = 3.5 + Math.random() * 2.5
			}

			this.addtable(distance)
			this.addbase(distance)
			this.spawnnext()
			this.spawnplayer()
			this.bindplayer()
			this.addlight()
			Camera.get().reset()
			// g.util.dump_3d(scene)
		}

		addtable(distance){
			var o = new t.MeshLambertMaterial({
				color: 0x619066
			})

			var r = new t.Geometry

			var s = new t.ConeGeometry(1,g.config.floor_height,32)
			this.merge(r,s,0,{
				x:0,
				y:0,
				z:0
			})

			var c = new t.CylinderGeometry(g.config.table_radius,g.config.table_radius,0.2,32)
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
			this.targetbase = m.position.y + g.config.floor_height/2
			this.targetradius=1.5
			this.world.add(m)
		}


		addbase(distance){
			var o = new t.MeshLambertMaterial({
				color: 0xff0000
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
			var pos = this.base.position.clone()
			pos.y = pos.y + g.config.floor_height
			this.spawnpos = pos
		}

		spawnplayer(){
			this.player = new Player()
			if(this.mode == Mode.Online)
				this.player.net = true
		}

		bindplayer(){
			this.player.bind(this.next)
		}

		detach(node){
			this.base.remove(node)
		}

		spawnnext(){
			this.idx++
			var root = new t.Object3D
			root.name="player-"+this.idx
			var o = new t.MeshLambertMaterial({
				color : this.idx * 400
			})
			var p = new t.CylinderGeometry(
				g.config.floor_radius,
				g.config.floor_radius,
				g.config.floor_height,
				32
			)
			var body = new t.Mesh(p,o)
			body.name="body"
			root.add(body)


			this.world.add(root)

			var tscale = this.base.position.y + 1
			root.position.set(this.base.position.x,this.base.position.y*2+1,this.base.position.z)
			this.next = root

			// animation
			var self = this
			var tws = new tw.Tween({ s : this.base.scale.y })
			.to({ s : this.idx },0.5)
			.easing(tw.Easing.Back.Out)
			.onUpdate(function(){
				self.base.scale.y = this.s
			}).onComplete(function(){

			})

			var twy = new tw.Tween({ y : this.base.position.y })
			.to({ y : this.idx },0.5)
			.easing(tw.Easing.Back.Out)
			.onUpdate(function(){
				self.base.position.y = this.y
			}).onComplete(function(){

			})

			var twn = new tw.Tween({ y :  root.position.y })
			.to({ y : this.idx * 2 + 1},0.5)
			.easing(tw.Easing.Back.Out)
			.onUpdate(function(){
				root.position.y = this.y
			}).onComplete(function(){

			})
			tws.start()
			twy.start()
			twn.start()

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
			if(this.player)
				this.player.update(delta)
		}

		addfloor(floor){
			this.targetpos = floor.root.position.clone()
			this.targetpos.y+=g.config.floor_height
			this.targetbase = this.targetpos.y - g.config.floor_height/2
			this.targetradius = g.config.floor_radius
			this.bindplayer()
			if(self.idx != 1){
				Camera.get().moveup()
			}
		}

		addlight(){
		    var e = new t.AmbientLight(0xffffff, .8);
		    e.name = "ambient light"
		    this.world.add(e)
		}

		changecolor(){
			var n = this.cur + 1 >=7 ? 0 : this.cur + 1
			this.cur = n
		}

		onSimJumpStart(){
			this.player.simprepare()
		}

		onSimJumpEnd(data){
			this.player.simjump(data)
		}

		onLocalJumpOver(result){
			if(this.mode == Mode.Offline){
				if(result == 0){
					g.step.addfloor(this.player)
				}
			}else{
				var result = g.user.getResult()
				result.oncomplete(this.onServerJumpEnd.bind(this))
			}
		}



		onServerJumpEnd(data){
			// the server will return the position calculated
			if(data.result == 0){
				console.log("[step] [round over] seq=" + data.tseq)
				this.player.root.position.set(data.destpos.x , (data.tseq - 0.5) * g.config.floor_height, data.destpos.y )
				this.addfloor(this.player)
				if(g.ui.page && g.ui.page.nextRound){
					g.ui.page.nextRound()
				}
			}else{
				console.log("[step] [round over] seq=" + data.tseq)
				this.player.root.position.set(data.destpos.x , (data.tseq - 0.5) * g.config.floor_height, data.destpos.y )
				this.addfloor(this.player)
				//failed
				console.log("[step] [game over]")
			}
			g.state.msg("local.roundover")
		}

		ontouchstart(t,x,y){

			if(State.Start != this.state){
				return
			}

			if(!g.user.isLocalRound){
				return
			}

			this.player.prepare()
	    }

	    ontouchend(t,x,y){
			if(State.Start != this.state){
				return
			}
			if(!g.user.isLocalRound){
				return
			}
			this.player.jump()
	    }

	    ontouchmove(t,x,y){

	    }
}
