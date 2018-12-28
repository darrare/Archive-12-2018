using UnityEngine;
using System.Collections;

public class EnemyAIGroundCheck : MonoBehaviour {

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Ground") {
			transform.parent.GetComponent<EnemyAI> ().SwitchDirection ();
		}
	}
}
