import * as t from '../libs/three.js'
import font from '../fonts/font.js'
export default class Text{
    constructor(te,options) {
        this.opts = options || {}

        this.mat = new t.MeshBasicMaterial({
            color:this.opts.fillStyle || 0x74d222 ,
            transparent: true
        })
        if(this.opts.opacity)
            this.mat.opacity = this.opts.opacity

        var mesh = new t.Mesh(new t.TextGeometry(te,{
            font: font,
            size: this.opts.size || 3,
            height:1
        }),this.mat);

        if(this.opts.textAlign == "center"){
            mesh.position.x=1 * t.length / -2
        }

        this.root = mesh
	}
}
