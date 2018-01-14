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
		var geometry = new THREE.BoxBufferGeometry( 10, 10, 10 );
		var material = new THREE.MeshBasicMaterial( { color: new THREE.Color( 0xff0000 ) } );
		var mesh = new THREE.Mesh( geometry, material );

		this.scene.add( mesh );
		mesh.position.set(0,0,0)
		var cam = CameraController.get()
		cam.camera.lookAt(this.scene.position)

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
