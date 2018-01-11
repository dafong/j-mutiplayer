using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

    public void PressUp(float scaleChange,float groundHeight,float duration){
        float destScale = transform.localScale.y - scaleChange;
        transform.DOLocalMoveY(groundHeight + destScale, duration, false).SetId("PlayerMoveDown").SetEase(Ease.Linear);
        transform.DOScale(new Vector3(0.6f, destScale, 0.6f), duration).SetId("PlayerScaleDown").SetEase(Ease.Linear);
    }

	// Use this for initialization
    public void JumpTo (Vector3 dest) {
        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f).SetId("PlayerScaleDown").SetEase(Ease.Linear);
        Vector3 dir = new Vector3(dest.x - transform.position.x, 0, dest.z - transform.position.z);
        Vector3 normal = dir.normalized;
        transform.forward = normal;
        transform.DOLocalRotate(new Vector3(180,0,0),0.5f,RotateMode.LocalAxisAdd);
        //Quaternion.Euler(new Vector3(180,0,0));
	}
	
}
