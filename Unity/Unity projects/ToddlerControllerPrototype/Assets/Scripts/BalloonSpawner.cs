using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using XInputDotNetPure;

public class BalloonSpawner : MonoBehaviour {
	public Slider slider;
	public float horizontalDistance = 5;
	public float spawnRateMin = .5f;
	public float spawnRateMax = 2f;
	float timer = 0;
	float spawnTimer;
	public Sprite[] sprites = new Sprite[4];
	int random;
	Queue<GameObject>[] balloons = new Queue<GameObject>[4];

	// Use this for initialization
	void Awake () {
		CONSTANTS.balloonSpawner = this.GetComponent<BalloonSpawner> ();
		for (int i = 0; i < balloons.Length; i++) {
			balloons [i] = new Queue<GameObject> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= spawnTimer) {
			timer = 0;
			spawnTimer = Random.Range (spawnRateMin, spawnRateMax);
			GameObject newBalloon = Instantiate (Resources.Load ("Balloon")) as GameObject;
			newBalloon.transform.position = new Vector2 (transform.position.x + Random.Range (-horizontalDistance, horizontalDistance), transform.position.y);
			random = Random.Range (0, CONSTANTS.colors.Length);
			balloons [random].Enqueue (newBalloon);
			newBalloon.GetComponent<Balloon> ().SetBalloon (CONSTANTS.colors [random], sprites [random], random);
		}
		if (Input.GetButtonDown ("X")) {
			balloons [0].Dequeue ().GetComponent<Balloon> ().DestroyBalloon ();
			GamePad.SetVibration (0, 1, 1);
			CancelInvoke ("StopVibration");
			Invoke ("StopVibration", .2f);
		}
		if (Input.GetButtonDown ("B")) {
			balloons [1].Dequeue ().GetComponent<Balloon> ().DestroyBalloon ();
			GamePad.SetVibration (0, 1, 1);
			CancelInvoke ("StopVibration");
			Invoke ("StopVibration", .2f);
		}
		if (Input.GetButtonDown ("A")) {
			balloons [2].Dequeue ().GetComponent<Balloon> ().DestroyBalloon ();
			GamePad.SetVibration (0, 1, 1);
			CancelInvoke ("StopVibration");
			Invoke ("StopVibration", .2f);
		}
		if (Input.GetButtonDown ("Y")) {
			balloons [3].Dequeue ().GetComponent<Balloon> ().DestroyBalloon ();
			GamePad.SetVibration (0, 1, 1);
			CancelInvoke ("StopVibration");
			Invoke ("StopVibration", .2f);
		}
	}

	void StopVibration()
	{
		GamePad.SetVibration (0, 0, 0);
	}

	public void UpdateSpawnRate()
	{
		float value = slider.value;
		spawnRateMin = value;
		spawnRateMax = value + 1f * value;
		Debug.Log (spawnRateMin);
	}

	public void RemoveFromQueue(int index)
	{
		balloons [index].Dequeue ();
	}
}
