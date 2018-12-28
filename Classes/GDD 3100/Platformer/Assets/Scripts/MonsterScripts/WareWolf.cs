using UnityEngine;
using System.Collections;

public class WareWolf : MonoBehaviour {
	float[] cameraLimits = new float[4];
	Animator anim;
	Rigidbody2D rigidBody;
	GameObject player;
	bool attacking = false;
	bool dead = false;
	float maxHealth = 1000;
	float curHealth = 1000;
	bool runOnce = false;
	
	float speed = 2;

	// Use this for initialization
	void Start () {
		cameraLimits [0] = -2.67f;
		cameraLimits [1] = 2.67f;
		cameraLimits [2] = 0f;
		cameraLimits [3] = 0f;
		GameObject.Find ("Main Camera").GetComponent<PlatformCameraControl> ().SetCameraLimits (cameraLimits);

		anim = GetComponent<Animator> ();
		player = GameObject.Find ("Character");
		rigidBody = GetComponent<Rigidbody2D> ();
		Invoke ("BossHealthDelay", 1);
	}

	void BossHealthDelay()
	{
		BossHealth.EnableBossBar("Werewolf", maxHealth);
	}
	
	// Update is called once per frame
	void Update () {
		if (curHealth <= 0) {
			dead = true;
			anim.SetBool ("isDead", true);
		}

		if (!dead && !attacking) {
			if (transform.position.x > player.transform.position.x) {
				transform.localScale = new Vector3 (1, 1, 1);
				rigidBody.velocity = new Vector2 (-speed, rigidBody.velocity.y);
			} else {
				transform.localScale = new Vector3 (-1, 1, 1);
				rigidBody.velocity = new Vector2 (speed, rigidBody.velocity.y);
			}	
			if (Vector2.Distance (transform.position, player.transform.position) < 1)
			{
				attacking = true;
			}
		} else if (!dead && attacking && !runOnce) {
			anim.SetBool ("isAttacking", true);
			rigidBody.velocity = new Vector2 (0, rigidBody.velocity.y);
			if (!runOnce)
			{
				runOnce = true;
				Invoke ("BackToIdle", 2.3f);
			}
		}

		if (dead && !runOnce) {
			runOnce = true;
			anim.SetBool ("isDead", true);
			Invoke ("Decay", 1.5f);
		}
	}

	void BackToIdle()
	{
		runOnce = false;
		if (Vector2.Distance (transform.position, player.transform.position) < 1) {
			attacking = true;
		} else {
			anim.SetBool ("isAttacking", false);
			attacking = false;
		}
	}

	void Decay()
	{
		YrouverMother.CompleteMileStone (1, 0);
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "PlayerWeapon") {
			curHealth -= 50;
			BossHealth.DamageDealt(50);
			Destroy (collider.gameObject);
		}
	}
}
