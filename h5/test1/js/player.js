import * as t from 'libs/three.js'
import * as tw from 'libs/tween.js'
let ChargeSpeed = 5
var State = {
	Idle : "idle",
    Prepare: "prepare",
	Charge : "charge",
    Jump : "jump",
	Land : "land"
}
export default class Player{

    constructor(){
        this.setup()
    }

    setup(){
        this.root = new t.Object3D
		this.root.name = "player"
        var o = new t.MeshLambertMaterial({
            color: 0x000000
        })
        var p = new t.CylinderGeometry(.5,.5, 2, 32)
        this.body = new t.Mesh(p,o)
        this.body.name = "body"
        this.root.add(this.body)
        this.flyingTime = 0
    }

    init(){
        this.state = State.Idle
    }

    prepare(){
        this.downtime = Date.now()/1000

        this.state = State.Prepare
        console.log("prepare")
        this.squeeze()
    }


    jump(){
        var presstime = Date.now()/1000 - this.downtime
        this.state = State.Jump
        this.stopprepare()
        this.speed = {y : g.config.speedY * presstime, z : g.config.speedZ * presstime}
        var dir = new t.Vector2(g.step.targetpos.x-this.root.position.x,g.step.targetpos.z-this.root.position.z)
        this.axis = new t.Vector3(dir.x,0,dir.y).normalize()
        console.log("[speed] "+ this.speed.z.toFixed(1)+" "+this.speed.y.toFixed(1))
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
    }

	stopjump(y){
		console.log("stoped")
		this.root.position.y = y+g.config.floor_height/2
		this.state = State.Land
	}

	checkhit(){
		var y = this.root.position.y - g.config.floor_height/2

		if(y < 0){
			this.stopjump(y)
		}

		// check if the player can land the table
		// has intersect and baseline is close to the table's top
		// if true stopit
		// and enter land state will check land is safe
		// if false
		// check if player's baseline is lower than 0
		// if true stop it

	}

    move(delta){
        var s = new t.Vector3(0, 0, 0)
        s.z = this.speed.z * delta
        s.y = this.speed.y * delta - g.config.gravity / 2 * delta * delta - g.config.gravity * this.flyingTime * delta
        this.flyingTime += delta
        // console.log(s.z+" "+s.y)
        this.root.translateY(s.y)
        this.root.translateOnAxis(this.axis, s.z)
		this.checkhit()
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
        this.root.position.y = _py(g.config.floor_height,g.step.baseheight)
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
