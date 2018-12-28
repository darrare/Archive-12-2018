using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
	public RectTransform panel;
	public RectTransform[] menuOptions;
	public enum MenuState {PLAY, SETTINGS, LEVELBUILDER, QUIT};
	MenuState menuState = MenuState.PLAY;
	float input = 0;
	bool readyToMove = true;

	// Use this for initialization
	void Start () {
		if (InputManager.keys.Count == 0) {
			InputManager.Initialize ();
		}

		if (SaveLoad.CheckIfFileExists ()) {
			Debug.Log ("Loading");
			SaveLoad.Load ();
		} else {
			Debug.Log ("Saving");
			SaveLoad.Save ();
		}

		if (CONSTANTS.controllerLayout == SettingsScreen.ControllerLayout.RETRO) {
			InputManager.SwitchControlScheme (InputManager.ControllerLayout.RETRO);
		}
		else if (CONSTANTS.controllerLayout == SettingsScreen.ControllerLayout.NEWAGE) {
			InputManager.SwitchControlScheme (InputManager.ControllerLayout.NEWAGE);
		}
		else if (CONSTANTS.controllerLayout == SettingsScreen.ControllerLayout.TRIGGERHAPPY) {
			InputManager.SwitchControlScheme (InputManager.ControllerLayout.TRIGGERHAPPY);
		}
	}
	
	// Update is called once per frame
	void Update () {
		input = InputManager.GetAxisRaw ("Vertical");
		if (input == 0) {
			readyToMove = true;
		} else if (input == 1 && readyToMove) {
			readyToMove = false;
			if (menuState != MenuState.PLAY) {
				menuState--;
				ChangeState ();
				GetComponent<AudioSource> ().Play ();
			}
		} else if (input == -1 && readyToMove) {
			readyToMove = false;
			if (menuState != MenuState.QUIT) {
				menuState++;
				ChangeState ();
				GetComponent<AudioSource> ().Play ();
			}
		}

		if (InputManager.GetButtonDown ("Jump") || InputManager.GetButtonDown("Start")) {
			ChangeScene ();
		}
	}

	public void ChangeState(int value)
	{
		menuState = (MenuState)value;
		ChangeState ();
	}

	void ChangeState()
	{
		if (menuState == MenuState.PLAY) {
			panel.anchoredPosition = menuOptions [0].anchoredPosition + Vector2.right * 15;
		} else if (menuState == MenuState.SETTINGS) {
			panel.anchoredPosition = menuOptions [1].anchoredPosition + Vector2.right * 15;
		} else if (menuState == MenuState.LEVELBUILDER) {
			panel.anchoredPosition = menuOptions [2].anchoredPosition + Vector2.right * 15;
		} else if (menuState == MenuState.QUIT) {
			panel.anchoredPosition = menuOptions [3].anchoredPosition + Vector2.right * 15;
		}
	}

	void ChangeScene(string levelName)
	{
		SceneManager.LoadScene (levelName);
	}

	public void ChangeScene()
	{
		if (menuState == MenuState.PLAY) {
			SceneManager.LoadScene ("LevelSelect");
		} else if (menuState == MenuState.SETTINGS) {
			SceneManager.LoadScene ("Settings");
		} else if (menuState == MenuState.LEVELBUILDER) {
			SceneManager.LoadScene ("LevelBuilder");
		} else if (menuState == MenuState.QUIT) {
			Application.Quit ();
		}
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
