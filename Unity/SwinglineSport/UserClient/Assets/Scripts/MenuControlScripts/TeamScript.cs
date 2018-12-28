using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TeamScript : MonoBehaviour {
	public void Clicked()
	{
		CONSTANTS.userHomeControl.SearchForTeam (transform.GetChild (0).GetComponent<Text> ().text);
	}
}
