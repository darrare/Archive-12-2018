using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AllySpawner : MonoBehaviour {
	Stats stats;
	GameObject allyMessage;
	bool delayActive = false;

	// Use this for initialization
	void Start () {
		stats = GameObject.Find ("Canvas").GetComponent<Stats> ();
		allyMessage = GameObject.Find ("RawImage (6)");
		allyMessage.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!delayActive) {
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				if (stats.GetGold > 75) {
					SpawnMage ();
					stats.ChangeGold (-75);
					delayActive = true;
					allyMessage.SetActive (true);
					Invoke ("SecondDelay", 1);
				}
			}
			if (Input.GetKeyDown (KeyCode.Alpha5)) {
				if (stats.GetGold > 100) {
					SpawnKnight ();
					stats.ChangeGold (-100);
					delayActive = true;
					allyMessage.SetActive (true);
					Invoke ("SecondDelay", 1);
				}
			}
		}
	}
	
	void SpawnMage()
	{
		GameObject newEnemy = Instantiate (Resources.Load ("GoodMage")) as GameObject;
		newEnemy.transform.parent = transform;
		newEnemy.transform.position = transform.position;

	}

	void SpawnKnight()
	{
		GameObject newEnemy = Instantiate (Resources.Load ("GoodKnight")) as GameObject;
		newEnemy.transform.parent = transform;
		newEnemy.transform.position = transform.position;
	}

	void SecondDelay()
	{
		allyMessage.SetActive (false);
		delayActive = false;
	}
}
