using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			collider.gameObject.GetComponent<PlayerController> ().KillPlayer ();
		}
	}
}
