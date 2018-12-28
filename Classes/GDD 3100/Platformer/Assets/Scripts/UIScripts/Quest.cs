using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;
using System.Reflection;

[Serializable ()]
public class Quest{
	string title, description;
	List<string> milestones = new List<string> ();
	List<bool> milestoneCompletion = new List<bool>();
	bool activate = false;

	public Quest(string title, string description, List<string> milestones, bool activate)
	{
		this.title = title;
		this.description = description;
		this.milestones = milestones;
		this.activate = activate;
		foreach (string milestone in milestones) {
			milestoneCompletion.Add (false);
		}
		if (activate) {
			GAMECONSTANTS.activeQuests.Add (this);
			GameObject.Find ("UI elements").GetComponent<QuestTracker> ().NewQuest (this);
		}
	}


	public void Activate()
	{
		activate = true;
		GAMECONSTANTS.activeQuests.Add (this);
		GameObject.Find ("UI elements").GetComponent<QuestTracker> ().NewQuest (this);
	}

	public bool IsActive
	{
		get { return activate; }
		set { activate = value; }
	}

	public string Title
	{
		get { return title;}
		set { title = value;}
	}

	public string Description
	{
		get { return description;}
		set { description = value;}
	}

	public bool IsCompleted
	{
		get
		{
			bool temp = true;
			foreach (bool check in milestoneCompletion)
			{
				if (check == false)
				{
					temp = false;
				}
			}
			return temp;
		}
	}

	public List<bool> GetMilestonesBool
	{
		get { return milestoneCompletion;}
	}

	public List<string> GetMilestonesStrings
	{
		get { return milestones; }
	}

	public string GetMilestoneString(int index)
	{
		return milestones [index];
	}

	public void CompleteTask(int index)
	{
		milestoneCompletion [index] = true;
	}

	public bool IsTaskComplete(int index)
	{
		return milestoneCompletion [index];
	}

	public void ResetObjectives()
	{
		for(int i = 0; i < milestoneCompletion.Count; i++) {
			milestoneCompletion[i] = false;
		}
	}
}
