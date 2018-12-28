using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReturnResult : MonoBehaviour {

	public void HomeWinClick()
	{
		GameObject.Find ("NetManager").GetComponent<NetManager> ().HomeWinClick ();
	}

	public void AwayWinClick()
	{
		GameObject.Find ("NetManager").GetComponent<NetManager> ().AwayWinClick ();
	}
}
