using UnityEngine;
using System.Collections;

public class LaserRay : MonoBehaviour {
	Vector2 direction;
	float speed = 7f;
	Quaternion rotation;
	GameObject spawnLocation;

	// Use this for initialization
	void Start () {
		spawnLocation = GameObject.Find ("LaserSpawn");
		spawnLocation.transform.parent.gameObject.GetComponent<Character> ().Battery -= 20;
		transform.position = spawnLocation.transform.position;
		Vector2 cameraPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		direction = new Vector2 (cameraPoint.x - spawnLocation.transform.position.x, cameraPoint.y - spawnLocation.transform.position.y);
		direction.Normalize ();
		GetComponent<Rigidbody2D> ().velocity = direction * speed;
		transform.RotateAround (spawnLocation.transform.position, Vector3.forward, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Ground") {
			Destroy (gameObject);
		} else if (collider.tag == "Item") {
			collider.gameObject.GetComponent<Rigidbody2D>().AddForce ((collider.gameObject.transform.position - transform.position) * 50);
			Destroy (gameObject);
		}
	}
}
