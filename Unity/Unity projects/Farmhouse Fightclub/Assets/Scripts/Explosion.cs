using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Invoke ("DestroyExplosion", .2f);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{

		if (collider.tag == "PlayerControlledObject") {
			collider.GetComponent<PlayerControlledObject> ().TakeDamage (50);
		}

	}

	void DestroyExplosion()
	{
		Destroy (gameObject);
	}
}
