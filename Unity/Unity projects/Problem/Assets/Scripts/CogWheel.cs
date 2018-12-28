using UnityEngine;
using System.Collections;

public class CogWheel : MonoBehaviour {
	public float rotationMultiplier = 50;
	public bool isClockwise = true;

	// Update is called once per frame
	void Update () {
		if (!isClockwise) {
			transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z + Time.deltaTime * rotationMultiplier);
		} else {
			transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z - Time.deltaTime * rotationMultiplier);
		}
	}
}
