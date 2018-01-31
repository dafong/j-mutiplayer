import * as t from '../libs/three.js'

export default class FImage{
    constructor(options) {
        this.opts = options || {}
        this.canvas = document.createElement("canvas")
        this.canvas.width = 512
        this.canvas.height= 512
        this.context= this.canvas.getContext('2d')
        this.texture= new t.Texture(this.canvas)
        this.mat = new t.MeshBasicMaterial({
            map : this.texture,
            transparent : true
        })
        this.geometry = new t.PlaneGeometry()
        this.mesh = new t.Mesh(this.geometry,this.mat)
        this.mat.map.minFilter = t.LinearFilter
        this.root = this.mesh
    }

    show(isshow){
        this.root.visible = isshow
    }
}

// s = (n(i(4)), n(i(10)), window.devicePixelRatio > 2 ? 2 : window.devicePixelRatio),
// h = window.innerHeight < window.innerWidth ? window.innerHeight : window.innerWidth,
// l = window.innerHeight > window.innerWidth ? window.innerHeight : window.innerWidth,
// c = l * s,
// u = h * s,
// d = o.FRUSTUMSIZE,
// f = u / c * d,
//
// this.canvas[p[e]].width = u,
// this.canvas[p[e]].height = this.cheight * s,
//
// this.geometry[p[e]] = new a.PlaneGeometry(f, this.cheight / l * d),
