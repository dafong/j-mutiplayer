import * as t from 'libs/three.js'
var half_lambert_shader = {
    uniforms : {
        lightcol : { type : '3f', value:[1,1,1] },
        lightdir : { type : '3f', value:[-1,1,1.5] },
        lightscale : { type : 'f' , value:1 },
        color : { type : '3f' , value:[1,0,0]},
    },
    vertex_shader :[
        'uniform vec3 lightcol;',
        'uniform vec3 lightdir;',
        'uniform float lightscale;',
        'uniform vec3 color;',
        'varying vec4 posw;',
        'varying vec3 f_color;',
        'varying vec3 f_lightcol;',
        'varying vec3 f_lightdir;',
        'varying vec3 f_normal;',
        'varying float f_lightscale;',
        'void main(){',
            'posw = modelMatrix * vec4(position,1.0);',
            'f_color = color;',
            'f_lightcol = lightcol;',
            'f_lightdir = lightdir;',
            'f_normal = normal;',
            'f_lightscale = lightscale;',
            'gl_Position = projectionMatrix * modelViewMatrix * vec4(position,1.0);',
        '}'
    ].join('\n'),
    fragment_shader :[
        'varying vec4 posw;',
        'varying vec3 f_color;',
        'varying vec3 f_lightcol;',
        'varying vec3 f_lightdir;',
        'varying vec3 f_normal;',
        'varying float f_lightscale;',
        'void main(){',
            'vec3 normal = normalize(f_normal);',
            'vec3 lightdir = normalize(f_lightdir);',
            'vec3 lightcol = f_lightcol * (dot(normal,lightdir)/2.0 + 0.5) * f_lightscale;',
            'gl_FragColor = vec4(lightcol * f_color,1);',
        '}'
    ].join('\n')
}

var planar_shadow_shader = {
    uniforms:{
        center : { type : '4f' , value:[0,0,0,0]},
        pnormal: { type : '4f' , value:[0,1,0,0]},
        col    : { type : '3f' , value:[0.6,0.6,0.6]},
        lightdir:{ type : '3f' , value:[-1,1,1.5]}
    },
    vertex_shader:[
        'uniform vec4 center;',
        'uniform vec4 pnormal;',
        'uniform vec3 col;',
        'uniform vec3 lightdir;',
        'varying vec3 shadowcol;',
        'void main(){',
            'vec4 wpos = modelMatrix * vec4(position,1.0);',
            'vec3 dir  = normalize(lightdir);',
            'shadowcol = col;',
            'float d = dot(center.xyz - wpos.xyz,pnormal.xyz) /  dot(dir,pnormal.xyz);',
            'wpos.xyz = d * dir + wpos.xyz;',
            'gl_Position = projectionMatrix * viewMatrix * wpos;',
        '}'
    ].join('\n'),
    fragment_shader: [
      'varying vec3 shadowcol;',
       "void main() {",
           "gl_FragColor = vec4( shadowcol.xyz, 1.0 );",
       "}"
   ].join("\n")
}

export default class Factory{

    static create(floorName){
        if(floorName == 'player'){
            return this.create_player()
        }
        if(floorName == 'table'){
            return this.create_table()
        }
        if(floorName == 'base'){
            return this.create_base()
        }
    }

    static create_table(){
        var o = new t.MeshLambertMaterial({
            color: 0x619066
        })

        var r = new t.Geometry

        var s = new t.ConeGeometry(1,g.config.floor_height,32)
        this.merge(r,s,0,{
            x:0,
            y:0,
            z:0
        })

        var c = new t.CylinderGeometry(g.config.table_radius,g.config.table_radius,0.2,32)
        this.merge(r,c,0,{
            x:0,
            y:0.9,
            z:0
        })

        var mats = [ this.getMat(half_lambert_shader) , this.getMat(planar_shadow_shader) ]
        return t.SceneUtils.createMultiMaterialObject(r,mats)
    }

    static merge(src,dest,mat_i,pos){
        var o = dest.faces.length
        for(var i = 0; i < o; i++){
            dest.faces[i].materialIndex = 0
        }
        var r = new t.Mesh(dest)
        r.position.set(pos.x,pos.y,pos.z)
        r.updateMatrix()
        src.merge(r.geometry,r.matrix,mat_i)
    }

    static create_player(){
        var p = new t.CylinderGeometry(
        	g.config.floor_radius,
        	g.config.floor_radius,
        	g.config.floor_height,
        	32
        )
        var mats = [ this.getMat(half_lambert_shader) , this.getMat(planar_shadow_shader) ]
        return t.SceneUtils.createMultiMaterialObject(p,mats)
    }

    static create_base(){
        var p = new t.CylinderGeometry(
        	g.config.floor_radius,
        	g.config.floor_radius,
        	g.config.floor_height,
        	32
        )
        var mats = [ this.getMat(half_lambert_shader) , this.getMat(planar_shadow_shader) ]
        return t.SceneUtils.createMultiMaterialObject(p,mats)
    }

    static test(){
        t.SceneUtils.createMultiMaterialObject
        this.getMat(half_lambert_shader,{
            lightdir :[1,2,1]
        })
    }
    static getMat(shader,param){
        var param = param || {}
        var args  = {}
        for(var u in shader.uniforms){
            args[u]  = {}
            var type = shader.uniforms[u]['type']
            args[u]['type'] = type
            var para_src = shader.uniforms[u]['value'];
            if(param[u] == undefined){
                if(para_src && ( para_src.isColor ||
					para_src.isMatrix3 || para_src.isMatrix4 ||
					para_src.isVector2 || para_src.isVector3 || para_src.isVector4 ||
					para_src.isTexture )){
                    args[u]['value'] = para_src.clone()
                }else if(Array.isArray(para_src)){
                    args[u]['value'] = para_src.slice()
                }else{
                    args[u]['value'] = para_src
                }
            }else{
                args[u]['value'] = param[u]
            }

        }
        var mat = new t.ShaderMaterial({
            uniforms: args,
            vertexShader: shader.vertex_shader,
            fragmentShader: shader.fragment_shader
        });
        return mat
    }




}
