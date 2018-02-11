using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    }

	public void OnPressUp(){
		
		state = State.Jump;
	}


}
