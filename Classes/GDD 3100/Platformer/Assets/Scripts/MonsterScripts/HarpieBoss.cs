using UnityEngine;
using System.Collections;

public class HarpieBoss : MonoBehaviour {
	Animator anim;
	GameObject player;
	bool engaged = false;
	bool attacking = false;
	bool dead = false;
	bool diving = false;
	bool runOnce = false;
	Vector3 pos;
	float maxHealth = 1000;
	float curHealth = 1000;

	float speed = 2;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		player = GameObject.Find ("Character");
		InvokeRepeating ("Attack", 4, 4);
	}
	
	// Update is called once per frame
	void Update () {
		if (curHealth <= 0) {
			dead = true;
			GetComponent<Rigidbody2D>().isKinematic = false;
			anim.SetBool ("isDead", true);
		}
		if (!dead && engaged && !attacking && !diving) {
			if (transform.position.x > player.transform.position.x) {
				transform.localScale = new Vector3 (1, 1, 1);
				pos = new Vector3 (player.transform.position.x + 1f, player.transform.position.y + 1f, 1);
			} else {
				transform.localScale = new Vector3 (-1, 1, 1);
				pos = new Vector3 (player.transform.position.x - 1f, player.transform.position.y + 1f, 1);
			}
			transform.position = Vector3.Lerp (transform.position, pos, speed * Time.deltaTime);



		} else if (!dead && attacking) {
			anim.SetBool ("isAttacking", true);

			if (!runOnce)
			{
				runOnce = true;
				//instantiate feather attack here
				GameObject feather0 = Instantiate(Resources.Load ("HarpieFeather")) as GameObject;
				feather0.transform.position = transform.position;
				GameObject feather1 = Instantiate(Resources.Load ("HarpieFeather")) as GameObject;
				feather1.transform.position = new Vector2(transform.position.x, transform.position.y + .1f);
				Invoke ("BackToIdle", 1);
			}
		} else if (!dead && diving) {
			anim.SetBool ("isDiving", true);
			pos = new Vector3 (player.transform.position.x, player.transform.position.y, 1);
			transform.position = Vector3.Lerp (transform.position, pos, speed * Time.deltaTime);
			if (!runOnce)
			{
				runOnce = true;
				Invoke ("BackToIdle", 1);
			}
		} else if (!dead && Vector2.Distance (player.transform.position, transform.position) < 10) {
			engaged = true;
			BossHealth.EnableBossBar("Harpie Queen", maxHealth);
		}
	}

	void BackToIdle()
	{
		attacking = false;
		diving = false;
		runOnce = false;
		anim.SetBool ("isAttacking", false);
		anim.SetBool ("isDiving", false);
	}

	void Attack()
	{
		if (engaged) {
			int rand = Random.Range (0, 2); //0 or 1
			if (rand == 0) {
				diving = true;
			} else {
				attacking = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "PlayerWeapon") {
			curHealth -= 50;
			BossHealth.DamageDealt(50);
			Destroy (collider.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Invoke ("Decay", 1.5f);
	}

	void Decay()
	{
		Tutorial3Control.CompleteMileStone (0);
		GameObject newObject = Instantiate (Resources.Load ("PendantOfTheAdventurer")) as GameObject;
		newObject.transform.position = transform.position;
		Destroy (gameObject);
	}
}
