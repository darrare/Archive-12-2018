using UnityEngine;
using System.Collections;

public class LadderClimb : MonoBehaviour {
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character");
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player" && Input.GetAxis ("Vertical") != 0) {
			player.GetComponent<Animator> ().SetBool ("climbing", true);
			if (player.GetComponent<Animator> ().GetBool ("climbing"))
			{
				//if player is holding up key
				if (collider.tag == "Player" && Input.GetAxis ("Vertical") > .1f) {
					player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (player.GetComponent<Rigidbody2D> ().velocity.x, 2);
				} else if (collider.tag == "Player" && Input.GetAxis ("Vertical") == 0) {
					player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (player.GetComponent<Rigidbody2D> ().velocity.x, 0);
				} else if (collider.tag == "Player" && Input.GetAxis ("Vertical") < -.1f) {
					player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (player.GetComponent<Rigidbody2D> ().velocity.x, -1);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			player.GetComponent<Animator>().SetBool("climbing", false);
		}
	}
}
