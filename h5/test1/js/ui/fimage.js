import * as t from '../libs/three.js'

export default class FImage{
    constructor(options) {
        this.opts = options || {}
        this.canvas = document.createElement("canvas")
        this.context= this.canvas.getContext('2d')
        this.texture= new t.Texture(this.canvas)
        this.mat = new t.MeshBasicMaterial({
            map = this.texture,
            transparent : true
        })
    }

    show(isshow){

    }
}
