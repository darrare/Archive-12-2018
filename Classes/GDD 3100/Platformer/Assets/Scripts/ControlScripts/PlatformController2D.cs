using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformController2D : MonoBehaviour {

	private Transform groundCheckPosition;
	private Rigidbody2D playerRigidBody;
	private float groundBuffer = .3f;
	private float length;
	private float height;
	private Vector2 dimensions;
	private float topMost;
	private List<ColliderInfo> colliders = new List<ColliderInfo> ();

	// Use this for initialization
	void Start () {
		groundCheckPosition = GameObject.Find ("Character").transform.Find ("GroundCheck");
		playerRigidBody = GameObject.Find ("Character").GetComponent<Rigidbody2D> ();

		foreach (BoxCollider2D collider in GetComponents<BoxCollider2D>()) {
			//creates a new ColliderInfo object so we can loop through it later
			length = transform.localScale.x * (collider.size.x);
			height = transform.localScale.y * (collider.size.y);
			dimensions = new Vector2(length, height);
			topMost = (transform.position.y + collider.offset.y) + dimensions.y / 2;
			colliders.Add (new ColliderInfo(collider, dimensions, topMost));
		}

	}
	
	// Update is called once per frame
	void Update () {
		foreach (ColliderInfo collider in colliders) {
			if (playerRigidBody.velocity.y > .05f)
			{
				collider.GetCollider.isTrigger = true;
			}
			else if (Input.GetAxis ("Vertical") == -1)
			{
				collider.GetCollider.isTrigger = true;
			}
			else if (collider.GetTopMost - groundBuffer > groundCheckPosition.position.y)
			{
				//set the collider to a trigger so we can pass right through it
				collider.GetCollider.isTrigger = true;
			}
			else if (playerRigidBody.velocity.y == 0)
			{
				collider.GetCollider.isTrigger = false;
			}
			else 
			{
				//set the collider as not a trigger so it holds us
				collider.GetCollider.isTrigger = false;

			}
		}
	}
}

public class ColliderInfo {
	BoxCollider2D collider;
	Vector2 dimensions;
	float topMost;
	public ColliderInfo(BoxCollider2D collider, Vector2 dimensions, float topMost)
	{
		this.collider = collider;
		this.dimensions = dimensions;
		this.topMost = topMost;
	}

	public BoxCollider2D GetCollider
	{
		get { return collider; }
	}

	public Vector2 GetDimensions
	{
		get { return dimensions; }
	}

	public float GetTopMost
	{
		get { return topMost; }
	}
}
