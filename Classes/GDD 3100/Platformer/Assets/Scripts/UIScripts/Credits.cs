using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
	GameObject creditsPane;
	bool isOpen = false;
	// Use this for initialization
	void Start () {
		creditsPane = GameObject.Find ("Credits");
		creditsPane.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isOpen && Input.GetKeyDown (KeyCode.I)) {
			creditsPane.SetActive (true);
			isOpen = true;
		} else if (isOpen && Input.GetKeyDown (KeyCode.I)) {
			creditsPane.SetActive (false);
			isOpen = false;
		}
	}
}
