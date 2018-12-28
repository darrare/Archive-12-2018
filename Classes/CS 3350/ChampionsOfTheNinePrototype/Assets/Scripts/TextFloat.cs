using UnityEngine;
using System.Collections;

public class TextFloat : MonoBehaviour {
	Vector3 position;
	// Use this for initialization
	void Start () {
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y + .02f);
		if (transform.position.y - position.y > 1) {
			Destroy (this.gameObject);
		}
	}
}
