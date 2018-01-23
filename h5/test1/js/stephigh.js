import * as THREE from 'libs/three.js'
import CameraController from 'cameracontroller.js'
// import * from 'OBJLoader2.js'

export default class StepHigh{

	init(){
		console.log('game step hight init...')
		this.scene = new THREE.Scene();
		// this.showMenu()
		// this.shadow()
		this.halflambert()
	}

	halflambert(){
		var shader = {
			uniforms : {
				lightcol : { type : '3f', value:[1,1,1] },
				lightdir : { type : '3f', value:[0,0,0] },
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
		// var mat = new THREE.ShaderMaterial({
		//     uniforms: THREE.UniformsUtils.clone(shader.uniforms),
		//     vertexShader: shader.vertex_shader,
		//     fragmentShader: shader.fragment_shader
		// });
		// mat.uniforms.color.value = [1,1,0];
		// mat.uniforms.lightdir.value = [1,1,-1];
		// mat.uniforms.lightscale.value = 1.2;
		var mat = new THREE.MeshLambertMaterial( { color: new THREE.Color( 0xffffff ) } );
	    var geometry = new THREE.BoxGeometry( 6, 6, 6 );
		var mesh = new THREE.Mesh( geometry, mat );
		mesh.position.set(0,0,0)
		this.scene.add( mesh );
		var e = new THREE.AmbientLight(16777215, .8);
	this.scene.add(e)
		var cam = CameraController.get()
		cam.lookAt(mesh.position)
	}

	shadow(){
		var shader = {
			uniforms:{
				center : { type : '4f' , value:[0,0,0,0]},
				pnormal: { type : '4f' , value:[0,1,0,0]},
				col    : { type : '3f' , value:[0.6,0.6,0.6]},
				lightdir:{ type : '3f' , value:[-100,-100,-100]}
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

		var mat = new THREE.ShaderMaterial({
		    uniforms: THREE.UniformsUtils.clone(shader.uniforms),
		    vertexShader: shader.vertex_shader,
		    fragmentShader: shader.fragment_shader
		});
	    var geometry = new THREE.BoxBufferGeometry( 60, 60, 60 );
		var mesh = new THREE.Mesh( geometry, mat );
		mesh.position.set(0,0,0)
		this.scene.add( mesh );

		var cam = CameraController.get()

	}

	test(){
		var outline_shader = {
			uniforms:{
				"outline_width" : { type : "f" , value:1 },
			},
			vertex_shader:[
				"uniform float outline_width;",
				"void main(){",
					"vec4 mvPosition = modelViewMatrix * vec4( position, 1.0 );",
					"vec4 displacement = vec4( normalize( normalMatrix * normal ) * outline_width, 0.0 ) + mvPosition;",
					"gl_Position = projectionMatrix * modelViewMatrix * vec4( position, 1.0 );",
				"}"
			].join("\n"),
			fragment_shader: [
			   "void main() {",
				   "gl_FragColor = vec4( 1.0 ,0.0, 0.0, 1.0 );",
			   "}"
		   ].join("\n")

		}

		var outline_material = new THREE.ShaderMaterial({
		    uniforms: THREE.UniformsUtils.clone(outline_shader.uniforms),
		    vertexShader: outline_shader.vertex_shader,
		    fragmentShader: outline_shader.fragment_shader
		});
		outline_material.uniforms.outline_width.value=10.0
	    var geometry = new THREE.BoxBufferGeometry( 60, 60, 60 );
		var mesh = new THREE.Mesh( geometry, outline_material );
		mesh.position.set(0,0,0)
		this.scene.add( mesh );

		var cam = CameraController.get()
		cam.lookAt(mesh.position)
	}
	showMenu(){
		// var geometry = new THREE.BoxBufferGeometry( 60, 60, 60 );
		var geometry = new THREE.BoxGeometry(10,1.25,10);

		var material = new THREE.MeshLambertMaterial( { color: new THREE.Color( 0xff0000 ) } );
		var mesh = new THREE.Mesh( geometry, material );

   	 	var directionalLight = new THREE.DirectionalLight( 0xffffff, 10 );
		directionalLight.position.set( 1, 1, 0 ).normalize();
		this.scene.add( directionalLight );
		var light = new THREE.AmbientLight( 0x66666666 ); // soft white light
		this.scene.add( light );
		this.scene.add( mesh );
		mesh.position.set(0,0,0)
		var cam = CameraController.get()
		cam.lookAt(mesh.position)

		// var mat = new THREE.ShaderMaterial()
		// var loader = new OBJLoader2();
		// // function called on successful load
		// 	var callbackOnLoad = function ( event ) {
		// 		scene.add( event.detail.loaderRootNode );
		// 	};
        //
		// 	// load a resource from provided URL synchronously
		// 	loader.load( '../mesh/8.obj', callbackOnLoad, null, null, null, false );
	}


	update(){}
}
