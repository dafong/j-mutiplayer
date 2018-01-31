import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'

let Size = new t.Vector2(640,1136)
let layerSize = 4
export default class UI{

    constructor(){
        this.layers = []
        for(var i = 0;i<layerSize;i++){
            var o = {}
            o.canvas = document.createElement("canvas") //wx.createCanvas()
            o.context= o.canvas.getContext('2d')
            o.canvas.width = window.innerWidth * g.config.devicePixelRatio
            o.canvas.height= window.innerHeight * g.config.devicePixelRatio
            o.tex = new t.Texture(o.canvas)
            o.mat = new t.MeshBasicMaterial({
                map : o.tex,
                // color : 0xff0000,
                transparent : true
            })
            // o.mat.map.minFilter = t.LinearFilter
            var size = 2 * g.config.frustumsize
            o.geo = new t.PlaneGeometry(g.config.ratio * size/2 , size/2)
            o.root= new t.Mesh(o.geo,o.mat)
            o.root.name="layer" + i
            o.root.position.set(0,0,10 - 0.01 * i)
            this.layers[i] = o
        }
    }

    hideall(){
        var cam = Camera.get()
        for(var o in this.layers){
            cam.camera.remove(o.root)
        }
    }

    showLayer(ls){
        var cam = Camera.get()
        this.hideall()
        for(var l in ls){
            cam.camera.add(this.layers[l].root)
        }
    }


    drawImage(path,l){
        var img = wx.createImage()
        var self = this
        img.onload = function(){
            console.log("image loaded")
            var ctx = self.layers[l].context
            ctx.fillStyle = 'blue';
            ctx.fillRect(10, 10, 100, 100);
            // self.layers[l].context.drawImage(img,10,-10,50,50)
        }
        img.src = path
    }

    showMainPage(){
        // this.drawImage('images/bullet.png',0)
        // this.showLayer([0])
        var canvas = document.createElement("canvas") //wx.createCanvas()
        var context= canvas.getContext('2d')
        canvas.width = window.innerWidth * g.config.devicePixelRatio
        canvas.height= window.innerHeight * g.config.devicePixelRatio
        var tex = new t.Texture( canvas)
        var mat = new t.MeshBasicMaterial({
            map : tex,
            // color : 0xff0000,
            transparent : true
        })
        mat.map.minFilter = t.LinearFilter
        var size = 2 * g.config.frustumsize
        var geo = new t.PlaneGeometry(g.config.ratio * size/2 , size/2)
        var root= new t.Mesh(geo,mat)
        root.name="layer" + 1
        root.position.set(0,0,10 - 0.01 * 1)
        var cam = Camera.get()
        cam.camera.add(root)



        var img = new Image
        var self = this
        img.onload = function(){
            context.drawImage(img,10,-10,50,50)
        }
        img.src = 'images/bullet.png'

        tex.needUpdate = true
    }

    showBonusPage(){

    }

    showRoomPage(){

    }

    showGamePage(){

    }

    showEndPage(){

    }

    showWarnDialog(){

    }
}
