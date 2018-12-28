using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject menuButton, loginDetails;

	// Use this for initialization
	void Start () {
		loginDetails.SetActive (false);
	}

	public void MainButtonClick()
	{
		menuButton.SetActive (false);
		loginDetails.SetActive (true);
	}
}
