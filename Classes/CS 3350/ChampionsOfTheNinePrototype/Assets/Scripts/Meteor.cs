using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 0, -20));
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player" && collision.tag != "Enemy") {
			GameObject particle = Instantiate (Resources.Load ("FireParticle")) as GameObject;
			particle.transform.position = new Vector3 (transform.position.x, transform.position.y, particle.transform.position.z);
			GameObject.Find ("AudioSource").GetComponent<AudioControl> ().PlayMeteor ();
			Destroy (this.gameObject);
		} else if (collision.tag == "Enemy") {
			GameObject particle = Instantiate (Resources.Load ("FireParticle")) as GameObject;
			particle.transform.position = new Vector3 (transform.position.x, transform.position.y, particle.transform.position.z);
			collision.gameObject.GetComponent<AIController> ().RemoveHealth (WeaponControl.FireDamage);
			GameObject.Find ("AudioSource").GetComponent<AudioControl> ().PlayMeteor ();
			Destroy (this.gameObject);
//		} else if (collision.tag == "Respawn") {
//			GameObject.Find ("castle").GetComponent<EnemyCastle>().RemoveHealth ();
//			GameObject.Find ("AudioSource").GetComponent<AudioControl> ().PlayMeteor ();
//			GameObject particle = Instantiate (Resources.Load ("FireParticle")) as GameObject;
//			particle.transform.position = new Vector3 (transform.position.x, transform.position.y, particle.transform.position.z);
//			Destroy (this.gameObject);
//		}
		}
	}
}
