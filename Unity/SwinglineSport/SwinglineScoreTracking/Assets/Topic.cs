using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
