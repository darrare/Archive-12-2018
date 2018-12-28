using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {
	bool isFixed = false;
	Vector2 originLocation;
	float timer = 0;
	BoxCollider2D triggerCollider;

	bool idle = true, attackStart = false, attackPush = false, attackEnd = false, runOnce = false;

	// Use this for initialization
	void Start () {
		timer = Random.Range (3f, 5f);
		triggerCollider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (idle) {
			Invoke("Delay", timer);
			idle = false;
		} else if (attackStart) {
			if (!runOnce) {
				runOnce = true;
				Invoke("AttackDelay", 2f);
			}
			if (transform.position.y - originLocation.y < .25f) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, .5f);
			} else {
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			}
		} else if (attackPush) {
			if (!runOnce) {
				runOnce = true;
				triggerCollider.enabled = true;
				Invoke("Attack2Delay", 1.5f);
			}
			if (transform.position.y - originLocation.y < .75f) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2.5f);
			} else {
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			}
		} else if (attackEnd) {
			if (!runOnce) {
				runOnce = true;
				Invoke("Reset", 2f);
			}
			if (transform.position.y - originLocation.y > 0f) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -.5f);
			} else {
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (!isFixed && collider.tag == "Ground") {
			isFixed = true;
			transform.position = collider.bounds.center;
			originLocation = transform.position;
		} else if (collider.tag == "Ground") {
			triggerCollider.enabled = false;
		}
		if (collider.tag == "Player") {
			collider.GetComponent<PlatformCharacterController2D>().Respawn ();
		}
	}

	void Delay()
	{
		attackStart = true;
	}

	void Reset()
	{
		idle = true;
		attackEnd = false;
		runOnce = false;
	}

	void AttackDelay()
	{
		attackStart = false;
		attackPush = true;
		runOnce = false;
	}

	void Attack2Delay()
	{
		attackPush = false;
		runOnce = false;
		attackEnd = true;
	}
}
