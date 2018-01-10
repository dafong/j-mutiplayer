using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum State{
	Menu = 0,
	Gaming,
	Over,
	ChargeUp,
	Jump
}

public class GameUtil : MonoBehaviour {

	public float scaleSpeed;

	public float minScale;

	public float jumpPowerSpeed;

	public CameraUtil cameraUtil;

	public Transform player;

	public Transform curFloor;

	public State state;

	private bool isStart;

	public float jumpPower;
	void Start(){
		cameraUtil.MoveCameraCenter (curFloor);
		state = State.Gaming;
	}


	void Update(){
		switch (state) {
		case State.ChargeUp:
			jumpPower += jumpPowerSpeed * Time.deltaTime;
			break;
		}
	}

	public void OnClick(){
		
	}

	public void OnPressDown(){
		state = State.ChargeUp;
		jumpPower = 0;

		float duration = (1 - minScale) / scaleSpeed;
		float translate = (2 * (1 - minScale)) / 2;

		curFloor.transform.DOLocalMoveY (0.5f, duration, false).SetId ("FloorMoveDown").SetEase (Ease.Linear);
		curFloor.transform.DOScaleY ( 0.5f, duration).SetId ("FloorScaleDown").SetEase (Ease.Linear);

//		player.transform.DOLocalMoveY (-2 * translate, duration, false).SetId ("PlayerMoveDown").SetEase (Ease.Linear);
//		player.transform.DOScale (new Vector3 (1.5f, 0.5f * minScale, 1.5f), duration).SetId ("PlayerScaleDown").SetEase (Ease.Linear);
	}

	public void OnPressUp(){
		curFloor.DOKill ();
		player.DOKill ();
		state = State.Jump;
	}


}
