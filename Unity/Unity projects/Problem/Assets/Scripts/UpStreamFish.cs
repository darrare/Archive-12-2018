using UnityEngine;
using System.Collections;

public class UpStreamFish : MonoBehaviour {
	Rigidbody2D rbody;
	BoxCollider2D platform;
	public float velocity;
	public float jumpVelocity;
	public float yPositionToJumpAt;
	public float xPositionToStopAt;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
		platform = GetComponent<BoxCollider2D> ();
		//Physics2D.IgnoreCollision (CONSTANTS.levelController.Player.GetComponent<PolygonCollider2D> (), platform, true);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector2 (transform.position.x + velocity * Time.deltaTime, transform.position.y + ((rbody.velocity.y) * Time.deltaTime));
		if (transform.position.y < yPositionToJumpAt && transform.position.x < xPositionToStopAt) {
			rbody.velocity = new Vector2 (rbody.velocity.x, jumpVelocity);
		}

		if (transform.position.y < yPositionToJumpAt - 15) {
			Destroy (gameObject);
		}
	}

	public void SetStats(float velocity, float jumpVelocity, float yPositionToJumpAt, float xPositionToStopAt)
	{
		this.velocity = velocity;
		this.jumpVelocity = jumpVelocity;
		this.yPositionToJumpAt = yPositionToJumpAt;
		this.xPositionToStopAt = xPositionToStopAt;
	}

//	void OnTriggerEnter2D(Collider2D collider)
//	{
//		if (collider.tag == "Player") {
//			Physics2D.IgnoreCollision (collider.GetComponent<PolygonCollider2D> (), platform, false);
//		}
//	}
//
//	void OnTriggerExit2D(Collider2D collider)
//	{
//		if (collider.tag == "Player") {
//			Physics2D.IgnoreCollision (collider.GetComponent<PolygonCollider2D> (), platform, true);
//		}
//	}
}
