using UnityEngine;
using System.Collections;

public class LevelDesignCameraControl : MonoBehaviour {
	Camera cam;
	Vector3 previousMousePos;
	Vector3 mousePosDelta;
	bool isUsable = true;

	float moveAmount = .2f;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Mouse controls
		if (isUsable) {
			//If the user is scrolling the mouse wheel to zoom in or out
			cam.orthographicSize = Mathf.Clamp (cam.orthographicSize - Input.mouseScrollDelta.y, 3, 30);

			//If the user presses the middle mouse button to pan the camera. Very ghetto version but it should work.
			if (Input.GetMouseButtonDown (2)) {
				previousMousePos = Input.mousePosition;
			}
			else if (Input.GetMouseButton (2)) {
				cam.ScreenToWorldPoint (Input.mousePosition);
				mousePosDelta = previousMousePos - Input.mousePosition;
				previousMousePos = Input.mousePosition;
				transform.position += mousePosDelta * cam.orthographicSize * .002f;
			}

			//WASD panning
		
			if (Input.GetKey (KeyCode.W)) {
				transform.position += new Vector3 (0, moveAmount, 0);
			}
			if (Input.GetKey (KeyCode.A)) {
				transform.position -= new Vector3 (moveAmount, 0, 0);
			}
			if (Input.GetKey (KeyCode.S)) {
				transform.position -= new Vector3 (0, moveAmount, 0);
			}
			if (Input.GetKey (KeyCode.D)) {
				transform.position += new Vector3 (moveAmount, 0, 0);
			}
		}
	}

	public void SetUsable(bool value)
	{
		isUsable = value;
	}
}
