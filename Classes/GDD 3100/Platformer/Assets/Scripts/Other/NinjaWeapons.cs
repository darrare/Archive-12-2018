using UnityEngine;
using System.Collections;

public class NinjaWeapons : MonoBehaviour {
	//kunai variables
	bool kunaiOnCooldown = false;
	float kunaiCooldownTimer = .60f;
	//float kunaiDamage = 25f;

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Fire1") > 0 && !kunaiOnCooldown) {
			CreateKunai ();
		}
	}

	void CreateKunai()
	{
		anim.SetBool ("throwAttack", true);
		kunaiOnCooldown = true;
		Instantiate (Resources.Load ("Kunai"));
		Invoke ("KunaiCooldownReset", kunaiCooldownTimer);
	}

	void KunaiCooldownReset()
	{
		anim.SetBool ("throwAttack", false);
		kunaiOnCooldown = false;
	}
}
