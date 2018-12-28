﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Rules : MonoBehaviour {
	private Topic[] topics = new Topic[12];
	private bool inSubMenu = false;
	private Vector3 originalPosition;
	private GameObject currentObject;

	void Start () {
		//find each of the main topics.
		topics[0] = new Topic(GameObject.Find ("Equipment"));
		topics[1] = new Topic(GameObject.Find ("General"));
		topics[2] = new Topic(GameObject.Find ("Players"));
		topics[3] = new Topic(GameObject.Find ("Batting"));
		topics[4] = new Topic(GameObject.Find ("Fielding"));
		topics[5] = new Topic(GameObject.Find ("Hits"));
		topics[6] = new Topic(GameObject.Find ("Pitching"));
		topics[7] = new Topic(GameObject.Find ("HomeRuns"));
		topics[8] = new Topic(GameObject.Find ("Outs"));
		topics[9] = new Topic(GameObject.Find ("Fouls"));
		topics[10] = new Topic(GameObject.Find ("LengthOfGame"));
		topics[11] = new Topic(GameObject.Find ("ScoringAndScoreKeeping"));

		//add each of the subtopics.
		topics[0].AddSubTopic (GameObject.Find("Equipment/1"));
		topics[1].AddSubTopic (GameObject.Find("General/1"));
		topics[2].AddSubTopic (GameObject.Find("Players/1"));
		topics[3].AddSubTopic (GameObject.Find("Batting/1"));
		topics[4].AddSubTopic (GameObject.Find("Fielding/1"));
		topics[5].AddSubTopic (GameObject.Find("Hits/1"));
		topics[6].AddSubTopic (GameObject.Find("Pitching/1"));
		topics[7].AddSubTopic (GameObject.Find("HomeRuns/1"));
		topics[8].AddSubTopic (GameObject.Find("Outs/1"));
		topics[9].AddSubTopic (GameObject.Find("Fouls/1"));
		topics[10].AddSubTopic (GameObject.Find("LengthOfGame/1"));
		topics[11].AddSubTopic (GameObject.Find("ScoringAndScoreKeeping/1"));

		//Spread main topics
		Vector3 position = topics [0].GetObject ().GetComponent<RectTransform>().anchoredPosition;

		for (int i = 0; i < topics.Length; i++) {
			topics[i].GetObject ().GetComponent<RectTransform>().anchoredPosition = position;
			position.y = topics[i].GetObject().GetComponent<RectTransform>().anchoredPosition.y - (topics[i].GetObject ().GetComponent<RectTransform>().rect.height * 1f);
		}


		foreach (Topic topic in topics) 
		{
			foreach(GameObject childObject in topic.GetSubTopicList())
			{
				if (childObject.name != "text")
				{
					childObject.SetActive (false);
				}
			}
		}

	}

	public void ButtonClick(GameObject topicObject)
	{
		if (inSubMenu) 
		{
			inSubMenu = false;
			currentObject = topicObject;
			Vector3 position = new Vector3 (topics [0].GetObject ().transform.position.x, 0);
			position.y = Screen.height - Screen.height / 20;
			foreach (Topic topic in topics) 
			{
				if (topic.GetObject ().name == topicObject.name)
				{
					//pushes topic back to its original location
					topic.GetObject ().transform.position = originalPosition;
					foreach(GameObject childObject in topic.GetSubTopicList())
					{
						if (childObject.name != "text")
						{
							childObject.SetActive (false);
						}
					}
				}
				else
				{
					topic.GetObject ().SetActive (true);
				}
			}
		} 
		else 
		{
			inSubMenu = true;
			currentObject = topicObject;
			foreach (Topic topic in topics) 
			{
				if (topic.GetObject ().name == topicObject.name)
				{
					//pushes the selected topic to the top of the screen.
					originalPosition = topic.GetObject().transform.position;
					topic.GetObject ().transform.position = topics[0].GetObject().transform.position;
					foreach(GameObject childObject in topic.GetSubTopicList())
					{
						if (childObject.name != "text")
						{
							childObject.SetActive (true);
						}
					}
				}
				else
				{
					topic.GetObject ().SetActive (false);
				}
			}
		}
	}


	public void CloseRules()
	{
		if (inSubMenu) {
			ButtonClick (currentObject);
		} else {
			if (!CONSTANTS.isStaff)
			{
				SceneManager.LoadScene ("UserScoreTracking");
			}
			else if (CONSTANTS.isStaff)
			{
				SceneManager.LoadScene ("StaffScoreTracking");
			}

		}
			
	}
}

public class Topic {
	private GameObject topicObject;
	private List<GameObject> subTopics = new List<GameObject>();
	
	public Topic(GameObject topicObject)
	{
		this.topicObject = topicObject;
	}
	
	public void AddSubTopic(GameObject subTopic)
	{
		subTopics.Add (subTopic);
	}
	
	public List<GameObject> GetSubTopicList()
	{
		return subTopics;
	}
	
	public GameObject GetObject()
	{
		return topicObject;
	}
}

