using UnityEngine;
using System.Collections;

public class BarrelCannon : MonoBehaviour {
	Rigidbody2D playerBody;
	bool hasFired = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		hasFired = false;
		if (collider.tag == "Player") {
			collider.transform.position = transform.position;
			playerBody = collider.gameObject.GetComponent<Rigidbody2D>();
			playerBody.velocity = Vector3.zero;
			Invoke ("Fire", 1);
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (hasFired == false && collider.tag == "Player") {
			collider.transform.position = transform.position;
			playerBody.velocity = Vector3.zero;
		}
	}

	void Fire()
	{
		hasFired = true;
		playerBody.velocity = new Vector3 (0, 23);
	}
}
