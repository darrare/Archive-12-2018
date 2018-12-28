using UnityEngine;
using System.Collections;

public class Piston : MonoBehaviour {
	enum PistonState { GOINGUP, IDLE, GOINGDOWN };
	PistonState pistonState;
	public float delayTime;
	public float idleTime;
	public bool isHorizontal = false;
	[Tooltip("Base time is 1 seconds")]
	public float velocityChange;
	float idleTimeCurrent = 0f;
	GameObject pistonArm;
	Vector2 origin;
	float maxChangeDistance = .5525f;
	bool isRunning = false;

	// Use this for initialization
	void Start () {
		pistonArm = gameObject.transform.GetChild (0).gameObject;
		origin = pistonArm.transform.position;
		pistonState = PistonState.IDLE;
		Invoke ("StartPiston", delayTime);
	}
	
	// Update is called once per frame
	void Update () {
		//when the position is vertical
		if (isRunning && !isHorizontal) {
			if (pistonState == PistonState.GOINGUP) {
				idleTimeCurrent += Time.deltaTime;
				pistonArm.transform.position = new Vector2 (pistonArm.transform.position.x, Mathf.Lerp (origin.y - maxChangeDistance, origin.y + maxChangeDistance, idleTimeCurrent * velocityChange));
				if (Mathf.Abs (pistonArm.transform.position.y - (origin.y + maxChangeDistance)) < .03f) {
					pistonState = PistonState.IDLE;
					idleTimeCurrent = 0;
				}
			} else if (pistonState == PistonState.IDLE) {
				idleTimeCurrent += Time.deltaTime;
				if (idleTimeCurrent >= idleTime) {
					if (pistonArm.transform.position.y >= origin.y) {
						pistonState = PistonState.GOINGDOWN;
						idleTimeCurrent = 0;
					} else if (pistonArm.transform.position.y < origin.y) {
						pistonState = PistonState.GOINGUP;
						idleTimeCurrent = 0;
					}
				}
			} else if (pistonState == PistonState.GOINGDOWN) {
				idleTimeCurrent += Time.deltaTime;
				pistonArm.transform.position = new Vector2 (pistonArm.transform.position.x, Mathf.Lerp (origin.y + maxChangeDistance, origin.y - maxChangeDistance, idleTimeCurrent * velocityChange));
				if (Mathf.Abs (pistonArm.transform.position.y - (origin.y - maxChangeDistance)) < .03f) {
					pistonState = PistonState.IDLE;
					idleTimeCurrent = 0;
				}
			}
		} 

		//When the piston is horizontal
		else if (isRunning && isHorizontal) {
			if (pistonState == PistonState.GOINGUP) {
				idleTimeCurrent += Time.deltaTime;
				pistonArm.transform.position = new Vector2 (Mathf.Lerp (origin.x - maxChangeDistance, origin.x + maxChangeDistance, idleTimeCurrent * velocityChange), pistonArm.transform.position.y);
				if (Mathf.Abs (pistonArm.transform.position.x - (origin.x + maxChangeDistance)) < .03f) {
					pistonState = PistonState.IDLE;
					idleTimeCurrent = 0;
				}
			} else if (pistonState == PistonState.IDLE) {
				idleTimeCurrent += Time.deltaTime;
				if (idleTimeCurrent >= idleTime) {
					if (pistonArm.transform.position.x >= origin.x) {
						pistonState = PistonState.GOINGDOWN;
						idleTimeCurrent = 0;
					} else if (pistonArm.transform.position.x < origin.x) {
						pistonState = PistonState.GOINGUP;
						idleTimeCurrent = 0;
					}
				}
			} else if (pistonState == PistonState.GOINGDOWN) {
				idleTimeCurrent += Time.deltaTime;
				pistonArm.transform.position = new Vector2 (Mathf.Lerp (origin.x + maxChangeDistance, origin.x - maxChangeDistance, idleTimeCurrent * velocityChange), pistonArm.transform.position.y);
				if (Mathf.Abs (pistonArm.transform.position.x - (origin.x - maxChangeDistance)) < .03f) {
					pistonState = PistonState.IDLE;
					idleTimeCurrent = 0;
				}
			}
		}
	}

	void StartPiston()
	{
		isRunning = true;
	}
}
