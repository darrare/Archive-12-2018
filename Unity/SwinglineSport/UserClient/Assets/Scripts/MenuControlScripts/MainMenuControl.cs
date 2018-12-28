using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour {

	public GameObject firstMenu;
	public GameObject liveEventMenu;
	public GameObject refereeLogInMenu;
	public GameObject shareMenu;
	public GameObject contactMenu;
	public GameObject sponsorsMenu;
	public GameObject backToMainButton;
	public GameObject userLogInMenu;
	public GameObject registerMenu;

	public GameObject[] userLoginRequirements = new GameObject[2];
	public GameObject[] staffLoginRequirements = new GameObject[2];
	public GameObject[] registerElements = new GameObject[7];


	public Image backgroundImage;
	public Sprite[] backgroundImages;

	void Awake()
	{
		CONSTANTS.mainMenuControl = this.GetComponent<MainMenuControl> ();
		CONSTANTS.netManager = GameObject.Find ("NetManager").GetComponent<NetManager> ();
		CloseAll ();
		firstMenu.SetActive (true);
		backToMainButton.SetActive (false);
		SetRandomBackgroundImage ();
	}

	void SetRandomBackgroundImage()
	{
		int index = UnityEngine.Random.Range (0, backgroundImages.Length);
		backgroundImage.sprite = backgroundImages [index];
	}

	void CloseAll()
	{
		firstMenu.SetActive (false);
		liveEventMenu.SetActive (false);
		refereeLogInMenu.SetActive (false);
		shareMenu.SetActive (false);
		contactMenu.SetActive (false);
		sponsorsMenu.SetActive (false);
		backToMainButton.SetActive (false);
		userLogInMenu.SetActive (false);
		registerMenu.SetActive (false);
	}

	//Back to main menu button click
	public void BackToMainMenuClick()
	{
		CloseAll ();
		firstMenu.SetActive (true);
	}

	//FIRST MENU BUTTONS
	public void TrackScoreClick()
	{
		SceneManager.LoadScene ("UserScoreTracking");
	}

	public void SignUpClick()
	{
		CloseAll ();
		userLogInMenu.SetActive (true);
		backToMainButton.SetActive (true);
	}

	public void ShareClick()
	{
		CloseAll ();
		shareMenu.SetActive (true);
		backToMainButton.SetActive (true);
	}

	public void ContactUsClick()
	{
		CloseAll ();
		contactMenu.SetActive (true);
		backToMainButton.SetActive (true);
	}

	public void SponsorsClick()
	{
		CloseAll ();
		sponsorsMenu.SetActive (true);
		backToMainButton.SetActive (true);
	}

	public void LiveEventClick()
	{
		CloseAll ();
		liveEventMenu.SetActive (true);
		backToMainButton.SetActive (true);
	}



	//User sign up and log in buttons
	public void ForgotPassword()
	{
		//NYI
	}

	public void RegisterForAccountClick()
	{
		string result = RegisterForAccount ();
		if (result == "") {
			CONSTANTS.netManager.SendRegistrationRequest (registerElements [0].GetComponent<InputField> ().text, 
				registerElements [1].GetComponent<InputField> ().text, 
				registerElements [2].GetComponent<InputField> ().text, 
				registerElements [3].GetComponent<InputField> ().text, 
				registerElements [4].GetComponent<InputField> ().text, 
				registerElements [5].GetComponent<InputField> ().text, 
				registerElements [6].GetComponent<InputField> ().text, 
				registerElements [7].GetComponent<InputField> ().text, 
				registerElements [8].GetComponent<InputField> ().text, 
				registerElements [9].GetComponent<Dropdown> ().value);
			
		} else {
			GameObject.Find ("RegisterErrorText").GetComponent<Text> ().text = result;
		}
	}

	public string RegisterForAccount()
	{
		//Verify that all elements of registerElements is valid
		//Check all but the last element (gender) and verify if anything is null
		for (int i = 0; i < registerElements.Length - 1; i++) {
			if (registerElements [i].GetComponent<InputField> ().text == "") {
				return "Please fill the entire registration.";
			}
		}

		return "";

	}

	public void UserSendLoginRequest()
	{
		if (CONSTANTS.isConnectedToServer) {
			CONSTANTS.netManager.SendUserLoginRequest (userLoginRequirements);
		}
		else {
			GameObject.Find ("ErrorText").GetComponent<Text>().text = "Error: You are not connected to the server." +
				"\nPlease verify your internet connection.";
		}
	}

	public void RegisterButtonClick()
	{
		CloseAll ();
		registerMenu.SetActive (true);
	}




	//LIVE EVENT MENU BUTTONS
	public void LiveBracketClick()
	{
		//open live bracket scene
	}

	public void RefereeLoginClick()
	{
		if (!CONSTANTS.isStaff) {
			CloseAll ();
			refereeLogInMenu.SetActive (true);
			backToMainButton.SetActive (true);
			staffLoginRequirements [0].GetComponent<InputField> ().text = "";
			staffLoginRequirements [1].GetComponent<InputField> ().text = "";
			GameObject.Find ("ErrorText").GetComponent<Text> ().text = "";
		} else {
			SceneManager.LoadScene ("StaffScoreTracking");
		}
	}

	public void RefereeSendLoginRequest()
	{
		if (CONSTANTS.isConnectedToServer) {
			CONSTANTS.netManager.SendStaffLoginRequest (staffLoginRequirements);
		}
		else {
			GameObject.Find ("ErrorText").GetComponent<Text>().text = "Error: You are not connected to the server." +
				"\nPlease verify your internet connection.";
		}
	}




	//EMAILS AND SHARING
	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL (url).Replace ("+", "%20");
	}

	public void SendEmail(string emailAddress)
	{
		string email = emailAddress;
		string subject = MyEscapeURL ("FEEDBACK/SUGGESTION/INQUIRY");
		string body = MyEscapeURL("Please Enter your message here\n\n\n\n" +
		                           "________" +
		                           "\n\nPlease Do Not Modify This\n\n" +
		                           "Model: "+SystemInfo.deviceModel+"\n\n"+
		                           "OS: "+SystemInfo.operatingSystem+"\n\n" +
		                           "________");

		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}

	public void SendForgotPasswordEmail()
	{
		string email = "ryandarras@gmail.com";
		string subject = MyEscapeURL ("SWINGLINE PASSWORD RECOVERY");
		string body = MyEscapeURL("This function of the app is currently undergoing development. Please " +
			"manually send us your username, date of birth, and email address, and we will verify your " +
			"account and send you your email address within 24 hours.");

		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
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

	public void OpenURL(string url)
	{
		Application.OpenURL (url);
	}

    public void GenerateBracket()
    {
        //WriteToFile("GeneratedTeams.txt");
        SceneManager.LoadScene("Bracket");
    }

   
}
