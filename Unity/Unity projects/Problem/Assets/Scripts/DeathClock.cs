using UnityEngine;
using System.Collections;

public class DeathClock : MonoBehaviour {

	public float deathTime = 1f;
	float timeAlive = 0f;

	// Update is called once per frame
	void Update () {
		timeAlive += Time.deltaTime;
		if (timeAlive >= deathTime) {
			Destroy (this.gameObject);
		}
	}
}
