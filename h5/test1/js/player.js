import * as t from 'libs/three.js'
import * as tw from 'libs/tween.js'
let ChargeSpeed = 5
var State = {
	None : "none",
	Idle : "idle",
    Prepare: "prepare",
	Charge : "charge",
    Jump : "jump",
	Landing : "land"
}
export default class Player{

	    constructor(){
			this.state = State.None
			this.net = false
	    }


		bind(root){
			this.oldy = root.position.y
			this.root = root
			this.body = this.root.children[0]
			this.state = State.Idle
			this.flyingTime = 0
			this.hitchecked = false
		}


	    prepare(){
			if(this.state != State.Idle) return
			this.hitchecked = false
	        this.downtime = Date.now()/1000
	        this.state = State.Prepare
			if(this.net)
				g.network.prepare()
	        console.log("[prepareing...]")
	        this.squeeze()
	    }


	    jump(){
			if(this.state != State.Prepare) return

	        var presstime = Date.now()/1000 - this.downtime

	        this.stopprepare()

			if(this.net)
				g.network.jumpend(presstime)

	        this.speed = {y : g.config.speedY * presstime, z : g.config.speedZ * presstime}

			this.speed.z = +this.speed.z.toFixed(2)

			this.speed.y = +Math.min(this.speed.y + g.config.baseY, 180).toFixed(2)

			var start = new t.Vector2(+this.root.position.x.toFixed(2), +this.root.position.z.toFixed(2))

			var dest  = new t.Vector2(+g.step.targetpos.x.toFixed(2), +g.step.targetpos.z.toFixed(2))

	        var dir   =  new t.Vector2().subVectors(dest, start )

	        this.axis = new t.Vector3(dir.x,0,dir.y).normalize()

			this.flyingTime = 0

			var downtime = this.speed.y / g.config.gravity * 2

			this.caldest =  new t.Vector2().addVectors(start , dir.normalize().multiplyScalar(downtime * this.speed.z) )

			var r1 = g.config.table_radius

			var r2 = g.config.floor_radius

			if(g.step.idx > 1){
				r1 = g.config.floor_radius
			}

			var len1 = (r1 + r2) * (r1 + r2)

			var len2 = new t.Vector2().subVectors(this.caldest, dest).lengthSq()

			var result = -1

			if(len2 < len1){
				result = 1
			}

			if(len2 < (r1 * r1)){
				result = 0
			}

			this.result = result

			this.desireTime = downtime

			console.log(`[jump] speed.z=${this.speed.z} speed.y=${this.speed.y} result=${result} desire=${this.desireTime}`)

	        var self = this
	        var tr = new tw.Tween({r : 0})
	        .to({r : 0 - 2 * Math.PI },0.32)
	        .onUpdate(function(){
	            if(g.step.dir == 1){
	                self.body.rotation.z = this.r
	            }else{
	                self.body.rotation.x = this.r
	            }
	        }).start()

			g.step.spawnnext()

			this.state = State.Jump
	    }


		triggerEnd(){
			console.log(`[end triggered] flyingTime=${this.flyingTime} `)
			if(this.result == 0){
				this.state = State.Landing
				this.root.position.set(this.caldest.x , g.step.targetpos.y , this.caldest.y )
				g.step.addfloor(this)
				g.step.onLocalJumpOver()
			}else if(this.result == 1){
				this.state = State.Landing
				this.root.position.set(this.caldest.x , g.step.targetpos.y , this.caldest.y )
				g.step.addfloor(this)
				g.step.onLocalJumpOver()
			}else{
				setTimeout( function(){ g.step.onLocalJumpOver() } , 1 )
			}
		}

	    move(delta){
			if(this.flyingTime + delta >= this.desireTime){
				this.triggerEnd()
				if(this.result != -1) return
			}
			this.p = this.p || 0
			this.n = this.n || 0
	        var s = new t.Vector3(0, 0, 0)
	        s.z = this.speed.z * delta
	        s.y = this.speed.y * delta - g.config.gravity / 2 * delta * delta - g.config.gravity * this.flyingTime * delta
			// console.log(`moving ${delta} ${s.y} ${this.p} ${this.n}` )
			this.flyingTime += delta
			if(s.y > 0)
				this.p += s.y
			else
				this.n += s.y
	        this.root.translateY(s.y)
	        this.root.translateOnAxis(this.axis, s.z)
			if(this.root.position.y < 1){
				this.state  =  State.None
				this.root.position.y = 1
			}
	    }

	    update(delta){
	        if(this.state == State.Jump){
	            this.move(delta)
	        }
	    }

	    stopprepare(){
	        for(var a of this.anis){
	            a.stop()
	        }
	        this.root.scale.set(1,1,1)
	        this.root.position.y = this.oldy
	    }

	    squeeze(){
	        var self = this
	        this.anis = []
	        var tws = new tw.Tween(this.root.scale)
	        .to(new t.Vector3(1.07,0.5,1.07), 2.5)
	        .onUpdate(function(){
	            self.root.scale.set(this.x,this.y,this.z)
	        }).start()
	        var twy = new tw.Tween({y:this.root.position.y})
	        .to({y: this.root.position.y - 2 * (1-0.5) * 0.5},2.5)
	        .onUpdate(function(){
	            self.root.position.y = this.y
	        }).start()
	        this.anis.push(tws,twy)
	    }
}
