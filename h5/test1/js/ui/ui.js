import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'

let Size = new t.Vector2(320,568)
let layerSize = 4
export default class UI{

    constructor(){
        this.layers = []
        for(var i = 0;i<layerSize;i++){
            var o = {}
            o.canvas = wx.createCanvas()
            o.ctx = o.canvas.getContext('2d')
            o.canvas.width = window.innerWidth * g.config.devicePixelRatio
            o.canvas.height= window.innerHeight * g.config.devicePixelRatio
            o.tex = new t.Texture(o.canvas)
            o.mat = new t.MeshBasicMaterial({
                map : o.tex,
                transparent : true
            })
            o.mat.map.minFilter = t.LinearFilter
            var size = 2 * g.config.frustumsize
            o.geo = new t.PlaneGeometry(g.config.ratio * size, size)
            o.root= new t.Mesh(o.geo,o.mat)
            o.root.name="layer" + i
            o.root.position.set(0,0,10 - 0.01 * i)
            this.layers[i] = o
        }
    }
    key: "_cwh",
    value: function(e) {
        var t = e * u / 414;
        return d / u < 736 / 414 && (t = e * d / 736), t * c
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
        for(var l of ls){
            cam.camera.add(this.layers[l].root)
        }
    }

    drawText(text,l,x,y){
        var ctx = self.layers[l].ctx
        // ctx.clearRect(0,0,)
    }

    drawImage(path,l,x,y){
        var img = wx.createImage()
        var ctx = this.layers[l].ctx
        var tex = this.layers[l].tex
        img.onload = function(){
            console.log(img.width)
            ctx.drawImage(img,x,y,img.width,img.height)
            tex.needsUpdate =true
        }
        img.src = path
    }
    clearRect(l,x,y,w,h){
        this.layers[l].ctx.clearRect(x,y,w,h)
    }

    fillStyle(l,sty){
        this.layers[l].ctx.fillStyle = sty
    }

    fillRect(l,x,y,w,h){
        this.layers[l].ctx.fillRect(x,y,w,h)
    }

    showMainPage(){
        this.clearRect(0,0,0)

        this.drawImage("images/bullet.png",0,0,0)
        this.drawImage("images/bullet.png",0,50,50)

        this.showLayer([0])


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
