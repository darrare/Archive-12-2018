using UnityEngine;
using System.Collections;

public class BandCollection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			GameObject.Find ("Character").GetComponent<WeaponControl>().EnableFireWave();
			GameObject.Find ("Character").GetComponent<PlatformCharacterController2D>().BandFound();
			GameObject.Find ("PauseMenu").GetComponent<LevelOneController>().GainedBand();
			Destroy (this.gameObject);
		}
	}
}
