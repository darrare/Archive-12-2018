using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	string owner;
	public AudioClip clip;

	// Use this for initialization
	void Start () {
		CONSTANTS.soundEffects.PlayOneShot (clip);
		Invoke ("DeathClock", 3.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (owner == "Player" && collider.tag == "Enemy") {
			collider.GetComponent<EnemyController> ().KillEnemy ();
			Destroy (gameObject);
		} else if (owner == "Enemy" && collider.tag == "Player") {
			collider.GetComponent<PlayerController> ().KillPlayer ();
			Destroy (gameObject);
		} else if (collider.tag != "FishPlatform"){
			Destroy (gameObject);
		}
	}

	public void SetOwner(string owner)
	{
		this.owner = owner;
	}

	void DeathClock()
	{
		Destroy (gameObject);
	}
}
