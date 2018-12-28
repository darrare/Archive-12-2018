using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HiddenGrottoJohnny : MonoBehaviour {
	float[] cameraLimits = new float[4];
	string chatDialog = "I'm Johnny! \n \n What's this you say about my mom looking for me? \n \n What's that? she doesn't " +
		"know about the hidden grotto behind that old building? This is where I always play. It's peaceful with no " +
		"warewolves or hoodlums. Will you tell my mom I'll be playing back here a little longer?";
	string chatDialogPost = "Yar har, tee hee, a pirates life for me! \n \n " +
		"Hey... Why are you watching me play? \n \n AHHH! STRANGER DANGER, STRANGER DANGER!";
	static Quest quest = new Quest("Report to mom", "False alarm, Johnny was just playing in the grotto his mother didn't " +
		"know about. Lets go tell his mom that everything is OK.",
        new List<string> {"Talk to Johnny's mom"}, false);
	
	
	// Use this for initialization
	void Start () {
		cameraLimits [0] = 0f;
		cameraLimits [1] = 0f;
		cameraLimits [2] = 0f;
		cameraLimits [3] = 0f;
		GameObject.Find ("Main Camera").GetComponent<PlatformCameraControl> ().SetCameraLimits (cameraLimits);
	}
	
	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			if (!quest.IsActive && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialog, quest);
				YrouverMother.CompleteMileStone (0, 0);
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