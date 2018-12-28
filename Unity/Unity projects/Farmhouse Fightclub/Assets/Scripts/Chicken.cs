using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chicken : PlayerControlledObject {
	float horizontalSpeedModifier = 5f;
	float verticalJumpSpeed = 5f;
	float verticalJumpSpeedIncrements = .7f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		hpText = GameObject.Find ("ChickenHealth").GetComponent<Text>();
		baseText = "Chicken HP:";
	}

	//update is called once every frame
	protected override void Update () {
		base.Update ();
		if (GetComponent<Rigidbody2D> ().velocity.y > .1f) {
			gameObject.layer = LayerMask.NameToLayer ("GoThroughPlatforms");
		} else {
			gameObject.layer = LayerMask.NameToLayer ("InteractWithPlatforms");
		}
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * horizontalSpeedModifier, GetComponent<Rigidbody2D> ().velocity.y);
		if (transform.position.y < 4) {
			if (Input.GetAxis("Vertical") > .1f) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * horizontalSpeedModifier, verticalJumpSpeed);
			}
		}
		else
		{
			if (Input.GetAxis("Vertical") < -.1f) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * horizontalSpeedModifier, GetComponent<Rigidbody2D> ().velocity.y - verticalJumpSpeedIncrements);
			}
		}

//		if (recentlyJumped && Input.GetAxis("Vertical") > .1f) {
//			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * horizontalSpeedModifier, GetComponent<Rigidbody2D> ().velocity.y + verticalJumpSpeedIncrements);
//		}

		if (Input.GetButtonDown ("Fire1")) {
			FireWeapon ();
		}
	}

	void FireWeapon()
	{
		Vector2 dir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		dir.Normalize ();

		GameObject bullet = Instantiate (Resources.Load ("Bullet")) as GameObject;
		bullet.transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg);
		bullet.transform.position = transform.position;
		bullet.GetComponent<Rigidbody2D> ().velocity = dir * 5f;
	}
}