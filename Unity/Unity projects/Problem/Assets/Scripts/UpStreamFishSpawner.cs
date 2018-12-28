using UnityEngine;
using System.Collections;

public class UpStreamFishSpawner : MonoBehaviour {
	public float velocity;
	public float jumpVelocity;
	public float jumpVelocityDifferenceMax = .3f;
	public float yPositionToJumpAt;
	public float xPositionToStopAt;
	public float minTimerSpawn;
	public float maxTimerSpawn;
	float nextTimerSpawn;
	float timer = 0;

	// Use this for initialization
	void Start () {
		nextTimerSpawn = Random.Range (minTimerSpawn, maxTimerSpawn);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= nextTimerSpawn) {
			GameObject newFish = Instantiate (Resources.Load ("UpStreamFish"), transform.position, Quaternion.identity) as GameObject;
			newFish.GetComponent<UpStreamFish> ().SetStats (velocity, jumpVelocity + Random.Range(-jumpVelocityDifferenceMax, jumpVelocityDifferenceMax), yPositionToJumpAt, xPositionToStopAt);
			nextTimerSpawn = Random.Range (minTimerSpawn, maxTimerSpawn);
			timer = 0;
		}
	}
}
