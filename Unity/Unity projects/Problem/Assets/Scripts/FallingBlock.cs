using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {

	Transform sprite;
	bool isFallStarted = false;
	float xPos;
	float shiftLength = .5f;
	float accelerator = 0;
	Vector2 origin;
	Vector2 originalLocation;

	// Use this for initialization
	void Start () {
		sprite = transform.GetChild (0);
		origin = sprite.localPosition;
		originalLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFallStarted) {
			accelerator += Time.deltaTime * 2;
			if (accelerator > 2) {
				sprite.localPosition = origin;
				GetComponent<Rigidbody2D> ().isKinematic = false;
				GetComponent<BoxCollider2D> ().enabled = false;
				if (transform.FindChild ("Character") != null) {
					transform.FindChild ("Character").GetComponentInChildren<CheckForGround> ().OnTriggerExit2D (GetComponent<BoxCollider2D> ());
				}
			} else {
				xPos = Mathf.PingPong (Time.time * accelerator, shiftLength) - (shiftLength / 2);
				sprite.localPosition = new Vector2(xPos, sprite.localPosition.y);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "PlayerGroundCheck" && !isFallStarted) {
			//CONSTANTS.levelController.AddItemToRespawnList ("FallingBlock", transform.position, transform.rotation);
			isFallStarted = true;
			Invoke ("ResetClock", 5f);
		}
	}

	void ResetClock()
	{
		isFallStarted = false;
		accelerator = 0;
		transform.position = originalLocation;
		GetComponent<Rigidbody2D> ().isKinematic = true;
		GetComponent<BoxCollider2D> ().enabled = true;
	}
}
