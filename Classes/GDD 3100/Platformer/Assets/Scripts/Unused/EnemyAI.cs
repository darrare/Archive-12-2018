using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	Rigidbody2D thisRigidBody;
	bool isDestroyed = false;
	//bool onPlatform = false;
	// Use this for initialization
	void Start () {
		thisRigidBody = GetComponent<Rigidbody2D> ();
		thisRigidBody.velocity = new Vector3 (3, 0);
	}
	
	// Update is called once per frame
	void Update () {
//		if (!onPlatform && thisRigidBody.velocity.y < 5f) {
//			thisRigidBody.velocity = new Vector3 (thisRigidBody.velocity.x, 0);
//		} else if (onPlatform) {
//			thisRigidBody.velocity = new Vector3(thisRigidBody.velocity.x, 0);
//		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			PlatformCharacterController2D.Respawn ();
		} else if (!isDestroyed && collider.tag == "Weapon") {
			isDestroyed = true;
			GameObject.Find ("Character").GetComponent<PlatformCharacterController2D>().DestroyedEnemy();
		}
	}
//
//	void OnTriggerExit2D(Collider2D collider)
//	{
//		if (collider.tag == "Ground") {
//			onPlatform = false;
//		}
//	}

	public void SwitchDirection()
	{
		thisRigidBody.velocity = new Vector3 (-thisRigidBody.velocity.x, thisRigidBody.velocity.y);
		transform.localScale = new Vector3 (-transform.localScale.x, 1, 1);
	}
}
