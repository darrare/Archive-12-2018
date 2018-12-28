using UnityEngine;
using System.Collections;

public class FireWave : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("KillSwitch", 2);
		transform.localScale = new Vector3 (.1f, .1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.x < 1) {
			transform.localScale = new Vector3(transform.localScale.x + .05f, transform.localScale.y + .05f);
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Enemy") {
			collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collider.gameObject.GetComponent<Rigidbody2D>().velocity.x, 10);
		}
	}

	void KillSwitch()
	{
		Destroy (this.gameObject);
	}
}
