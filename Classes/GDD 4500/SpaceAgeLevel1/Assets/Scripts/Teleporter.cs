using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {
	bool canUse = false;
	GameObject partner;
	float siblingIndex;

	GameObject player;

	// Use this for initialization
	void Start () {
		siblingIndex = transform.GetSiblingIndex ();
		if (siblingIndex == 0) {
			partner = transform.parent.GetChild (1).gameObject;
		}else if (siblingIndex == 1) {
			partner = transform.parent.GetChild (0).gameObject;
		}
		player = GameObject.Find ("Character");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (canUse && Input.GetButton ("Action")) {
			canUse = false;
			player.transform.position = partner.transform.position;
		} 
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			if (!Input.GetButton ("Action"))
			{
				canUse = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			if (!Input.GetButton ("Action"))
			{
				canUse = true;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			canUse = false;
		}
	}
}
