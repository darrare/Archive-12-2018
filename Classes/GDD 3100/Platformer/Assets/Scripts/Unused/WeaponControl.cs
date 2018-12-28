using UnityEngine;
using System.Collections;

public class WeaponControl : MonoBehaviour {
	bool gustOnCooldown = false;
	bool fireWaveOnCooldown = false;
	int fireWaveCount = 1;
	float fireWaveOffset = 1f;
	GameObject fireWaveBase;
	Animator playerAnimator;
	GameObject player;
	bool canFireWave = false;
	ParticleSystem particles;

	// Use this for initialization
	void Start () {
		particles = GameObject.Find ("handGlow").GetComponent<ParticleSystem> ();
		player = GameObject.Find ("Character");
		fireWaveBase = new GameObject();
		playerAnimator = GameObject.Find ("Character").GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gustOnCooldown && Input.GetAxis ("Fire1") > 0) {
			particles.startColor = new Color(0, 0, 1);
			Instantiate(Resources.Load ("WindGust"));
			gustOnCooldown = true;
			Invoke ("GustCooldownReset", 1.5f);
			Invoke ("CastingAnimationReset", .5f);
			playerAnimator.SetBool("casting", true);
		}
		if (canFireWave && !fireWaveOnCooldown && Input.GetAxis ("Fire2") > 0) {
			particles.startColor = new Color(1, 0, 0);
			fireWaveBase.transform.position = player.transform.position;
			fireWaveOnCooldown = true;
			InvokeRepeating ("FireWaveCycle", 0, .1f);
			Invoke ("FireWaveCooldownReset", 3f);
			Invoke ("CastingAnimationReset", .5f);
			playerAnimator.SetBool("casting", true);
		}
	}

	public void EnableFireWave()
	{
		canFireWave = true;
	}

	void CastingAnimationReset()
	{
		playerAnimator.SetBool("casting", false);
	}

	void GustCooldownReset()
	{
		gustOnCooldown = false;
	}

	void FireWaveCooldownReset()
	{
		fireWaveOnCooldown = false;
	}

	void FireWaveCycle()
	{
		GameObject fireWave = Instantiate (Resources.Load ("FireWave")) as GameObject;
		fireWave.transform.parent = fireWaveBase.transform;
		fireWave.transform.localPosition = new Vector3(-fireWaveOffset * fireWaveCount, -.59f);

		GameObject fireWave2 = Instantiate (Resources.Load ("FireWave")) as GameObject;
		fireWave2.transform.parent = fireWaveBase.transform;
		fireWave2.transform.localPosition = new Vector3(fireWaveOffset * fireWaveCount, -.59f);

		fireWaveCount++;
		if (fireWaveCount == 10) {
			fireWaveCount = 1;
			CancelInvoke ("FireWaveCycle");
		}
	}
}
