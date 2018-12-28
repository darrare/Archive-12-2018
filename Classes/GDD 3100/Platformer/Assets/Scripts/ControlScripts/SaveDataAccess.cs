using UnityEngine;
using System.Collections;

public class SaveDataAccess : MonoBehaviour {
	GameObject savePanel;
	bool isOpen = false;

	void Start()
	{
		savePanel = GameObject.Find ("SaveLoad");
		savePanel.SetActive (false);
	}

	void Update()
	{
		if (!isOpen && Input.GetButtonDown ("Cancel")) {
			savePanel.SetActive (true);
			isOpen = true;
		} else if (isOpen && Input.GetButtonDown ("Cancel")) {
			savePanel.SetActive (false);
			isOpen = false;
		}
	}
	public void OnSaveClick()
	{
		SaveLoad.Save ();
		savePanel.SetActive (false);
		isOpen = false;
	}

	public void OnLoadClick()
	{
		SaveLoad.Load ();
		savePanel.SetActive (false);
		isOpen = false;
	}
}
