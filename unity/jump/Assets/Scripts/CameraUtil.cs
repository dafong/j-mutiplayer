using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraUtil : MonoBehaviour {



	private Transform curTarget;

	public void MoveCameraUp(){
		transform.DOMoveY(transform.position.y + 2.66f, 1f, false);
	}

	public void MoveCameraTo(Transform target){
		if (target == null)
			return;

		if (curTarget == null) {
			MoveCameraCenter (target);
			return;
		}

		Vector3 dir = target.transform.position - curTarget.transform.position;

		Vector3 pos = curTarget.transform.position + dir * 0.5f;

		transform.DOMove (new Vector3(pos.x - 17, 17, pos.z-17), 1f, false);
	}

	public void MoveCameraCenter(Transform target){
		transform.DOMove (new Vector3 (target.position.x - 17f, transform.position.y, target.position.z - 17f), 1f, false);
		curTarget = target;
	}
}
