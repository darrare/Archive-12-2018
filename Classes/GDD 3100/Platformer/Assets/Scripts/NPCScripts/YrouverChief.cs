using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YrouverChief : MonoBehaviour {
	float[] cameraLimits = new float[4];
	string chatDialog = "Welcome to the fabulous city of Yrouver! \n Or, I should say the once previously fabulous" +
		"city of Yrouver! As you may have noticed, our beautiful city is deteriorating, and its all because of the hoodlum " +
		"that stole our oxy clean... Would you mind recovering it for us?";
	string chatDialogPost = "If you help us get our oxy clean back, I'll give you another fun task.. Doing chores for" +
		"us is fun right?";
	string chatDialogPost2 = "Thanks for bringing me the oxy clean! Now I'm going to make the city cleaner!";
	static Quest quest = new Quest("Get the oxy clean", "The hoodlum that stole the oxy clean likely headed west out of town." +
	    " Go find him, and bring back the oxy clean.. But be careful, we don't know how this hoodlum will react.",
	    new List<string> {"Get the oxy clean"}, false);
	
	
	// Use this for initialization
	void Start () {
		cameraLimits [0] = -7.09f;
		cameraLimits [1] = 7.13f;
		cameraLimits [2] = -.86f;
		cameraLimits [3] = 1.63f;
		GameObject.Find ("Main Camera").GetComponent<PlatformCameraControl> ().SetCameraLimits (cameraLimits);
	}

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
			else if (quest.IsCompleted && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialogPost2);
				OxyClean.CompleteMileStone(0);
			}
		}
	}

	public static void CompleteMileStone(int index)
	{
		quest.CompleteTask (index);
	}
}
