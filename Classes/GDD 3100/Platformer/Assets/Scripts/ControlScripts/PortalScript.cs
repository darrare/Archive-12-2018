using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
	public string targetScene;
	public Vector2 targetLocation;

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player" && Input.GetAxis ("Vertical") > 0) {
			if (GAMECONSTANTS.playerMapPosition.x == 9999);
			{
				GAMECONSTANTS.playerMapPosition = targetLocation;
			}
			Application.LoadLevel (targetScene);
		}
			
	}
}
