using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tutorial1Control : MonoBehaviour {
	float[] cameraLimits = new float[4];
	enum Stage {wasd, waiting0, climbingIntro, climbing, waiting1, runThenClimb, waiting2, jumping, finish};
	Stage stage = Stage.wasd;

	int jumpCount = 0;
	bool canJump = true;

	Rigidbody2D rigidBody;
	GameObject chatBubble;
	Animator anim;

	Quest test;
	Quest IntroToMovement;

	// Use this for initialization
	void Start () {
		//test = new Quest ("this is a test", "this is a test to see if what i think will happen happens.", new List<string> {"hey", "ho", "go"});

		IntroToMovement = new Quest ("Intro to movement", "Follow the guide to learn how to maneuver around the game world. \n " +
			"WASD and the left thumbstick to move. \n" +
			"Hold W or UP to climb vines. \n" +
			"Jump with SPACE or the X button. ", new List<string> {"Move", "Climb", "Jump"}, true);

		cameraLimits [0] = -3.35f;
		cameraLimits [1] = 1.37f;
		cameraLimits [2] = -2f;
		cameraLimits [3] = 4.2f;
		GameObject.Find ("Main Camera").GetComponent<PlatformCameraControl> ().SetCameraLimits (cameraLimits);
		rigidBody = GetComponent<Rigidbody2D> ();
		chatBubble = transform.GetChild (0).transform.GetChild (0).gameObject;
		chatBubble.SetActive (false);
		anim = GetComponent<Animator> ();
	}

	void Update()
	{
		if (rigidBody.velocity.x < 0) {
			transform.localScale = new Vector3 (-1, 1, 1);
		} else {
			transform.localScale = new Vector3 (1, 1, 1);
		}

		if (rigidBody.velocity.x != 0) {
			anim.SetBool ("running", true);
		} else {
			anim.SetBool ("running", false);
		}

		if (rigidBody.velocity.y > 0) {
			anim.SetBool ("climbing", true);
		} else {
			anim.SetBool ("climbing", false);
		}

		if (stage == Stage.wasd) {
			if (transform.position.x > -2) {
				rigidBody.velocity = new Vector2 (-4, rigidBody.velocity.y);
			} else {
				rigidBody.velocity = new Vector2 (0, 0);
				chatBubble.SetActive (true);
				stage = Stage.waiting0;
			}
		} else if (stage == Stage.waiting0) {
			if (GameObject.Find ("Character").GetComponent<Rigidbody2D> ().velocity.x != 0) {
				IntroToMovement.CompleteTask (0);
				chatBubble.SetActive (false);
				chatBubble.GetComponent<RawImage> ().texture = Resources.Load ("ChatBubbles/climbingBubble") as Texture;
				stage = Stage.climbingIntro;
			}
		} else if (stage == Stage.climbingIntro) {
			if (GameObject.Find ("Character").transform.position.x > 2) {
				chatBubble.GetComponent<RawImage> ().texture = Resources.Load ("ChatBubbles/jumpingBubble") as Texture;
				chatBubble.SetActive (false);
				stage = Stage.climbing;
			} else if (transform.position.x < 3) {
				rigidBody.velocity = new Vector2 (4, rigidBody.velocity.y);
			} else {
				rigidBody.velocity = new Vector2 (0, 0);
				chatBubble.SetActive (true);
			}

		} else if (stage == Stage.climbing) {
			if (transform.position.y < .1f) {
				rigidBody.velocity = new Vector2 (0, 2);
			} else {
				stage = Stage.waiting1;
			}
		} else if (stage == Stage.waiting1) {
			if (transform.position.x > 1.45f) {
				rigidBody.velocity = new Vector2 (-2, rigidBody.velocity.y);
			} else {
				rigidBody.velocity = new Vector2 (0, 0);
				if (GameObject.Find ("Character").transform.position.y > -.7f) {
					IntroToMovement.CompleteTask (1);
					stage = Stage.runThenClimb;
				}
			}
		} else if (stage == Stage.runThenClimb) {
			if (transform.position.x > -3.24f) {
				rigidBody.velocity = new Vector2 (-2, rigidBody.velocity.y);
			} else {
				rigidBody.isKinematic = true;
				rigidBody.velocity = new Vector2 (0, 0);
				rigidBody.velocity = new Vector2 (0, 2);
				if (transform.position.y > 1.53f) {
					rigidBody.isKinematic = false;
					stage = Stage.waiting2;
				}
			}
		} else if (stage == Stage.waiting2) {
			chatBubble.SetActive (true);
			if (GameObject.Find ("Character").transform.position.y > 2.5f) {
				chatBubble.GetComponent<RawImage> ().texture = Resources.Load ("ChatBubbles/finishBubble") as Texture;
				chatBubble.SetActive (false);
				stage = Stage.jumping;
			}
		} else if (stage == Stage.jumping) {
			if (jumpCount < 3) {
				rigidBody.velocity = new Vector2 (2, rigidBody.velocity.y);
			} else {
				rigidBody.velocity = new Vector2 (-2.5f, rigidBody.velocity.y);
			}

			if (canJump && rigidBody.velocity.y == 0 && jumpCount < 4) {
				canJump = false;
				Invoke ("JumpReset", .1f);
				jumpCount++;
				rigidBody.AddForce (new Vector2 (0, 400));
			}
			if (transform.position.x < -3.9f) {
				rigidBody.velocity = new Vector2 (0, 0);
				stage = Stage.finish;
			}
		} else if (stage == Stage.finish) {
			chatBubble.SetActive (true);
			if (GameObject.Find ("Character").transform.position.y > 5)
			{
				IntroToMovement.CompleteTask (2);
			}
		}
	}

	void JumpReset()
	{
		canJump = true;
	}
}
