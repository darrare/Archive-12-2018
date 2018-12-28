using UnityEngine;
using System.Collections;

public class Elephant : MonoBehaviour {

	void DeathMethod()
	{
		Tutorial2Control.CompleteMileStone (0);
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "PlayerWeapon") {
			DeathMethod ();
			Destroy (collider.gameObject);
		}
	} 
}
