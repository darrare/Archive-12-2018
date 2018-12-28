using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		GameObject explosion = Instantiate (Resources.Load ("Explosion")) as GameObject;
		explosion.transform.position = new Vector2 (transform.position.x + GetComponent<Rigidbody2D> ().velocity.x / 5, transform.position.y + GetComponent<Rigidbody2D> ().velocity.y / 5);
		Destroy (gameObject);
	}
}
