using UnityEngine;
using System.Collections;

public class PendantOfTheAdventurer : MonoBehaviour {
	string chatDialog = "Upon killing the harpie queen you received the pendant of the adventurer! " +
		"This pendant is a symbol of great strength, and gives you the power and opportunity to step" +
			" out and explore the world!";
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 300));
		GetComponent<Rigidbody2D> ().AddTorque (10);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{
			Tutorial3Control.CompleteMileStone (1);
			GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialog);
			Destroy (gameObject);
		}
	}
}
