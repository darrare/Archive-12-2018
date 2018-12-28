using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YrouverMother : MonoBehaviour {
	string chatDialog = "Oh my, oh my! Where is my son Johnny!\n Oh please mister, can you go searching for my son?" +
		" The last I saw him, he was heading into this abandoned building to play.";
	string chatDialogPost = "Have you found my son yet? Oh where or where could he be!";
	static Quest quest = new Quest("Where's Johnny?", "The mothers son is missing and was last seen heading into the" +
		" abandoned building to play. You should go check inside to see if you can find him.",
	    new List<string> {"Find the boy"}, false);


	string chatDialog2 = "Thankyou for finding my boy. Since I like you, I'll make you a deal. If you go kill the werewolf " +
		"in the east, I'll bake you a pie.";
	string chatDialogPost2 = "That dirty old werewolf...";
	static Quest quest2 = new Quest("The big bad wolf", "A werewolf has been terrorizing the town and is currently off in " +
		"the easy. Johnny's mother said she would bake you a pie if you killed that werewolf.",
	    new List<string> {"Kill the werewolf"}, false);
	
	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			if (!quest.IsCompleted && !quest.IsActive && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialog, quest);
			}
			else if (!quest.IsCompleted && quest.IsActive && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialogPost);
			}
			else if (quest.IsCompleted && !quest2.IsActive && Input.GetButton ("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialog2, quest2);
				HiddenGrottoJohnny.CompleteMileStone(0);
			}
			else if (quest.IsCompleted && quest2.IsActive && Input.GetButton ("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialogPost2);
			}
		}
	}
	
	public static void CompleteMileStone(int questIndex, int index)
	{
		if (questIndex == 0) {
			quest.CompleteTask (index);
		} else if (questIndex == 1) {
			quest2.CompleteTask (index);
		}
	}
}