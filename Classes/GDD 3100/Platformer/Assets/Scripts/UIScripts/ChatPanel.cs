using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour {
	GameObject chatPanel;
	Image image;
	Text dialog;
	bool isActive = false;
	Quest quest = null;

	// Use this for initialization
	void Start () {
		chatPanel = GameObject.Find ("ChatPanel");
		image = GameObject.Find ("ChatImage").GetComponent<Image> ();
		dialog = GameObject.Find ("ChatDialog").GetComponent<Text> ();
		chatPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (Input.GetButtonDown ("Cancel"))
			{
				CloseChatPanel();
			}
			else if (Input.GetButtonDown ("Jump"))
			{
				CloseChatPanel();
				PlayerAccepts();
			}
		}
	}

	public void OpenChatPanel(Sprite image, string dialog, Quest quest)
	{
		isActive = true;
		chatPanel.SetActive (true);
		this.image.sprite = image;
		this.dialog.text = dialog;
		this.quest = quest;
	}
	public void OpenChatPanel(Sprite image, string dialog)
	{
		isActive = true;
		chatPanel.SetActive (true);
		this.image.sprite = image;
		this.dialog.text = dialog;
	}

	public void CloseChatPanel()
	{
		isActive = false;
		chatPanel.SetActive (false);
	}

	public void PlayerAccepts()
	{
		CloseChatPanel();
		if (quest != null) {
			Quest newQuest = quest;
			newQuest.Activate ();
		}
		quest = null;
	}
}
