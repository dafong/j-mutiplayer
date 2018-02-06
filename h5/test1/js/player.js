import * as t from 'libs/three.js'

export default class Player{

    constructor(){
        this.setup()
    }

    setup(){
        var o = new t.MeshLambertMaterial({
            color: 0x000000
        })
        var p = new t.CylinderGeometry(.5,.5,2,32)
        this.root = new t.Mesh(p,o)
        this.root.name = "player"
    }

    charge(){
        
    }
}
