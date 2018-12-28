using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("KillSelf", .5f);
		GameObject.Find ("AudioSource").GetComponent<AudioControl> ().PlayLightning ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy") {
			collision.gameObject.GetComponent<AIController>().RemoveHealth(WeaponControl.LightningDamage);
		}
	}

	void KillSelf()
	{
		Destroy (this.gameObject);
	}
}
