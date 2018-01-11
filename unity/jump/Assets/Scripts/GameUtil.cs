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

    public float maxScaleDuration;

	public float jumpPowerSpeed;

	public CameraUtil cameraUtil;

	public Transform player;

	public Transform curFloor;

    public Transform targetFloor;

	public State state;

    public int score;

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
        float scaleChange = maxScaleDuration * scaleSpeed;

        float halfHeight = curFloor.GetComponent<Floor>().ScaleY(scaleChange,maxScaleDuration);

        player.GetComponent<Player>().PressUp(scaleChange, 2 * halfHeight, maxScaleDuration);
    }

	public void OnPressUp(){
		curFloor.DOKill ();
		player.DOKill ();
		state = State.Jump;
        curFloor.GetComponent<Floor>().Revert(0.1f);
        player.GetComponent<Player>().JumpTo(targetFloor.transform.position);
	}


}
