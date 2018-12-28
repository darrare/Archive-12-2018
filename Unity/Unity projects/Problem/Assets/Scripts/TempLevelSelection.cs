using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempLevelSelection : MonoBehaviour {


	public InputField levelInput;

	string levelString;

	public void ChangeString()
	{
		levelString = levelInput.text;
	}


	public void LoadLevel()
	{
		SceneManager.LoadScene (levelString);
	}
}
