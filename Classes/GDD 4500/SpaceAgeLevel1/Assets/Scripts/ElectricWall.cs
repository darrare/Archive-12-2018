using UnityEngine;
using System.Collections;

public class ElectricWall : MonoBehaviour {
	Character player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Character").GetComponent<Character>();	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			player.DamagePlayer (25);
		}
	}
}
