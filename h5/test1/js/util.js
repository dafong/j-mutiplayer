function _dump_3d(o3d,l,s){
    var lv = l || 0
    var str= s || ''
    var tab = '-'.repeat(lv)
    str = (tab + ' '+ _to_string(o3d)+ '\n')
    for(var c of o3d.children){
        str += _dump_3d(c,lv+2)
    }
    return str
}

function _to_string(o3d){

    return o3d.name + '  [x=' +o3d.position.x.toFixed(1)+',y='+o3d.position.y.toFixed(1)+',z='+o3d.position.z.toFixed(1) +']'
}


function _get_world_canvas(s,e){
    var c = wx.createCanvas()
    var ctx = c.getContext('2d')
    c.width = 64,c.height = 64
    var g = ctx.createLinearGradient(0, 0, 0, c.height);
    g.addColorStop(0,s)
    g.addColorStop(1,e)
    ctx.fillStyle = g
    ctx.fillRect(0,0,c.width,c.height)
    return c
}

export default {
    dump_table : function(){

    },


    get_world_canvas : _get_world_canvas,

    dump_3d : function(o){
        console.log(_dump_3d(o))
    }
}
