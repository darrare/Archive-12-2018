using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelOneController : MonoBehaviour {
	Canvas pauseMenu;
	Canvas gainedBand;

	// Use this for initialization
	void Start () {
		pauseMenu = GameObject.Find ("PauseMenu").GetComponent<Canvas> ();
		gainedBand = GameObject.Find ("BandMenu").GetComponent<Canvas> ();
		pauseMenu.enabled = false;
		gainedBand.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			ToggleMenu ();
		}
	}

	void ToggleMenu()
	{
		pauseMenu.enabled = !pauseMenu.enabled;
		if (pauseMenu.enabled) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	public void GainedBand()
	{
		gainedBand.enabled = true;
		Invoke ("BandDelay", 6);
	}

	public void BandDelay()
	{
		gainedBand.enabled = false;
	}

	public void LevelCompleted()
	{
		GAMECONSTANTS.enemiesDestroyed = GameObject.Find ("Character").GetComponent<PlatformCharacterController2D> ().EnemiesDestroyed ();
		GAMECONSTANTS.bandsFound = GameObject.Find ("Character").GetComponent<PlatformCharacterController2D> ().GetBandsFound ();
		GAMECONSTANTS.timeTaken = GameObject.Find ("Timer").GetComponent<Timer> ().GetTime ();
		Application.LoadLevel ("LevelCompletion");
	}
}
