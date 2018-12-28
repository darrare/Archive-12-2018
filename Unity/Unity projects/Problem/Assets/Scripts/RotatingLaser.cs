using UnityEngine;
using System.Collections;

public class RotatingLaser : MonoBehaviour {
	float rotationVelocity = 1f;
	public bool clockwise = true;
	float originalRotation = 0f;

	// Use this for initialization
	void Start () {
		if (clockwise) {
			rotationVelocity = -rotationVelocity;
		}
		transform.GetChild (0).gameObject.transform.eulerAngles = new Vector3 (0, 0, transform.GetChild (0).gameObject.transform.eulerAngles.z + originalRotation);
	}

	public float CurrentRotation
	{
		get { return transform.GetChild (0).gameObject.transform.eulerAngles.z; }
		set { transform.GetChild (0).gameObject.transform.eulerAngles = new Vector3 (0, 0, value); }
	}

	public bool IsClockwise
	{
		get { return clockwise; }
		set { clockwise = value; }
	}
		
	// Update is called once per frame
	void Update () {
		transform.GetChild (0).gameObject.transform.RotateAround (transform.GetChild (0).transform.position, new Vector3 (0, 0, rotationVelocity), 125 * Time.deltaTime);
	}

	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.transform.tag == "Player") {
			collision.gameObject.GetComponent<PlayerController> ().KillPlayer ();
		}
	}

	public void SetClockwise(bool value)
	{
		if (clockwise && !value || !clockwise && value) {
			rotationVelocity = -rotationVelocity;
		}
		clockwise = value;
	}
}
