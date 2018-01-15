import * as THREE from 'libs/three.js'
import CameraController from 'cameracontroller.js'
// import * from 'OBJLoader2.js'

export default class StepHigh{

	init(){
		console.log('game step hight init...')
		this.scene = new THREE.Scene();
		this.showMenu()
	}

	showMenu(){
		var geometry = new THREE.BoxBufferGeometry( 60, 60, 60 );
		var material = new THREE.MeshLambertMaterial( { color: new THREE.Color( 0xff0000 ) } );
		var mesh = new THREE.Mesh( geometry, material );

	var directionalLight = new THREE.DirectionalLight( 0xffffff, 10 );
					directionalLight.position.set( 1, 1, 0 ).normalize();
					this.scene.add( directionalLight );
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
