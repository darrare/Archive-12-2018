using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using LgOctEngine.CoreClasses;

public class InterfaceControl : MonoBehaviour {
	GameObject login, administrativeMenu, menuButton, listOfClients, levels;
	Text loginText;
	InputField username, password;

	// Use this for initialization
	void Start () {
		login = GameObject.Find ("Login");
		administrativeMenu = GameObject.Find ("AdministrativeMenu");
		menuButton = GameObject.Find ("BackToAdministrativeMenuButton");
		listOfClients = GameObject.Find ("ListOfClients");
		levels = GameObject.Find ("Levels");
		loginText = GameObject.Find ("LoginText").GetComponent<Text> ();
		username = GameObject.Find ("Username").GetComponent<InputField> ();
		password = GameObject.Find ("Password").GetComponent<InputField> ();

		administrativeMenu.SetActive (false);
		menuButton.SetActive (false);
		listOfClients.SetActive (false);
		levels.SetActive (false);
	}


	public void SignInButtonClick()
	{
		if (username.text == "administrator" && password.text == "password") {
			login.SetActive (false);
			administrativeMenu.SetActive (true);
			menuButton.SetActive (true);
		} else {
			loginText.text = "Username or password was incorrect...";
		}
	}

	public void BackToAdministrativeMenuClick()
	{
		administrativeMenu.SetActive (true);
		listOfClients.SetActive (false);
		levels.SetActive (false);
	}

	public void DisplayClientsClick()
	{
		string connectionString = "";
		administrativeMenu.SetActive (false);
		foreach (Connection con in NetManager.networkConnections) {
			connectionString += con.username + ": IPv4: " + con.ip + ": ConnectionID: " + con.connectionId + "\n";
		}
		listOfClients.SetActive (true);
		listOfClients.GetComponent<Text> ().text = connectionString;
	}

	public void ViewSavedLevelsClick()
	{
		administrativeMenu.SetActive (false);
		levels.SetActive (true);
		foreach (Transform child in levels.transform) {
			GameObject.Destroy(child.gameObject);
		}
		float Y = 224f;
		float offset = 35f;
		int index = 0;
		foreach (string str in NetManager.levels) {
			//JsonMessage<Level> jsonMessage = str;
			Debug.Log (str);
			Level obj = LgJsonNode.CreateFromJsonString<Level>(str);
			Debug.Log (obj.LevelName);
			GameObject newButton = Instantiate (Resources.Load ("LevelButton")) as GameObject;
			newButton.transform.SetParent (levels.transform);
			newButton.name = obj.LevelName;
			newButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, Y - (offset * index), 0);
			newButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			newButton.GetComponentInChildren<Text>().text = obj.LevelName;
			index++;
		}
	}

	public void LevelButtonClick(GameObject gameObj)
	{
		foreach (string str in NetManager.levels) {
			//JsonMessage<Level> jsonMessage = str;
			Level obj = LgJsonNode.CreateFromJsonString<Level>(str);
			if (obj.LevelName == gameObj.name)
			{
				levels.GetComponent<Text>().text = obj.LevelObjectArray.Serialize ();
			}
		}
	}
}

public class Connection
{
	public string ip;
	public int connectionId;
	public string username;

	public Connection(string ip, int connectionId, string username)
	{
		this.ip = ip;
		this.connectionId = connectionId;
		this.username = username;
	}
}
