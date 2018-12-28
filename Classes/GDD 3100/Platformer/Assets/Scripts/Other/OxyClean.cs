using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OxyClean : MonoBehaviour {
	string chatDialog = "The hoodlum got scared and ran away... \n \n He did, however, drop the oxy clean" +
		" so we can bring it back to the chief!";

	static Quest quest = new Quest("Deliver the goods", "The hoodlum was a cowered and ran at first sight... " +
	    "Now that you have the oxy clean, bring it back to the chief in Yrouver.",
	    new List<string> {"Speak to the chief"}, false);

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 300));
		GetComponent<Rigidbody2D> ().AddTorque (10);
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{
			quest.Activate ();
			YrouverChief.CompleteMileStone(0);
			GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialog);
			Destroy (gameObject);
		}
	}

	public static void CompleteMileStone(int index)
	{
		quest.CompleteTask (index);
	}
}
