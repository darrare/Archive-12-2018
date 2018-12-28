using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StaffClick : MonoBehaviour {
	public void OnClick()
	{
		GameObject.Find ("StaffNameText").GetComponent<Text> ().text = this.name;
	}
}
