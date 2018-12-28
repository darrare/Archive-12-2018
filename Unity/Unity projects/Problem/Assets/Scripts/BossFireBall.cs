using UnityEngine;
using System.Collections;

public class BossFireBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			collider.GetComponent<PlayerController> ().KillPlayer ();
		} else if (collider.tag == "BossFireBallStopper") {
			Instantiate (Resources.Load ("Explosion"), transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
