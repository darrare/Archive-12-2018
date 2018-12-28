using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {
	public Text[] textArray;
	string levelName;
	string baseTimeMessage = "BestTime: ";

	// Use this for initialization
	void Start () {
		CONSTANTS.levelController.Player.transform.position = CONSTANTS.levelSelectSpawnPosition;
		for (int i = 0; i < textArray.Length; i++) {
			//removes the question mark at the end of the text.
			levelName = textArray [i].text;
			levelName = levelName.Remove (levelName.Length - 1);
			if (CONSTANTS.levelInfo.ContainsKey (levelName)) {
				textArray [i].transform.GetChild (0).GetComponent<Text> ().text = baseTimeMessage + CONSTANTS.levelInfo [levelName].GetTimeInTextFormat();
			} 
		}
	}
}
