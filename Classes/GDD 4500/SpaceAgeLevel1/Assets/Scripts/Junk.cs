using UnityEngine;
using System.Collections;

public class Junk : MonoBehaviour {
	Character player;
	bool hasThrusted = false;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character").GetComponent<Character>();
	}

	void Update()
	{
		if (!hasThrusted && Physics2D.gravity.y == 0) {
			hasThrusted = true;
			GetComponent<Rigidbody2D>().AddTorque(10f);
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-20, 20), Random.Range (-20, 20)));
		} else if (hasThrusted && Physics2D.gravity.y != 0) {
			hasThrusted = false;
		}
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.collider.tag == "Player") {
			player.DamagePlayer(25);
		}
	}
}
