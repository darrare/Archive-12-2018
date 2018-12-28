using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("SpawnEnemy", 0, 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SpawnEnemy()
	{
		int random = Random.Range (0, 2);
		if (random == 0) {
			GameObject newEnemy = Instantiate (Resources.Load ("BadMage")) as GameObject;
			newEnemy.transform.parent = transform;
			newEnemy.transform.position = transform.position;
		} else if (random == 1) {
			GameObject newEnemy = Instantiate (Resources.Load ("BadKnight")) as GameObject;
			newEnemy.transform.parent = transform;
			newEnemy.transform.position = transform.position;
		}

	}
}
