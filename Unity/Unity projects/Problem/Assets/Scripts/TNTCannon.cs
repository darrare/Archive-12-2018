using UnityEngine;
using System.Collections;

public class TNTCannon : MonoBehaviour {
	public Vector2 initialVelocity;
	public float initialVelocityRange;

	public GameObject destroyWhenDead;

	float shortDelay = 2;
	float longDelay = 5;
	float timer = 0;
	float delay;


	bool canShoot = false;

	// Use this for initialization
	void Start () {
		delay = Random.Range (shortDelay, longDelay);

	}
	
	// Update is called once per frame
	void Update () {
		if (canShoot) {
			timer += Time.deltaTime;
			if (timer > delay) {
				timer = 0;
				delay = Random.Range (shortDelay, longDelay);
				GameObject newBarrel = Instantiate (Resources.Load ("TNT"), transform.GetChild (0).position, Quaternion.identity) as GameObject;
				newBarrel.GetComponent<Rigidbody2D> ().velocity = initialVelocity + Vector2.right * Random.Range (-initialVelocityRange, initialVelocityRange);
				if (destroyWhenDead != null) {
					newBarrel.transform.SetParent (destroyWhenDead.transform);
				}
			}
		}

	}

	public bool CanShoot
	{
		set {canShoot = value;}
	}
		
}
