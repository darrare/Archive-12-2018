using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckpointToolTip : MonoBehaviour {

	GameObject toolTip;
	Text text;

	// Use this for initialization
	void Start () {
		toolTip = transform.FindChild ("CheckPointToolTip").gameObject;
		text = toolTip.transform.GetChild (0).GetComponent<Text> ();
		toolTip.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeToolTipMessage(string message)
	{
		text.text = message;
	}

	public void IsActive(bool value)
	{
		if (value) {
			toolTip.SetActive (true);
		} else {
			toolTip.SetActive (false);
		}
	}
}
