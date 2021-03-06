import * as t from '../libs/three.js'
import Camera from '../cameracontroller.js'

let Size = new t.Vector2(414,736)
let layerSize = 4
var dpr = window.devicePixelRatio > 2 ? 2 : window.devicePixelRatio
var wr = window.innerWidth/Size.x
var hr = window.innerHeight/Size.y
var _cx=function(e){ return e * wr * dpr }
var _cy=function(e){ return e * hr * dpr }
var _cw=function(e){ return e * wr * dpr }
var _ch=function(e){ return e * hr * dpr }
var _tx=function(e){ return e * 1/wr }
var _ty=function(e){ return e * 1/hr }
var _py=function(h,b){ return b + h/2 }
GameGlobal._tx = _tx
GameGlobal._ty = _ty
GameGlobal._py = _py
var l = {
    init : function(){
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
            o.root.name="[ui] layer" + i
            o.root.position.set(0,0,-5 + 0.1 * i)
            this.layers[i] = o
        }
    },
    hideall : function(){
        var cam = Camera.get()
        for(var o of this.layers){
            cam.camera.remove(o.root)
        }
    },

    clearall : function(){
        for(var o in this.layers){
            this.clearLayer(o)
        }
    },

    showLayer : function(ls){
        var cam = Camera.get()
        this.hideall()
        for(var l of ls){
            cam.camera.add(this.layers[l].root)
        }
    },

    hideLayer : function(i){
        cam.camera.remove(this.layers[i].root)
    },

    drawText : function(txt,l,x,y,size,sty,font,align,bl){
        x     = x || 0
        y     = y || 0
        align = align || 'center'
        sty   = sty || '#000'
        bl    = bl || 'middle'
        font  = font || 'Helvetica'
        size  = size || 17
        size  = size * wr * dpr
        x = _cx(x),y = _cy(y)
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
    },

    drawImage : function(path,l,x,y,px,py,w,h){
        x=_cx(x),y=_cy(y)
        if(px == undefined) px = 0.5
        if(py == undefined) py = 0.5
        w = _cx(w),h=_cy(h)
        var img = wx.createImage()
        var ctx = this.layers[l].ctx
        var tex = this.layers[l].tex
        var self= this
        var p = new Promise(function(resolve,reject){
            img.onload = function(){
                // console.log(img.src + " loaded")
                w = (w || img.width) * wr
                h = (h || img.height) * hr
                x = x - w * px
                y = y - h * py
                ctx.drawImage(img,x,y,w,h)
                tex.needsUpdate =true
                resolve()
            }
            img.onerror = function(){
                console.log('error when loading ' + img.src)
            }
            img.src = path
        })

        return p
    },

    clearRect : function(l,x,y,w,h){
        x=_cx(x),y=_cy(y),w=_cw(w),h=_ch(h)
        this.layers[l].ctx.clearRect(x,y,w,h)
    },

    clearLayer : function(l){
        this.clearRect(l, 0, 0, g.config.design.x, g.config.design.y)
        var tex = this.layers[l].tex
        tex.needsUpdate = true
    },

    fillStyle : function(l,sty){
        this.layers[l].ctx.fillStyle = sty
    },

    fillRect : function(l,x,y,w,h){
        x=_cx(x),y=_cy(y),w=_cw(w),h=_ch(h)
        this.layers[l].ctx.fillRect(x,y,w,h)
    },

    dialog : function(){
        var t = arguments.length > 1 ? arguments[0] : "提示"
        var c = arguments.length > 1 ? arguments[1] : arguments[0]
        wx.showModal({
            title : t,
            content : c,
            showCancel : false
        })
        // wx.showModal({
        //     title: "提示",
        //     content: "分数上传失败,请检查网络状态后重试(" + e + ")",
        //     confirmText: "重试",
        //     success: function(i) {
        //         i.confirm ? t.reUploadScore() : i.cancel ? t.reUpLoadShowOverPage && t.showOverPage(!1) : t.showReUpLoadScoreModel(e)
        //     },
        //     fail: function() {
        //         t.reUpLoadShowOverPage && t.showOverPage(!1)
        //     }
        // })
    },
    echo : function(a,b){
        console.log(this.str + a + b )
    }
}

export default l
