using UnityEngine;
using System.Collections;

public class WindGust : MonoBehaviour {
	GameObject player;
	Rigidbody2D thisRigidBody;

	// Use this for initialization
	void Start () {
		Invoke ("KillSwitch", 5);
		thisRigidBody = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Character");
		if (player.transform.localScale.x == -1) {
			//player is facing left
			transform.position = new Vector3(player.transform.position.x - .2f, player.transform.position.y);
			thisRigidBody.velocity = new Vector3(-8f, 0, 0);
		} 
		else {
			transform.position = new Vector3(player.transform.position.x + .2f, player.transform.position.y);
			thisRigidBody.velocity = new Vector3(8f, 0, 0);
		}
	}

	void KillSwitch()
	{
		Destroy (this.gameObject);
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Enemy") {
			collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(thisRigidBody.velocity.x, collider.gameObject.GetComponent<Rigidbody2D>().velocity.y);
		}
	}

}
