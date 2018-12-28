using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	public void Clicked()
	{
		CONSTANTS.userHomeControl.ApplicantClicked (transform.GetChild (0).GetComponent<Text> ().text);
	}
}
