using UnityEngine;
using System.Collections;

public class SawBlade : MonoBehaviour {
	float speed = 2f;
	float left, right, top, bottom;
	bool isFixed = false;
	Vector2 direction;
	int startDirection;

	// Use this for initialization
	void Start () {
		startDirection = Random.Range (0, 4);
		if (startDirection == 0) {
			direction = new Vector2 (0, 1 * speed);
		} else if (startDirection == 1) {
			direction = new Vector2 (0, -1 * speed);
		}else if (startDirection == 2) {
			direction = new Vector2 (1 * speed, 0);
		} else if (startDirection == 3) {
			direction = new Vector2 (-1 * speed, 0);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isFixed) {
			GetComponent<Rigidbody2D>().velocity = direction;
			GetComponent<Rigidbody2D>().transform.Rotate(Vector3.forward, -5 * speed);
			//going right
			if (direction.x == 1 * speed)
			{
				//if out of bounds send downwards
				if (transform.position.x > right)
				{
					direction = new Vector2(0, -1 * speed);
				}
			}
			//going down
			else if (direction.y == -1 * speed)
			{
				//if out of bounds send left
				if (transform.position.y < bottom)
				{
					direction = new Vector2(-1 * speed, 0);
				}
			}
			//going left
			else if (direction.x == -1 * speed)
			{
				//if out of bounds send up
				if (transform.position.x < left)
				{
					direction = new Vector2(0, 1 * speed);
				}
			}
			//going up
			else if (direction.y == 1 * speed)
			{
				//if out of bounds send downwards
				if (transform.position.y > top)
				{
					direction = new Vector2(1 * speed, 0);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Ground") {
			transform.position = collider.bounds.center;
			left = collider.bounds.center.x - collider.bounds.extents.x;
			right = collider.bounds.center.x + collider.bounds.extents.x;
			top = collider.bounds.center.y + collider.bounds.extents.y;
			bottom = collider.bounds.center.y - collider.bounds.extents.y;
			isFixed = true;
		} else if (collider.tag == "Player") {
			collider.GetComponent<PlatformCharacterController2D>().Respawn ();
		}

	}

	bool IsInBounds()
	{
		if (transform.position.x < right && transform.position.x > left) {
			if (transform.position.y > bottom && transform.position.y < top) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
}
