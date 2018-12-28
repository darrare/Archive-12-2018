using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour {
	bool activePage = true;
	bool questLogOpen = false;
	float questItemOffset = 25;
	GameObject questDescription;
	GameObject questPane;

	// Use this for initialization
	void Start () {
		questDescription = GameObject.Find ("QuestDescription");
		questPane = GameObject.Find ("QuestPane");
		questPane.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!questLogOpen && Input.GetKeyDown (KeyCode.L)) {
			questPane.SetActive (true);
			questLogOpen = true;
			DisplayQuests ();
			//questPane.GetComponent<RectTransform>().localPosition = new Vector2(questPane.GetComponent<RectTransform>().localPosition.x - 400, questPane.GetComponent<RectTransform>().localPosition.y);
		} else if (questLogOpen && Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.L)) {
			questLogOpen = false;
			questPane.SetActive (false);
			//questPane.GetComponent<RectTransform>().localPosition = new Vector2(questPane.GetComponent<RectTransform>().localPosition.x + 400, questPane.GetComponent<RectTransform>().localPosition.y);
		}
		for (int i = GAMECONSTANTS.activeQuests.Count - 1; i >= 0; i--) {
			if (GAMECONSTANTS.activeQuests[i].IsCompleted)
			{
				//create fireworks or some shit here
				GameObject fireWorks = Instantiate (Resources.Load ("QuestComplete")) as GameObject;
				fireWorks.transform.SetParent (GameObject.Find ("Character").transform);
				fireWorks.transform.localPosition = Vector2.zero;

				GAMECONSTANTS.completedQuests.Add (GAMECONSTANTS.activeQuests[i]);
				GAMECONSTANTS.activeQuests.Remove (GAMECONSTANTS.activeQuests[i]);
	
				if (questLogOpen)
				{
					DisplayQuests ();
				}
			}
		}
	}

	void DisplayQuests()
	{
		for (int i = GameObject.Find ("QuestButtonHolder").transform.childCount - 1; i >= 0; i--) {
			Destroy(GameObject.Find ("QuestButtonHolder").transform.GetChild (i).gameObject);
		}
		if (activePage) {
			for (int i = 1; i < GAMECONSTANTS.activeQuests.Count + 1; i++)
			{
				GameObject newItem = Instantiate (Resources.Load ("QuestItem")) as GameObject;
				newItem.transform.SetParent(GameObject.Find ("QuestButtonHolder").transform);
				newItem.name = GAMECONSTANTS.activeQuests[i - 1].Title;
				newItem.transform.GetChild(0).GetComponent<Text>().text = GAMECONSTANTS.activeQuests[i - 1].Title;
				newItem.GetComponent<RectTransform>().localPosition = new Vector2(0, -questItemOffset * i);
			}
		} else {
			for (int i = 1; i < GAMECONSTANTS.completedQuests.Count + 1; i++)
			{
				GameObject newItem = Instantiate (Resources.Load ("QuestItem")) as GameObject;
				Destroy (newItem.transform.Find ("Button").gameObject);
				newItem.transform.SetParent(GameObject.Find ("QuestButtonHolder").transform);
				newItem.name = GAMECONSTANTS.completedQuests[i - 1].Title;
				newItem.transform.GetChild(0).GetComponent<Text>().text = GAMECONSTANTS.completedQuests[i - 1].Title;
				newItem.GetComponent<RectTransform>().localPosition = new Vector2(0, -questItemOffset * i);
			}
		}
	}

	public void DisplayActive()
	{
		activePage = true;
		DisplayQuests ();
	}

	public void DisplayCompleted()
	{
		activePage = false;
		DisplayQuests ();
	}

	public void DisplayQuestInfo(string name)
	{
		if (activePage) {
			foreach (Quest quest in GAMECONSTANTS.activeQuests) {
				if (quest.Title == name) {
					questDescription.transform.GetChild (0).GetComponent<Text> ().text = quest.Title;
					questDescription.transform.GetChild (1).GetComponent<Text> ().text = quest.Description;
				}
			}
		} else {
			foreach (Quest quest in GAMECONSTANTS.completedQuests) {
				if (quest.Title == name)
				{
					questDescription.transform.GetChild (0).GetComponent<Text>().text = quest.Title;
					questDescription.transform.GetChild (1).GetComponent<Text>().text = quest.Description;
				}
			}
		}

	}
}
