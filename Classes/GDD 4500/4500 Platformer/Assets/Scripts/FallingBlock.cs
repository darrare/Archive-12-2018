using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {
	Vector3 startPosition;
	Transform child;
	bool wiggleLeft = true;
	BoxCollider2D boxCollider;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		child = transform.FindChild ("FallingBlockSprite");
		boxCollider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.tag == "Player") {
			InvokeRepeating ("WiggleChild", 0, .05f);
			Invoke ("StartFall", .5f);
		}
	}

	void WiggleChild()
	{
		wiggleLeft = !wiggleLeft;
		if (wiggleLeft) {
			child.localPosition = new Vector2 (-.1f, 0);
		} else {
			child.localPosition = new Vector2(.1f, 0);
		}
	}

	void StartFall()
	{
		CancelInvoke ("WiggleChild");
		child.localPosition = Vector2.zero;
		GetComponent<Rigidbody2D> ().isKinematic = false;
		boxCollider.enabled = false;
		Invoke ("ResetFall", 5);
	}

	void ResetFall()
	{
		GetComponent<Rigidbody2D> ().isKinematic = true;
		transform.position = startPosition;
		boxCollider.enabled = true;
	}
}
