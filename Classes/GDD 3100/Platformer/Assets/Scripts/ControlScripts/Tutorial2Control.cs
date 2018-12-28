using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial2Control : MonoBehaviour {
	public delegate void RoosterSpawner();
	public static event RoosterSpawner StartSpawn;

	float[] cameraLimits = new float[4];
	bool spawnStarted = false;
	static int roosterCount = 0;
	string chatDialog = "Nice work! You figured out how to move around the game world and now " +
		"you have managed to start a conversation with me! Now it's time for the nitty gritty.. " +
		"This map isn't as friendly as the previous, accept this quest and go kill some bad guys! \n" +
		"Attack using the mouse left click or the B button";
	string chatDialogPost = "What are you waiting for? Go kill all the things!";
	static Quest quest = new Quest("Kill all the things!", "Your objective is to learn how to defeat " +
		"enemies, and when to avoid enemies that you aren't yet powerful enough to fight. " +
	    "Attack using the mouse left click or the B button", 
	    new List<string> {"Kill an elephant", "Avoid golems", "Kill 10 roosters"}, false);

	// Use this for initialization
	void Start () {
		cameraLimits [0] = -9f;
		cameraLimits [1] = 8.96f;
		cameraLimits [2] = -2.75f;
		cameraLimits [3] = 3.34f;
		GameObject.Find ("Main Camera").GetComponent<PlatformCameraControl> ().SetCameraLimits (cameraLimits);
	}
	
	// Update is called once per frame
	void Update () {
		if (!spawnStarted && quest.IsActive) {
			spawnStarted = true;
			StartSpawner ();
		}
	}

	void StartSpawner()
	{
		StartSpawn ();
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			if (!quest.IsActive && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialog, quest);
			}
			else if (spawnStarted && Input.GetButton("Action"))
			{
				GameObject.Find ("UI elements").GetComponent<ChatPanel>().OpenChatPanel(GetComponent<SpriteRenderer>().sprite, chatDialogPost);
			}
		}
	}

	public static void CompleteMileStone(int index)
	{
		if (index != 2) {
			quest.CompleteTask (index);
		} else {
			roosterCount++;
			if (roosterCount >= 10)
			{
				quest.CompleteTask (index);
				quest.CompleteTask (index - 1);
			}
		}
	}

	public static void FailQuest()
	{

		roosterCount = 0;
		GAMECONSTANTS.activeQuests.Remove (quest);
		quest.ResetObjectives ();
		quest.IsActive = false;
	}
}
