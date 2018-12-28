using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SettingsScreen : MonoBehaviour {
	public GameObject generalTab, keybindsTab;
	public GameObject generalDisplay, keybindsDisplay;
	public GameObject retro, newAge, triggerHappy;
	public GameObject controller;

	enum MenuSelection {GENERAL, KEYBINDS};
	MenuSelection menuSelection = MenuSelection.GENERAL;

	public enum ControllerLayout {RETRO, NEWAGE, TRIGGERHAPPY};
	ControllerLayout controllerLayout = ControllerLayout.RETRO;

	Color selected, notSelected;

	bool controllerLayoutNeedsUpdated = true;

	// Use this for initialization
	void Start () {
		selected = generalTab.GetComponent<Image> ().color;
		notSelected = keybindsTab.GetComponent<Image> ().color;
		GeneralPressed ();
		controllerLayout = CONSTANTS.controllerLayout;
	}
	
	// Update is called once per frame
	void Update () {
		//Main Header Tabs
		if (menuSelection == MenuSelection.GENERAL && InputManager.GetButtonDown ("RightTrigger")) {
			menuSelection = MenuSelection.KEYBINDS;
			KeybindsPressed ();
			GetComponent<AudioSource> ().Play ();
		} else if (menuSelection == MenuSelection.KEYBINDS && InputManager.GetButtonDown ("LeftTrigger")) {
			menuSelection = MenuSelection.GENERAL;
			GeneralPressed ();
			GetComponent<AudioSource> ().Play ();
		}


		if (menuSelection == MenuSelection.KEYBINDS && !controllerLayoutNeedsUpdated) {
			if (InputManager.GetAxis ("Horizontal") > 0) {
				if (controllerLayout != ControllerLayout.TRIGGERHAPPY) {
					controllerLayout += 1;
					controllerLayoutNeedsUpdated = true;
					GetComponent<AudioSource> ().Play ();
				}
			} else if (InputManager.GetAxis ("Horizontal") < 0) {
				if (controllerLayout != ControllerLayout.RETRO) {
					controllerLayout -= 1;
					controllerLayoutNeedsUpdated = true;
					GetComponent<AudioSource> ().Play ();
				}
			}
		}
			
		if (controllerLayoutNeedsUpdated) {
			if (InputManager.GetAxis ("Horizontal") == 0) {
				controllerLayoutNeedsUpdated = false;
			}
			if (controllerLayout == ControllerLayout.RETRO) {
				RetroPressed ();
			} else if (controllerLayout == ControllerLayout.NEWAGE) {
				NewAgePressed ();
			} else if (controllerLayout == ControllerLayout.TRIGGERHAPPY) {
				TriggerHappyPressed ();
			}
		}

		if (InputManager.GetButtonDown ("B")) {
			BackToMainMenu ();
		}
	}


	public void BackToMainMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	void ChangeKeybindDescriptions(List<string> values)
	{
		controller.transform.FindChild ("RightStick").GetComponent<Text> ().text = values [0];
		controller.transform.FindChild ("A").GetComponent<Text> ().text = values [1];
		controller.transform.FindChild ("B").GetComponent<Text> ().text = values [2];
		controller.transform.FindChild ("X").GetComponent<Text> ().text = values [3];
		controller.transform.FindChild ("Y").GetComponent<Text> ().text = values [4];
		controller.transform.FindChild ("RightBumper").GetComponent<Text> ().text = values [5];
		controller.transform.FindChild ("RightTrigger").GetComponent<Text> ().text = values [6];
		controller.transform.FindChild ("LeftTrigger").GetComponent<Text> ().text = values [7];
		controller.transform.FindChild ("LeftBumper").GetComponent<Text> ().text = values [8];
		controller.transform.FindChild ("LeftStick").GetComponent<Text> ().text = values [9];
		controller.transform.FindChild ("DPad").GetComponent<Text> ().text = values [10];
	}

	public void GeneralPressed()
	{
		generalTab.GetComponent<Image> ().color = selected;
		keybindsTab.GetComponent<Image> ().color = notSelected;
		generalDisplay.SetActive (true);
		keybindsDisplay.SetActive (false);
	}

	public void KeybindsPressed()
	{
		generalTab.GetComponent<Image> ().color = notSelected;
		keybindsTab.GetComponent<Image> ().color = selected;
		generalDisplay.SetActive (false);
		keybindsDisplay.SetActive (true);
	}

	public void RetroPressed()
	{
		retro.GetComponent<Image> ().color = selected;
		newAge.GetComponent<Image> ().color = notSelected;
		triggerHappy.GetComponent<Image> ().color = notSelected;
		InputManager.SwitchControlScheme (InputManager.ControllerLayout.RETRO);
		ChangeKeybindDescriptions (CONSTANTS.retro);
		CONSTANTS.controllerLayout = ControllerLayout.RETRO;
		SaveLoad.Save ();
	}

	public void NewAgePressed()
	{
		retro.GetComponent<Image> ().color = notSelected;
		newAge.GetComponent<Image> ().color = selected;
		triggerHappy.GetComponent<Image> ().color = notSelected;
		InputManager.SwitchControlScheme (InputManager.ControllerLayout.NEWAGE);
		ChangeKeybindDescriptions (CONSTANTS.newAge);
		CONSTANTS.controllerLayout = ControllerLayout.NEWAGE;
		SaveLoad.Save ();
	}

	public void TriggerHappyPressed()
	{
		retro.GetComponent<Image> ().color = notSelected;
		newAge.GetComponent<Image> ().color = notSelected;
		triggerHappy.GetComponent<Image> ().color = selected;
		InputManager.SwitchControlScheme (InputManager.ControllerLayout.TRIGGERHAPPY);
		ChangeKeybindDescriptions (CONSTANTS.triggerHappy);
		CONSTANTS.controllerLayout = ControllerLayout.TRIGGERHAPPY;
		SaveLoad.Save ();
	}
}
