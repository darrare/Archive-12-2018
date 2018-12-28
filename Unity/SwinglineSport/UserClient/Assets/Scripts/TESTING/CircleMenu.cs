using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CircleMenu : MonoBehaviour {
	public GameObject[] leafButtons = new GameObject[6];
	bool isActive = false;
	float time = 0;
	float fingerAngle, distance;
	int index = 999;
	Color swingLineGreen = new Color (61/255 , 255/255, 64/255, 1);

	// Use this for initialization
	void Start () {
		HideLeafButtons ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			SceneManager.LoadScene ("MainMenu");
		}
		if (Input.touchCount == 1 && CheckForFingerDistance(Camera.main.ScreenToWorldPoint(Input.GetTouch (0).position), .65f)) {
			isActive = true;
		} else if (Input.touchCount != 1) {
			//SAVE WHAT WE ARE CLICKING ON AND THEN HIDE BUTTONS
			if (index != 999) {
				switch (index) {
				case 0:
					ButtonZeroSelect ();
					break;
				}
			}
			HideLeafButtons();
			isActive = false;
		}

		if (isActive) {
			LerpLeafButtons ();
			if (time > 1) {
				fingerAngle = FingerAngle (Camera.main.ScreenToWorldPoint(Input.GetTouch (0).position));
				//fingerAngle = FingerAngle (Camera.main.ScreenToWorldPoint(Input.mousePosition));
				ShowWhichLeafUserIsOn (fingerAngle);
			}

		}
			



		//KEYBOARD DEBUG SHIT BELOW
//		if (Input.GetKeyDown (KeyCode.A)) {
//			isActive = true;
//		}
//		if (Input.GetKeyDown (KeyCode.S)) {
//			isActive = false;
//			HideLeafButtons ();
//		}
	}

	void HideLeafButtons()
	{
		foreach (GameObject obj in leafButtons) {
			obj.GetComponent<RectTransform> ().localScale = new Vector3 (.5f, .5f, 1);
			obj.GetComponent<Image> ().color = Color.clear;
			obj.SetActive (false);
			time = 0;
		}
	}

	void LerpLeafButtons()
	{
		time += Time.deltaTime * 5;
		if (time <= 1) {
			foreach (GameObject obj in leafButtons) {
				obj.SetActive (true);
				obj.GetComponent<RectTransform> ().localScale = new Vector3 (Mathf.Lerp (.5f, 1f, time), Mathf.Lerp (.5f, 1f, time), 1);
				obj.GetComponent<Image> ().color = Color.Lerp (Color.clear, Color.white, time);
			}
		}
	}

	bool CheckForFingerDistance(Vector2 fingerPos, float dist)
	{
		float x = this.GetComponent<RectTransform> ().position.x;
		float y = this.GetComponent<RectTransform> ().position.y;
		float distance = Mathf.Sqrt (Mathf.Pow(fingerPos.x - x, 2) + Mathf.Pow(fingerPos.y - y, 2));
			
		if (distance < dist) {
			return true;
		}
		return false;
	}

	float FingerAngle(Vector3 fingerPos)
	{
		Vector3 targetDir = fingerPos - this.GetComponent<RectTransform> ().position;
		if (fingerPos.y >= this.GetComponent<RectTransform> ().position.y) {
			return Vector2.Angle (targetDir, transform.right);
		}
		return (180 + (180 - Vector2.Angle(targetDir, transform.right)));
	}

	void ShowWhichLeafUserIsOn(float angle)
	{
		index = 999;
		if (!CheckForFingerDistance (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), .65f)) {
			if (angle >= 60 && angle < 120) {
				index = 0;
			} else if (angle >= 120 && angle < 180) {
				index = 1;
			} else if (angle >= 180 && angle < 240) {
				index = 2;
			} else if (angle >= 240 && angle < 300) {
				index = 3;
			} else if (angle >= 300 && angle < 360) {
				index = 4;
			} else if (angle >= 0 && angle < 60) {
				index = 5;
			}
		}

		for (int i = 0; i < leafButtons.Length; i++) {
			if (i == index) {
				leafButtons [i].GetComponent<RectTransform> ().localScale = new Vector2 (1.2f, 1.2f);
				//leafButtons [i].GetComponent<Image> ().color = swingLineGreen;
			} else {
				leafButtons [i].GetComponent<RectTransform> ().localScale = new Vector2 (1f, 1f);
				//leafButtons [i].GetComponent<Image> ().color = Color.white;
			}
		}
	}

	void ButtonZeroSelect()
	{
		SceneManager.LoadScene ("MainMenu");
	}
		
}
