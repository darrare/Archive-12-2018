using UnityEngine;
using System.Collections;

public class ButtonEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendName()
	{
		GameObject.Find ("UI elements").GetComponent<QuestLog> ().DisplayQuestInfo (transform.name);
	}

	public void ChangeQuestTracker()
	{
		GameObject.Find ("UI elements").GetComponent<QuestTracker> ().FindQuest (transform.name);
	}
}
