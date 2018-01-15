import * as THREE from 'libs/three.js'
import CameraController from 'cameracontroller.js'
// import * from 'OBJLoader2.js'

export default class StepHigh{

	init(){
		console.log('game step hight init...')
		this.scene = new THREE.Scene();
		// this.showMenu()
		this.shadow()
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
		cam.lookAt(mesh.position)
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
		var geometry = new THREE.BoxBufferGeometry( 60, 60, 60 );
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

		var mat = new THREE.ShaderMaterial()
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
