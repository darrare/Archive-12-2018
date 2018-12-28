using UnityEngine;
using System.Collections;

public class BossLaser : MonoBehaviour {
	public AudioClip clip;

	// Use this for initialization
	void Start () {
		CONSTANTS.soundEffects.PlayOneShot (clip);
		Invoke ("DeathClock", 10);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			collider.GetComponent<PlayerController> ().KillPlayer ();
			Destroy (gameObject);
		}
	}

	void DeathClock()
	{
		Destroy (gameObject);
	}
}
