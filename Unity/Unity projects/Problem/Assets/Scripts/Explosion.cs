using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public AudioClip[]clip;
	float timer = 0;
	// Use this for initialization
	void Start () {
		CONSTANTS.soundEffects.PlayOneShot (clip[Random.Range(0, clip.Length)]);
		transform.eulerAngles = new Vector3 (0, 0, Random.Range (0, 360));
		Invoke ("DeathClock", .75f);
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer > .1f) {
			GetComponent<CircleCollider2D> ().enabled = false;
		}
	}

	void DeathClock()
	{
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			collider.GetComponent<PlayerController> ().KillPlayer ();
		}
	}
}
