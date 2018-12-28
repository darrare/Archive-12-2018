using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pig : PlayerControlledObject {
	float horizontalSpeedModifier = 5f;
	float verticalJumpSpeed = 5f;
	float verticalJumpSpeedIncrements = .7f;
	bool recentlyJumped = false;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		hpText = GameObject.Find ("PigHealth").GetComponent<Text>();
		baseText = "Pig HP:";
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (GetComponent<Rigidbody2D> ().velocity.y > .1f) {
			gameObject.layer = LayerMask.NameToLayer ("GoThroughPlatforms");
		} else {
			gameObject.layer = LayerMask.NameToLayer ("InteractWithPlatforms");
		}
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal1") * horizontalSpeedModifier, GetComponent<Rigidbody2D> ().velocity.y);
		if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.y) <= .2f && transform.position.y < 4) {
			//player is on the ground
			if (Input.GetAxis("Vertical1") > .1f) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal1") * horizontalSpeedModifier, GetComponent<Rigidbody2D> ().velocity.y + verticalJumpSpeed);
				recentlyJumped = true;
				Invoke("DisableJumpIncrements", .2f);
			}
		}
		else
		{
			if (Input.GetAxis("Vertical1") < -.1f) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal1") * horizontalSpeedModifier, GetComponent<Rigidbody2D> ().velocity.y - verticalJumpSpeedIncrements);
			}
		}

		if (recentlyJumped && Input.GetAxis("Vertical1") > .1f) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal1") * horizontalSpeedModifier, GetComponent<Rigidbody2D> ().velocity.y + verticalJumpSpeedIncrements);
		}

		if (Input.GetButtonDown ("Fire11")) {
			FireWeapon ();
		}
	}

	void DisableJumpIncrements()
	{
		recentlyJumped = false;
	}

	void FireWeapon()
	{
		Vector2 dir = new Vector2 (Input.GetAxis ("RocketAimHorizontal"), -Input.GetAxis ("RocketAimVertical"));
		dir.Normalize ();

		GameObject rocket = Instantiate (Resources.Load ("Rocket")) as GameObject;
		rocket.transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg);
		rocket.transform.position = transform.position;
		rocket.GetComponent<Rigidbody2D> ().velocity = dir * 5f;
		if (dir == Vector2.zero) {
			rocket.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * 5f;
		}
	}
}
