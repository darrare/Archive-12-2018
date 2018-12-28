using UnityEngine;
using System.Collections;

public class YrouverWestHoodlum : MonoBehaviour {
	float[] cameraLimits = new float[4];
	bool hasRunAway = false;

	// Use this for initialization
	void Start () {
		cameraLimits [0] = -2.67f;
		cameraLimits [1] = 2.67f;
		cameraLimits [2] = 0f;
		cameraLimits [3] = 0f;
		GameObject.Find ("Main Camera").GetComponent<PlatformCameraControl> ().SetCameraLimits (cameraLimits);
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasRunAway && Vector2.Distance (transform.position, GameObject.Find ("Character").transform.position) < 3.4f) {
			hasRunAway = true;
			GameObject oxyClean = Instantiate (Resources.Load ("OxyClean")) as GameObject;
			oxyClean.transform.position = this.transform.position;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (-5, GetComponent<Rigidbody2D>().velocity.y);
		}
		if (transform.position.x < -4.5) {
			Destroy (gameObject);
		}
	}
}
