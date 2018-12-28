using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	/*INDEX INSTRUCTIONS
	0 - Movement
	1 - Main ability (mouse 1)
	2 - Secondary ability (mouse 2)
	3 - Special ability (E)
	4 - Boost ability (R)
	5 - Destroy 1 enemy
	6 - Destroy Castle
	*/
	protected bool[] isCompleted = new bool[7];
	protected ScrollScript scroll;
	protected GameObject button;
	protected enum Stage {Stage0, Stage1, Stage2, Stage3, Stage4, Stage5, Stage6};
	protected Stage stage;
	protected int textIndex = 0;
	protected string[] stringArray;

	// Use this for initialization
	protected virtual void Start () {
		scroll = GameObject.Find ("scroll").GetComponent<ScrollScript> ();
		button = GameObject.Find ("scrollNextButton");
		//HideText ();
		stage = Stage.Stage0;
	}

//	public void NextStage()
//	{
//		isCompleted [(int)stage] = true;
//		stage++;
//	}

//	protected void CompleteStage(int index)
//	{
//		isCompleted[index] = true;
//	}

//	public bool CheckStage(int index)
//	{
//		return isCompleted[index];
//	}

	public int GetStage()
	{
		return (int)stage;
	}

	protected void ChangeText(string text)
	{
		button.SetActive (false);
		scroll.Text = text;
	}

	protected void ChangeText(string[] text)
	{
		stringArray = text;
		scroll.Text = text[0];
		textIndex = 0;
		button.SetActive (true);
	}

	public virtual void ButtonTextChange()
	{
		if (textIndex < stringArray.Length - 2) {
			textIndex++;
			scroll.Text = stringArray [textIndex];
		} else {
			textIndex++;
			ChangeText (stringArray[textIndex]);
		}
	}

	protected void DisplayText()
	{
		scroll.gameObject.SetActive (true);
	}

	protected void HideText()
	{
		scroll.gameObject.SetActive (false);
	}
}
