using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomDebug : MonoBehaviour {
	Text debugText;

	// Use this for initialization
	void Start () {
		debugText = GetComponent<Text> ();
	}

	public void Log(string info)
	{
		debugText.text += "\n" + info;
	}
}
