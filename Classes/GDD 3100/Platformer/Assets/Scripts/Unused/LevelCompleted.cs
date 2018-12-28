using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelCompleted : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("Text").GetComponent<Text> ().text =
			"Enemies destroyed: " + GAMECONSTANTS.enemiesDestroyed +
			"\nBands found: " + GAMECONSTANTS.bandsFound +
			"\nTime Taken: " + GAMECONSTANTS.timeTaken.ToString ("F2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReturnToMainMenuClicked()
	{
		Application.LoadLevel ("MainMenu");
	}
}
