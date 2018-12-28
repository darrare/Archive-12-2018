using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class InputScreenControl : MonoBehaviour {
	public InputField[] inputFields = new InputField[4];
	public Text[] displayTeam = new Text[4];
	private InputField selectedField;
	public Text teamName;
	public Text captainName;
	public Text player1Name;
	public Text player2Name;
	private string normalText;
	public static List<Team> teams = new List<Team>();
	public Dropdown teamsDropdown;
	public Text info;
	private string notification = "";
	EventSystem system;

	void Start()
	{
		system = EventSystem.current;
		normalText = teamName.text;
		system.SetSelectedGameObject (inputFields[0].gameObject, new BaseEventData (system));
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable> ().FindSelectableOnDown ();
			if (next != null) {
				InputField inputfield = next.GetComponent<InputField> ();
				if (inputfield != null)
					inputfield.OnPointerClick (new PointerEventData (system));  //if it's an input field, also set the text caret
				system.SetSelectedGameObject (next.gameObject, new BaseEventData (system));
			}
			//else Debug.Log("next nagivation element not found");
		} else if (Input.GetKeyDown (KeyCode.Return)) {
			SubmitTeam ();
			system.SetSelectedGameObject (inputFields[0].gameObject, new BaseEventData (system));
		}

		info.text = notification + "\n\n\n\n\n\n\nNumber of teams: " + teams.Count;
	}

	public void SubmitTeam()
	{
		if (teamName.text != "" && captainName.text != "" && player1Name.text != "" && player2Name.text != "") {
			Debug.Log ("team added");
			teams.Add (new Team(teamName.text, captainName.text, player1Name.text, player2Name.text));
			teamsDropdown.options.Add (new Dropdown.OptionData(teamName.text));
			foreach (InputField inputField in inputFields)
			{
				inputField.text = "";
			}
			notification = "Team submitted";
			teamsDropdown.value = teamsDropdown.options.Count;
			DisplayTeam ();
		} else {
			notification = "Error in team submission";
		}
	}

	public void DisplayTeam()
	{
		displayTeam [0].text = teams [teamsDropdown.value].TeamName;
		displayTeam [1].text = teams [teamsDropdown.value].CaptainName;
		displayTeam [2].text = teams [teamsDropdown.value].Player1Name;
		displayTeam [3].text = teams [teamsDropdown.value].Player2Name;
		notification = "Displaying " + teams [teamsDropdown.value].TeamName;
	}

	public void RemoveTeam()
	{
		if (teams.Count > 1) {
			teams.RemoveAt (teamsDropdown.value);
			teamsDropdown.options.RemoveAt (teamsDropdown.value);
			if (teamsDropdown.options.Count == 0) {
				teamsDropdown.options.Clear ();
			} else {
				teamsDropdown.value -= 1;
			}
			DisplayTeam ();
		} else {
			teams.Clear ();
			teamsDropdown.options.Clear ();
			notification = "No teams to remove";
			displayTeam [0].text = "";
			displayTeam [1].text = "";
			displayTeam [2].text = "";
			displayTeam [3].text = "";
			teamsDropdown.captionText.text = "Teams";
		}

	}

	public void WriteToFile()
	{
		File.WriteAllText ("Teams.txt", string.Empty);
		List<string> lines = new List<string> ();
		for (int i = 0; i < teams.Count; i++) {
			lines.Add(teams[i].teamName + "," + teams[i].captainName + "," + teams[i].player1Name + "," + teams[i].player2Name);
		}
		StreamWriter writer = new StreamWriter ("Teams.txt", true);
		foreach (string line in lines) {
			writer.WriteLine (line);
		}
		writer.Close ();
		notification = "Teams saved";
	}

	public void WriteToFile(string location)
	{
		File.WriteAllText (location, string.Empty);
		List<string> lines = new List<string> ();
		for (int i = 0; i < teams.Count; i++) {
			lines.Add(teams[i].teamName + "," + teams[i].captainName + "," + teams[i].player1Name + "," + teams[i].player2Name);
		}
		StreamWriter writer = new StreamWriter (location, true);
		foreach (string line in lines) {
			writer.WriteLine (line);
		}
		writer.Close ();
		notification = "Teams saved";
	}

	public void ReadFromFile()
	{
		teams.Clear ();
		teamsDropdown.options.Clear ();
		string[] splitLine;
		StreamReader reader = new StreamReader ("Teams.txt");
		string line;

		while ((line = reader.ReadLine ()) != null) {
			splitLine = line.Split (',');
			teams.Add (new Team(splitLine[0], splitLine[1], splitLine[2], splitLine[3]));
			teamsDropdown.options.Add (new Dropdown.OptionData(splitLine[0]));
		}
		reader.Close ();
		File.Copy ("Teams.txt", "MostRecentLoad.txt", true);
		notification = "Teams loaded";
	}

	public void GenerateBracket()
	{
		WriteToFile ("GeneratedTeams.txt");
		Application.LoadLevel ("Bracket");
	}

	public void TempButton()
	{
		Application.LoadLevel ("StaffManagement");
	}
}
