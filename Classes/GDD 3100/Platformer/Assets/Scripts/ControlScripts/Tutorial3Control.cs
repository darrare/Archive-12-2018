using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial3Control : MonoBehaviour {
	float[] cameraLimits = new float[4];
	string chatDialog = "Now that you have learned the basics, lets test your combat against a real challenge! " +
		"We have trapped the harpie queen in these catacombs. Go see if you can take her out in 1-on-1 combat.";
	string chatDialogPost = "What are you waiting for? Go slay the harpie queen and reap your rewards. \n \n " +
		"Try to avoid her the best you can, she will lunge at you and throw razor sharp feathers!";
	static Quest quest = new Quest("Kill the harpie queen", "The harpie queen has been captured in the catacombs and " +
		"will be your final challenge before you are ready to explore the world!", 
	     new List<string> {"Kill the harpie", "Loot rewards", "Exit catacombs"}, false);


	// Use this for initialization
	void Start () {
		cameraLimits [0] = -12.99f;
		cameraLimits [1] = 12.99f;
		cameraLimits [2] = -3.8f;
		cameraLimits [3] = -3.8f;
		GameObject.Find ("Main Camera").GetComponent<PlatformCameraControl> ().SetCameraLimits (cameraLimits);
	}

	void Update()
	{
		if (GameObject.Find ("Character").transform.position.x > 14) {
			GAMECONSTANTS.playerMapPosition = new Vector2(-.69f, -1.25f);
			Tutorial3Control.CompleteMileStone (2);
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			if (!quest.IsActive && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialog, quest);
			}
			else if (quest.IsActive && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialogPost);
			}
		}
	}

	public static void CompleteMileStone(int index)
	{
		quest.CompleteTask (index);
	}
}
