using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaffClientControl : Control {


	public GameObject winnerPanelButton;


	// Use this for initialization
	protected override void Start () {
		base.Start ();
		if (!CONSTANTS.currentlyInGame) {
			winnerPannel.SetActive (true);
			winnerPanelButton.SetActive (false);
		} else {
			winnerPannel.SetActive (false);
			winnerPanelButton.SetActive (false);
		}
	}

	protected override void ChangeNames ()
	{
		homeTeamText.GetComponent<Text> ().text = CONSTANTS.homeTeamName;
		awayTeamText.GetComponent<Text> ().text = CONSTANTS.awayTeamName;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	protected override void DisplayWinnerMessage()
	{
		ranOnce = true;
		CONSTANTS.currentlyInGame = false;
		winnerPannel.SetActive (true);
		winnerPanelButton.SetActive (true);
		winnerPanelButton.GetComponentInChildren<Text> ().text = "Reset";
		if (homeTeamScore > awayTeamScore)
		{
			winnerPannel.transform.GetChild (0).GetComponent<Text>().text = GameObject.Find ("HomeTeam").transform.FindChild("Text").GetComponent<Text>().text + " wins!" +
				"\nResult has been sent in.";
			CONSTANTS.netManager.SendResults ("Home");
		}
		else if (homeTeamScore < awayTeamScore)
		{
			winnerPannel.transform.GetChild (0).GetComponent<Text>().text = GameObject.Find ("AwayTeam").transform.FindChild("Text").GetComponent<Text>().text + " wins!" +
				"\nResult has been sent in.";
			CONSTANTS.netManager.SendResults ("Away");
		}
		else
		{
			winnerPannel.transform.GetChild (0).GetComponent<Text>().text = "Its a tie!" +
				"\nResult has been sent in.";
			CONSTANTS.netManager.SendResults ("Tie");
		}
	}

	void DisplayWaitingMessage()
	{
		winnerPannel.SetActive (true);
		winnerPanelButton.SetActive (false);
		winnerPannel.GetComponentInChildren<Text> ().text = "No tasks currently assigned... \n\nWaiting...";
	}

	public void ChangeWaitingMessage()
	{
		ResetEverything ();
		homeTeamText.GetComponent<Text>().text = CONSTANTS.homeTeamName;
		awayTeamText.GetComponent<Text>().text = CONSTANTS.awayTeamName;
		winnerPannel.SetActive (true);
		winnerPanelButton.SetActive (true);
		winnerPanelButton.GetComponentInChildren<Text> ().text = "OK";
		winnerPannel.GetComponentInChildren<Text> ().text = "You are needed on field #" + CONSTANTS.fieldNum.ToString () + ".";
		CONSTANTS.currentlyInGame = true;
	}

	public void WaitingPanelButtonClick()
	{
		winnerPannel.SetActive (false);
		winnerPanelButton.SetActive (false);
	}

	public override void ResetEverything()
	{
		base.ResetEverything ();
		homeTeamText.GetComponent<Text> ().text = "Home Team";
		awayTeamText.GetComponent<Text> ().text = "Away Team";
		DisplayWaitingMessage ();
	}

	public void ButtonClick()
	{
		if (!CONSTANTS.currentlyInGame) {
			ResetEverything ();
		} else {
			winnerPannel.SetActive (false);
			winnerPanelButton.SetActive (false);
		}
	}
}
