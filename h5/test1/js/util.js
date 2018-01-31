function _dump_3d(o3d,l,s){
    var lv = l || 0
    var str= s || ''
    var tab = '-'.repeat(lv)
    str = (tab + o3d.name + '\n')
    for(var c of o3d.children){
        str += _dump_3d(c,lv+2)
    }
    return str
}

export default {
    dump_table : function(){

    },

    dump_3d : function(o){
        console.log(_dump_3d(o))
    }
}
