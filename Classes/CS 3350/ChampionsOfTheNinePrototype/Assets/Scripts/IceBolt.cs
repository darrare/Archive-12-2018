using UnityEngine;
using System.Collections;

public class IceBolt : MonoBehaviour {
	Transform target;
	CharacterController character;
	// Use this for initialization
	void Start () {
		character = GameObject.Find ("Character").GetComponent<CharacterController> ();
		GameObject.Find ("AudioSource").GetComponent<AudioControl> ().PlayIce ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 0, -20));
	}

	public Transform SetTarget
	{
		set { target = value; }
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player" && collision.tag != "Enemy") {
			GameObject particle = Instantiate (Resources.Load ("IceParticle")) as GameObject;
			particle.transform.position = new Vector3 (transform.position.x, transform.position.y, particle.transform.position.z);
			Destroy (this.gameObject);
		} else if (collision.transform == target) {
			GameObject particle = Instantiate (Resources.Load ("IceParticle")) as GameObject;
			particle.transform.position = new Vector3 (transform.position.x, transform.position.y, particle.transform.position.z);
			if (target.name != "Character") {
				collision.gameObject.GetComponent<AIController> ().RemoveHealth (WeaponControl.IceDamage);
			} else {
				character.RemoveHealth (WeaponControl.IceDamage);
			}
			Destroy (this.gameObject);
		} else if (target == null && collision.transform.tag != "Player") {
			GameObject particle = Instantiate (Resources.Load ("IceParticle")) as GameObject;
			particle.transform.position = new Vector3 (transform.position.x, transform.position.y, particle.transform.position.z);
			collision.gameObject.GetComponent<AIController> ().RemoveHealth (WeaponControl.IceDamage);
			Destroy (this.gameObject);
		}
	}
}
