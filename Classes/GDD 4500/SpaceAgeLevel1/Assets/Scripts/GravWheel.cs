using UnityEngine;
using System.Collections;

public class GravWheel : MonoBehaviour {
	bool canUse = false;
	float rotation = 0;
	Transform bar;
	Color positive;
	Color negative;
	Color zero;
	SpriteRenderer barColor;

	// Use this for initialization
	void Start () {
		bar = transform.parent.FindChild ("gravitySignPart2").transform;
		positive = new Color (0, 255, 0);
		negative = new Color (255, 0, 0);
		zero = new Color (0, 0, 255);
		barColor = bar.gameObject.GetComponent<SpriteRenderer> ();
	}
	
	// FixedUpdate is called 60 times a second
	void FixedUpdate () {
		if (canUse && Input.GetButton ("Action")) {
			rotation = Input.GetAxis ("Horizontal");
			if (rotation != 0) {
				transform.Rotate (new Vector3 (0, 0, -rotation * 2));
				if (rotation > 0 && bar.localScale.x < 1) {
					bar.localScale = new Vector3 (bar.localScale.x + .01f, 1, 1);
				} else if (rotation < 0 && bar.localScale.x > -1) {
					bar.localScale = new Vector3 (bar.localScale.x - .01f, 1, 1);
				}
				if (bar.localScale.x > 0) {
					barColor.color = positive;
				} else {
					barColor.color = negative;
				}
			}
			if (bar.localScale.x < .15f && bar.localScale.x > -.15f) {
				barColor.color = zero;
				Physics2D.gravity = new Vector3 (0, 0, 0);
			} else {
				Physics2D.gravity = new Vector3 (0, bar.localScale.x * -9.81f, 0);
			}
		} else {
			bar.localScale = new Vector3(-Physics2D.gravity.y / 9.81f, 1, 1);
			if (Physics2D.gravity.y == 0)
			{
				barColor.color = zero;
			} 
			else if (Physics2D.gravity.y > 0)
			{
				barColor.color = negative;
			}
			else
			{
				barColor.color = positive;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			canUse = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			canUse = false;
		}
	}

}
