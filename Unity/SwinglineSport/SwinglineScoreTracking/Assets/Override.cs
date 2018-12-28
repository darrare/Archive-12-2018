using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Override : MonoBehaviour {
	private Button[] buttons = new Button[7];
	private GameObject[] arrows = new GameObject[3];
	private bool goingDown = true;
	private Image overrideColor;
	private GameObject selectedButton;
	private Control control;

	//Initializes every button and disables then by default.
	void Awake () {
		buttons [0] = GameObject.Find ("HomeScore").GetComponent<Button> ();
		buttons [1] = GameObject.Find ("AwayScore").GetComponent<Button> ();
		buttons [2] = GameObject.Find ("Outs").GetComponent<Button> ();
		buttons [3] = GameObject.Find ("Inning").GetComponent<Button> ();
		buttons [4] = GameObject.Find ("Diamond").GetComponent<Button> ();
		buttons [5] = GameObject.Find ("HomeTeam").transform.GetChild(0).GetComponent<Button> ();
		buttons [6] = GameObject.Find ("AwayTeam").transform.GetChild(0).GetComponent<Button> ();

		arrows [0] = GameObject.Find ("UpButton");
		arrows [1] = GameObject.Find ("DownButton");
		arrows [2] = GameObject.Find ("Reset");

		overrideColor = GetComponent<Image> ();
		control = GameObject.Find ("Canvas").GetComponent<Control> ();

		foreach (Button button in buttons) {
			button.enabled = false;
		}

		foreach (GameObject arrow in arrows) {
			arrow.SetActive(false);
		}
	}


	void ColorChanger()
	{
		if (overrideColor.color.g > .39f && goingDown) {
			overrideColor.color = new Color(overrideColor.color.r, overrideColor.color.g - .1f, overrideColor.color.b);
			goingDown = true;
		} else if (overrideColor.color.g <= .98f && !goingDown){
			overrideColor.color = new Color(overrideColor.color.r, overrideColor.color.g + .1f, overrideColor.color.b);
			goingDown = false;
		} else {
			goingDown = !goingDown;
		}
	}


	public void OverrideClick()
	{
		if (buttons [0].IsActive ()) {
			foreach (Button button in buttons) {
				button.enabled = false;
			}
			foreach (GameObject arrow in arrows) {
				arrow.SetActive(false);
			}
			CancelInvoke ("ColorChanger");
			overrideColor.color = new Color(.87f, .98f, .22f);
			GameObject.Find("HomeTeam").GetComponent<InputField>().enabled = true;
			GameObject.Find("AwayTeam").GetComponent<InputField>().enabled = true;
		} else {
			foreach (Button button in buttons)
			{
				button.enabled = true;
			}
			arrows[2].SetActive (true);
			InvokeRepeating ("ColorChanger", 0, .1f);
			GameObject.Find("HomeTeam").GetComponent<InputField>().enabled = false;
			GameObject.Find("AwayTeam").GetComponent<InputField>().enabled = false;
		}
	}

	public void ButtonClick(GameObject buttonObject)
	{
		//if one of the 4 changing buttons are pressed
		if (buttonObject.name == buttons [0].name || buttonObject.name == buttons [1].name ||
			buttonObject.name == buttons [2].name || buttonObject.name == buttons [3].name) {
			foreach (GameObject arrow in arrows) {
				arrow.SetActive (true);
			}
			selectedButton = buttonObject;

			int pixelXOffset = Screen.width / 16;
			int pixelYOffset = Screen.height / 32;
			arrows [0].transform.position = new Vector3 (buttonObject.transform.position.x + pixelXOffset, buttonObject.transform.position.y - pixelYOffset);
			arrows [1].transform.position = new Vector3 (buttonObject.transform.position.x - pixelXOffset, buttonObject.transform.position.y - pixelYOffset);
		} 
		//if one of the three bases are clicked.
		else if (buttonObject.name == buttons [4].name) {
			control.DiamondControl ();
		} 
		//if the team name button is pressed.
		else if (buttonObject.name == buttons [5].transform.parent.name) {
			control.HomeBatterIndexIncrement();
		} else if (buttonObject.name == buttons [6].transform.parent.name) {
			control.AwayBatterIndexIncrement();
		}
	}

	public void ArrowControlUp()
	{
		if (selectedButton.name == "HomeScore") {
			control.HomeTeamScore += 1;
		} else if (selectedButton.name == "AwayScore") {
			control.AwayTeamScore += 1;
		} else if (selectedButton.name == "Outs") {
			if (control.Outs < 2)
				control.Outs += 1;
		} else if (selectedButton.name == "Inning") {
			control.ChangeInning(1);
		}
	}

	public void ArrowControlDown()
	{
		if (selectedButton.name == "HomeScore") {
			if (control.HomeTeamScore > 0)
				control.HomeTeamScore -= 1;
		} else if (selectedButton.name == "AwayScore") {
			if (control.AwayTeamScore > 0)
				control.AwayTeamScore -= 1;
		}else if (selectedButton.name == "Outs") {
			if (control.Outs > 0)
				control.Outs -= 1;
		}else if (selectedButton.name == "Inning") {
			control.ChangeInning(-1);
		}
	}

	public bool IsOverride ()
	{
		return buttons [0].enabled;
	}

//	public void ChangeHomeName()
//	{
//		GameObject.Find ("HomeTeam").transform.FindChild("Text").GetComponent<Text>().text = GameObject.Find ("HomeTeam").GetComponent<InputField>().text;
//	}
//
//	public void ChangeAwayName()
//	{
//		GameObject.Find ("AwayTeam").transform.FindChild("Text").GetComponent<Text>().text = GameObject.Find ("AwayTeam").GetComponent<InputField>().text;
//	}
}
