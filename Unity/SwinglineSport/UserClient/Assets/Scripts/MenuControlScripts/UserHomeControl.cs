using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class UserHomeControl : MonoBehaviour {
	enum MenuState { NOTONTEAM, LEADEROFTEAM, MEMBEROFTEAM };
	MenuState menuState;

	public Text teamName;
	public Text teamLeader;
	public Text player1;
	public Text player2;

	public GameObject leaderMenu;
	public GameObject memberMenu;
	public GameObject notOnTeamMenu;

	public GameObject createTeamMenu;
	public GameObject disbandTeamMenu;
	public GameObject leaveTeamMenu;
	public GameObject searchForTeamMenu;
	bool searchForTeamMenuActive = false;
	public GameObject applyToTeamMenu;
	public GameObject[] applyToTeamInformation;
	public GameObject applicantMenu;
	public GameObject[] applicantInformation;
	bool applicantMenuActive = false;
	public GameObject singleApplicantDisplay;
	string mostRecentApplicantUsername = "ggg";
	string mostRecentApplicantName = "ggg";
	public GameObject teamContactInfo;
	public GameObject verificationMenu;
	public GameObject kickMemberMenu;

	public GameObject teamSearchBar;
	public GameObject searchContent;
	public GameObject playerSearchContent;


	List<Team> teams = new List<Team>();
	List<Team> teamsCurrentlyDisplayed = new List<Team> ();

	List<Player> applicants = new List<Player>();

	// Use this for initialization
	void Start () {
		CONSTANTS.netManager.RequestUserHomeUpdate ();
		CONSTANTS.userHomeControl = this.GetComponent<UserHomeControl> ();
		leaderMenu.SetActive (false);
		memberMenu.SetActive (false);
		notOnTeamMenu.SetActive (false);
		createTeamMenu.SetActive (false);
		disbandTeamMenu.SetActive (false);
		leaveTeamMenu.SetActive (false);
		searchForTeamMenu.SetActive (false);
		applyToTeamMenu.SetActive (false);
		applicantMenu.SetActive (false);
		singleApplicantDisplay.SetActive (false);
		teamContactInfo.SetActive (false);
		verificationMenu.SetActive (false);
		kickMemberMenu.SetActive (false);
	}

	public void WebsitePayment()
	{
		//Application.OpenURL ("https://www.swinglinesport.com");
		DisplayVerificationMenu("Payment is currently disabled. Please check back soon.");
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void ShareToFaceBook()
	{
		Application.OpenURL ("https://www.facebook.com/swinglinesport/");
	}

	public void ShareToTwitter()
	{
		//Application.OpenURL ("https://twitter.com/intent/tweet?text=" + WWW.EscapeURL (CONSTANTS.tweetMessage)); // + "&amp;lang=" + WWW.EscapeURL ("en"));
		Application.OpenURL("https://www.twitter.com/SwinglineSport");
	}

	public void ShareToInstagram()
	{
		Application.OpenURL ("https://www.instagram.com/swinglinesport/");
	}

	public void ShareToYoutube()
	{
		Application.OpenURL ("https://www.youtube.com/channel/UCDELRIrHRNdFhU87Pcp9dvw");
	}

	public void ReceiveUpdate(string teamName, string teamLeader, string player1, string player2, int status) //status 0=not on team, 1=leader, 2=member
	{
		this.teamName.text = teamName;
		CONSTANTS.teamName = teamName;
		this.teamLeader.text = teamLeader;
		this.player1.text = player1;
		this.player2.text = player2;

		if (status == 0) {
			menuState = MenuState.NOTONTEAM;
		} else if (status == 1) {
			menuState = MenuState.LEADEROFTEAM;
		} else if (status == 2) {
			menuState = MenuState.MEMBEROFTEAM;
		}

		UpdateMenu ();
	}

	void UpdateMenu()
	{
		if (menuState == MenuState.NOTONTEAM) {
			notOnTeamMenu.SetActive (true);
			leaderMenu.SetActive (false);
			memberMenu.SetActive (false);
		} else if (menuState == MenuState.LEADEROFTEAM) {
			leaderMenu.SetActive (true);
			memberMenu.SetActive (false);
			notOnTeamMenu.SetActive (false);
		} else if (menuState == MenuState.MEMBEROFTEAM) {
			memberMenu.SetActive (true);
			leaderMenu.SetActive (false);
			notOnTeamMenu.SetActive (false);
		}
	}

	public void CreateTeamButtonClick()
	{
		createTeamMenu.SetActive (true);
	}

	public void CreateTeam()
	{
		CONSTANTS.netManager.SendCreateTeamRequest (createTeamMenu.transform.GetChild (0).FindChild ("TeamName").GetComponent<InputField> ().text);
	}

	public void CreateTeamClose()
	{
		createTeamMenu.SetActive (false);
		createTeamMenu.transform.GetChild (0).FindChild ("ErrorText").GetComponent<Text> ().text = "";
	}

	public void LeaveTeamButtonClick()
	{
		leaveTeamMenu.SetActive (true);
	}

	public void LeaveTeam()
	{
		CONSTANTS.netManager.SendLeaveTeamRequest (leaveTeamMenu.transform.GetChild (0).FindChild ("TeamName").GetComponent<InputField> ().text);
	}

	public void LeaveTeamClose()
	{
		leaveTeamMenu.SetActive (false);
		leaveTeamMenu.transform.GetChild (0).FindChild ("ErrorText").GetComponent<Text> ().text = "";
	}

	public void SearchForTeamButtonClick()
	{
		searchForTeamMenu.SetActive (true);
		searchForTeamMenuActive = true;
		CONSTANTS.netManager.SendRequestForTeamList ();
	}

	public void SearchForTeam(string value)
	{
		applyToTeamMenu.SetActive (true);
		searchForTeamMenu.SetActive (false);
		for (int i = 0; i < teams.Count; i++) {
			if (teams [i].teamName == value) {
				applyToTeamInformation [0].GetComponent<Text>().text = teams[i].teamName;
				applyToTeamInformation [1].GetComponent<Text>().text = teams[i].leaderName;
				applyToTeamInformation [2].GetComponent<Text>().text = teams[i].player1;
				applyToTeamInformation [3].GetComponent<Text>().text = teams[i].player2;
				break;
			}
		}
	}

	public void ApplyForTeam()
	{
		CONSTANTS.netManager.SendPlayerTeamApplication (applyToTeamInformation [0].GetComponent<Text> ().text);
	}

	public void ApplyForTeamClose()
	{
		applyToTeamMenu.SetActive (false);
		SearchForTeamClose ();
		applyToTeamMenu.transform.GetChild (0).FindChild ("ErrorText").GetComponent<Text> ().text = "";
	}

	public void SearchForTeamClose()
	{
		searchForTeamMenu.SetActive (false);
		searchForTeamMenuActive = false;
		teams.Clear ();
	}

	public void DisbandTeamButtonClick()
	{
		disbandTeamMenu.SetActive (true);
	}

	public void DisbandTeam()
	{
		CONSTANTS.netManager.SendDeleteTeamRequest (disbandTeamMenu.transform.GetChild (0).FindChild ("TeamName").GetComponent<InputField> ().text);
	}

	public void DisbandTeamClose()
	{
		disbandTeamMenu.SetActive (false);
		disbandTeamMenu.transform.GetChild (0).FindChild ("ErrorText").GetComponent<Text> ().text = "To disband your team. Type your team name into the box above.";
	}

	public void AddTeamToList(string teamName, string leaderName, string player1, string player2)
	{
		if (searchForTeamMenuActive) {
			teams.Add (new Team (teamName, leaderName, player1, player2));
			UpdateTeamView ();
		}
	}

	public void FinishedAddingTeamsToList()
	{
		teams.Sort (new TeamComparer ());
		UpdateTeamView ();
	}

	public void UpdateTeamView()
	{
		teamsCurrentlyDisplayed.Clear ();
		foreach (Transform child in searchContent.transform) {
			Destroy (child.gameObject);
		}
		string includeString = teamSearchBar.GetComponent<InputField> ().text;

		foreach (Team team in teams) {
			if (team.teamName.IndexOf(includeString, System.StringComparison.OrdinalIgnoreCase) >=0) {
				teamsCurrentlyDisplayed.Add (team);
			}
		}

		int index = 0;
		float startingPosition = -35f;
		float incrementingDistance = -70f;

		searchContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (searchContent.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Abs (teamsCurrentlyDisplayed.Count * incrementingDistance));

		foreach (Team team in teamsCurrentlyDisplayed) {
			GameObject newPanel = Instantiate (Resources.Load ("TeamPanel")) as GameObject;
			RectTransform rect = newPanel.GetComponent<RectTransform> ();
			newPanel.transform.GetChild (0).GetComponent<Text> ().text = team.teamName;
			newPanel.transform.SetParent (searchContent.transform);
			rect.anchoredPosition = new Vector2 (0, startingPosition + index * incrementingDistance);
			rect.localScale = Vector3.one;
			rect.sizeDelta = new Vector2 (0, rect.sizeDelta.y);
			index++;
		}
	}

	public void ViewApplicantsClick()
	{
		applicantMenu.SetActive (true);
		applicantMenuActive = true;
		applicants.Clear ();
		UpdateApplicantView ();
		CONSTANTS.netManager.SendRequestForApplicants ();
	}

	public void ApplicantClicked(string value)
	{
		applicantMenu.SetActive (false);
		applicantMenuActive = false;
		singleApplicantDisplay.SetActive (true);

		for (int i = 0; i < applicants.Count; i++) {
			if (applicants[i].playerName == value) {
				mostRecentApplicantUsername = applicants [i].username;
				mostRecentApplicantName = applicants [i].playerName;
				applicantInformation [0].GetComponent<Text>().text = applicants[i].playerName;
				applicantInformation [1].GetComponent<Text>().text = applicants[i].playerGender;
				applicantInformation [2].GetComponent<Text>().text = applicants[i].playerAge.ToString();
				break;
			}
		}
	}
	public void ApplicantAccepted()
	{
		CONSTANTS.netManager.SendApplicantAcceptance (mostRecentApplicantUsername, CONSTANTS.teamName);
		singleApplicantDisplay.SetActive (false);
		CONSTANTS.netManager.RequestUserHomeUpdate ();
		DisplayVerificationMenu ("You have added <color=teal>" + mostRecentApplicantName + "</color> to your team.");
	}

	public void ApplicantDenied()
	{
		CONSTANTS.netManager.SendApplicantDenial (mostRecentApplicantUsername, CONSTANTS.teamName);
		singleApplicantDisplay.SetActive (false);
		DisplayVerificationMenu ("You have declined <color=teal>" + mostRecentApplicantName + "</color> to your team.");
	}

	public void ApplicantsMenuClose()
	{
		applicantMenu.SetActive (false);
	}

	public void UpdateApplicantView()
	{
		foreach (Transform child in playerSearchContent.transform) {
			Destroy (child.gameObject);
		}
			
		int index = 0;
		float startingPosition = -35f;
		float incrementingDistance = -70f;

		playerSearchContent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (searchContent.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Abs (applicants.Count * incrementingDistance));

		foreach (Player player in applicants) {
			GameObject newPanel = Instantiate (Resources.Load ("PlayerPanel")) as GameObject;
			RectTransform rect = newPanel.GetComponent<RectTransform> ();
			newPanel.transform.GetChild (0).GetComponent<Text> ().text = player.playerName;
			newPanel.transform.SetParent (playerSearchContent.transform);
			rect.anchoredPosition = new Vector2 (0, startingPosition + index * incrementingDistance);
			rect.localScale = Vector3.one;
			rect.sizeDelta = new Vector2 (0, rect.sizeDelta.y);
			index++;
		}
	}

	public void AddApplicantToList(string username, string playerName, string bday, int gender)
	{
		if (applicantMenuActive) {
			applicants.Add (new Player (username, playerName, gender, bday));
			UpdateApplicantView ();
		}
	}

	public void CloseTeamContactInfo()
	{
		teamContactInfo.SetActive (false);
	}

	public void ViewTeamContactInfo()
	{
		teamContactInfo.SetActive (true);
		CONSTANTS.netManager.SendRequestForTeamContactInfo ();
	}

	public void SetTeamContactInfo(string player0Name, string player0Email, string player0Phone, string player1Name, string player1Email, string player1Phone, string player2Name, string player2Email, string player2Phone)
	{
		teamContactInfo.transform.GetChild (0).FindChild ("TeamInfo").GetComponent<Text> ().text = 
			player0Name + "\n     " + player0Email + "\n     " + player0Phone + "\n\n\n" +
			player1Name + "\n     " + player1Email + "\n     " + player1Phone + "\n\n\n" +
			player2Name + "\n     " + player2Email + "\n     " + player2Phone;
	}

	public void DisplayVerificationMenu(string message)
	{
		verificationMenu.SetActive (true);
		verificationMenu.transform.FindChild ("VerificationDescription").GetComponent<Text> ().text = message;
	}

	public void CloseVerificationMenu()
	{
		verificationMenu.SetActive (false);
	}

	public void RemoveTeamMemberButtonClick()
	{
		kickMemberMenu.SetActive (true);
	}

	public void RemoveTeamMember()
	{
		CONSTANTS.netManager.SendRemoveMemberRequest (kickMemberMenu.transform.GetChild(0).FindChild("PlayerName").GetComponent<InputField> ().text);
	}

	public void RemoveTeamMemberClose()
	{
		kickMemberMenu.SetActive (false);
		kickMemberMenu.transform.GetChild (0).FindChild ("ErrorText").GetComponent<Text> ().text = "To remove a member from your team, type their name into the box above.";
	}

}


public struct Team
{
	public string teamName;
	public string leaderName;
	public string player1;
	public string player2;

	public Team(string teamName, string leaderName, string player1, string player2)
	{
		this.teamName = teamName;
		this.leaderName = leaderName;
		this.player1 = player1;
		this.player2 = player2;
	}


}

public class TeamComparer : IComparer<Team>
{
	public int Compare(Team a, Team b)
	{
		return a.teamName.CompareTo (b.teamName);
	}
}

public struct Player
{
	public string username;
	public string playerName;
	public string playerGender;
	public int playerAge;

	public Player (string username, string playerName, int playerGender, string dateOfBirth)
	{
		this.username = username;
		this.playerName = playerName;
		if (playerGender == 0) {
			this.playerGender = "Male";
		} else {
			this.playerGender = "Female";
		}

		DateTime bday = Convert.ToDateTime (dateOfBirth);
		playerAge = DateTime.Today.Year - bday.Year;
		if (bday > DateTime.Today.AddYears (-playerAge)) {
			playerAge--;
		}
	}
}