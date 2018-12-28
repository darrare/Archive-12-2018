using UnityEngine;
using System.Collections;

public class RoosterSpawner : MonoBehaviour {
	bool isActive = false;
	GameObject newRooster;

	void OnEnable()
	{
		Tutorial2Control.StartSpawn += ActivateSpawner;
	}

	void OnDisable()
	{
		Tutorial2Control.StartSpawn -= ActivateSpawner;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive && newRooster == null) {
			isActive = false;
			Invoke ("SpawnNewRooster", 2f);
		}
	}

	void ActivateSpawner()
	{
		SpawnNewRooster ();
	}

	void SpawnNewRooster()
	{
		isActive = true;
		newRooster = Instantiate (Resources.Load ("Rooster")) as GameObject;
		newRooster.transform.position = transform.position;
	}
}
