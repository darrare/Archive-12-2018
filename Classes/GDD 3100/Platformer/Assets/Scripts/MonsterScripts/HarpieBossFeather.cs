using UnityEngine;
using System.Collections;

public class HarpieBossFeather : MonoBehaviour {
	Vector2 target;
	Vector2 direction;

	// Use this for initialization
	void Start () {
		Invoke ("SmallDelay", 1);
	}

	void Update()
	{
		GetComponent<Rigidbody2D> ().rotation += 7;
	}

	void SmallDelay()
	{
		target = GameObject.Find ("Character").transform.position;
		direction = new Vector2 (target.x - transform.position.x, target.y - transform.position.y);
		direction.Normalize ();
		GetComponent<Rigidbody2D> ().velocity = direction * Random.Range (3.0f, 5.0f);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			//deal damage to player here.
			Destroy (gameObject);
		}
	}
}
