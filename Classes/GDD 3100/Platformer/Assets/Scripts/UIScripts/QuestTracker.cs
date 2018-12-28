using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestTracker : MonoBehaviour {
	Quest quest;
	Text title;
	GameObject[] toggles = new GameObject[3];
	GameObject tracker;
	bool isActive = false;

	// Use this for initialization
	void Awake () {
		title = GameObject.Find ("TrackerTitle").GetComponent<Text> ();
		toggles [0] = GameObject.Find ("Tracker0");
		toggles [1] = GameObject.Find ("Tracker1");
		toggles [2] = GameObject.Find ("Tracker2");
		tracker = GameObject.Find ("QuestTracker");
		tracker.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			for (int i = 0; i < quest.GetMilestonesBool.Count; i++) {
				if (quest.IsTaskComplete(i))
				{
					toggles[i].GetComponent<Toggle>().isOn = true;
				}
				else
				{
					toggles[i].GetComponent<Toggle>().isOn = false;
				}
			}
		}
	}

	public void NewQuest(Quest quest)
	{
		tracker.SetActive (true);
		isActive = true;
		this.quest = quest;
		title.text = quest.Title;
		for (int i = 0; i < 3; i++) {
			if (quest.GetMilestonesStrings.Count > i)
			{
				toggles[i].SetActive (true);
				toggles[i].transform.GetChild (1).GetComponent<Text>().text = quest.GetMilestoneString(i);
			}
			else
			{
				toggles[i].SetActive (false);
			}
		}
	}

	public void FindQuest(string name)
	{
		foreach (Quest quest in GAMECONSTANTS.activeQuests) {
			if (quest.Title == name)
			{
				NewQuest (quest);
			}
		}
	}
}
