using UnityEngine;
using System.Collections;

public class Golem : MonoBehaviour {
	Animator anim;
	Rigidbody2D rigidBody;
	enum Direction {left, still, right};
	Direction direction;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		InvokeRepeating ("ChangeDirection", 0, 2);
		ChangeDirection ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rigidBody.velocity.x != 0) {
			anim.SetBool ("isMoving", true);
		} else if (rigidBody.velocity.x == 0) {
			anim.SetBool ("isMoving", false);
		}
		
		if (direction == Direction.left) {
			rigidBody.velocity = new Vector2 (-.5f, rigidBody.velocity.y);
			transform.localScale = new Vector3(1, 1, 1);
		} else if (direction == Direction.right) {
			rigidBody.velocity = new Vector2 (.5f, rigidBody.velocity.y);
			transform.localScale = new Vector3(-1, 1, 1);
		} else if (direction == Direction.still) {
			rigidBody.velocity = new Vector2 (0, rigidBody.velocity.y);
		}
	}

	void ChangeDirection()
	{
		float rand = Random.Range (0.00f, 1.00f);
		if (rand >= .66) {
			direction = Direction.left;
		} else if (rand <= .33) {
			direction = Direction.right;
		} else {
			direction = Direction.still;
		}
	}
	
	void ChangeDirection(Direction direction)
	{
		if (direction == Direction.left) {
			this.direction = Direction.right;
		} else if (direction == Direction.right) {
			this.direction = Direction.left;
		}
	}
	
	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Ground") {
			ChangeDirection (direction);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			Tutorial2Control.FailQuest ();
			PlatformCharacterController2D.Respawn ();
		}
	}
}
