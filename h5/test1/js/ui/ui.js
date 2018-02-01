import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'

let Size = new t.Vector2(414,736)
let layerSize = 4
var dpr = window.devicePixelRatio > 2 ? 2 : window.devicePixelRatio
var wr = window.innerWidth/Size.x
var hr = window.innerHeight/Size.y
var cx=function(e){ return e * wr * dpr }
var cy=function(e){ return e * hr * dpr }
var cw=function(e){ return e * wr * dpr }
var ch=function(e){ return e * hr * dpr }

export default class UI{

    constructor(){
        this.layers = []

        for(var i = 0;i<layerSize;i++){
            var o = {}
            o.canvas = wx.createCanvas()
            o.ctx = o.canvas.getContext('2d')

            o.canvas.width = window.innerWidth * dpr
            o.canvas.height= window.innerHeight * dpr
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
            o.root.position.set(0,0,-10 + 0.01 * i)
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
        for(var l of ls){
            cam.camera.add(this.layers[l].root)
        }
    }

    drawText(txt,l,x,y,size,sty,font,align,bl){
        align = align || 'center'
        sty   = sty || '#000'
        bl    = bl || 'middle'
        font  = font || 'Helvetica'
        size  = size || 17
        size  = size * wr
        x = cx(x),y = cy(y)
        var ctx = this.layers[l].ctx
        ctx.fillStyle = sty
        ctx.textBaseline = bl
        ctx.textAlign = align
        ctx.font = size + 'px '+ font
        ctx.fillText(txt,x,y)
        var tex = this.layers[l].tex
        tex.needsUpdate =true
        var p = new Promise(function(resolve){
            resolve()
        })
        return p
    }

    drawImage(path,l,x,y){
        x=cx(x),y=cy(y)
        var img = wx.createImage()
        var ctx = this.layers[l].ctx
        var tex = this.layers[l].tex
        var self= this
        var p = new Promise(function(resolve,reject){
            img.onload = function(){
                ctx.drawImage(img,x,y,img.width * wr,img.height * hr)
                tex.needsUpdate =true
                resolve()
            }
            img.src = path
        })

        return p
    }
    clearRect(l,x,y,w,h){
        x=cx(x),y=cy(y),w=cw(w),h=ch(h)
        this.layers[l].ctx.clearRect(x,y,w,h)
    }

    fillStyle(l,sty){
        this.layers[l].ctx.fillStyle = sty
    }

    fillRect(l,x,y,w,h){
        x=cx(x),y=cy(y),w=cw(w),h=ch(h)
        this.layers[l].ctx.fillRect(x,y,w,h)
    }

    showMainPage(){
        this.clearRect(0,0,0)
        var self = this
        this.drawImage("images/bullet.png",0,0,0).then(function(){
            return self.drawImage("images/enemy.png",0,0,10)
        }).then(function(){
            return self.drawText("hello world",0,10,20)
        }).then(function(){
            return self.drawImage("images/hero.png",0,0,10)
        })

        this.showLayer([0,1])
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
