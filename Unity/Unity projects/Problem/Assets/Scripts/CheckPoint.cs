using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
	CheckpointToolTip toolTip;
	public string message = null;
	GameObject flag;

	bool isActive = false;

	// Use this for initialization
	void Start () {
		flag = transform.GetChild (0).gameObject;
		flag.SetActive (false);
		toolTip = CONSTANTS.levelController.gameObject.GetComponent<CheckpointToolTip> ();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			flag.SetActive (true);
			if (message != "") {
				toolTip.ChangeToolTipMessage (message);
				toolTip.IsActive (true);
			}
			if (!isActive) {
				CONSTANTS.levelController.SetRespawnPoint (transform);
				GetComponent<AudioSource> ().Play ();
				isActive = true;
			}

		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			toolTip.IsActive (false);
		}
	}
}
