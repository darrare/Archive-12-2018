using UnityEngine;
using System.Collections;

public class FullBeam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("DeathClock", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			collider.GetComponent<PlayerController> ().KillPlayer ();
		} 
	}

	void DeathClock()
	{
		Destroy(gameObject);
	}
}
