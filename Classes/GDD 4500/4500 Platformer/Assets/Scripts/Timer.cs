using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	GameObject begin;
	GameObject end;
	GameObject player;
	string baseTimeText = "Time: ";
	Text timeText;
	float time = 0;

	bool isTimeStarted = false;

	// Use this for initialization
	void Start () {
		begin = transform.FindChild ("Begin").gameObject;
		end = transform.FindChild ("End").gameObject;
		player = GameObject.Find ("Character");
		timeText = GameObject.Find ("TimeText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (begin.GetComponent<BoxCollider2D> ().IsTouching (player.GetComponent<BoxCollider2D> ())) {
			isTimeStarted = true;
		}
		if (end.GetComponent<BoxCollider2D> ().IsTouching (player.GetComponent<BoxCollider2D> ())) {
			isTimeStarted = false;
		}

		if (isTimeStarted) {
			time += Time.deltaTime;
			timeText.text = baseTimeText + time.ToString ("F2");
		}
	}

	public void ResetTimer()
	{
		isTimeStarted = false;
		time = 0;
		timeText.text = baseTimeText + time.ToString ("F2");
	}
}
