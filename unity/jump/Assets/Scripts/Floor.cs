using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Floor : MonoBehaviour {

    public float lastScale;

    private void Awake(){
        lastScale = transform.localScale.y;
    }

    public float ScaleY(float deltaScaleY,float duration){
        return ScaleTo(transform.localScale.y - deltaScaleY,duration);
    }

    public float ScaleTo(float scale,float duration){
        lastScale = transform.localScale.y;
        transform.DOLocalMoveY(scale, duration, false).SetId("FloorMoveDown").SetEase(Ease.Linear);
        transform.DOScaleY(scale, duration).SetId("FloorScaleDown").SetEase(Ease.Linear);
        return scale;
    }

    public void Revert(float duration){
        transform.DOLocalMoveY(lastScale, duration, false).SetId("FloorMoveDown").SetEase(Ease.Linear);
        transform.DOScaleY(lastScale, duration).SetId("FloorScaleDown").SetEase(Ease.Linear);
    }
	
}
