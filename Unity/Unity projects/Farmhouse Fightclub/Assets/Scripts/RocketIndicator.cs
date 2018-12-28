using UnityEngine;
using System.Collections;

public class RocketIndicator : MonoBehaviour {
	Vector2 dir;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		dir = new Vector2 (Input.GetAxis ("RocketAimHorizontal"), -Input.GetAxis ("RocketAimVertical"));
		dir.Normalize ();

		gameObject.transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg);
	}
}
