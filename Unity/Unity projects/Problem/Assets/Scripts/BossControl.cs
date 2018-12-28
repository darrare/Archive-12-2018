using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BossControl : MonoBehaviour {
	public float bossMaxHealth = 5000;
	float bossHealth = 5000;
	float healthBarOrigin;
	public RectTransform healthBar;
	public GameObject bossHeathBar;
	AudioSource source;
	public AudioClip bgm;

	GameObject player;

	public GameObject doors;
	bool isPlayerInRoom = false;

	enum BossState { NULL, IDLE, CASTINGFIRE, CASTINGBEAMS, RESET, DIEING };
	BossState bossState = BossState.NULL;

	enum FireState { EYESGROWING, FIRESPAWNING, IDLE, EYESSHRINKING };
	FireState fireState = FireState.EYESGROWING;

	enum BeamState { INDICATORBEAMS, IDLE, FULLBEAMS };
	BeamState beamState = BeamState.INDICATORBEAMS;

	List<GameObject> indicatorBeams = new List<GameObject>();
	List<int> indicatorPositions = new List<int>();
	int newIndicatorPosition;

	public GameObject leftEye, rightEye;
	GameObject rocket1, rocket2;
	SpriteRenderer spriteRenderer;
	float colorTimer = 0;

	float fireSpawnRangeX = 10f;
	float fireSpawnDifferenceY = 9f;
	float timer = 0;
	float subTimer = 0;
	float timeToIdle = 1f;
	float timeToChangeEyes = 2f;
	float durationToSpawnFire = 7f;
	float fireSpawnInterval = .25f;
	float timeToIdleForFire = 1f;
	float durationToSpawnIndicatorBeams = 5f;
	float indicatorBeamsSpawnInterval = .75f;
	float timeToIdleForBeams = 1f;
	float durationForFullBeams = 2f;


	// Use this for initialization
	void Start () {
		bossHealth = bossMaxHealth;
		healthBarOrigin = healthBar.anchoredPosition.x;
		leftEye.transform.localScale = new Vector3 (0, 0, 1);
		rightEye.transform.localScale = new Vector3 (0, 0, 1);
		spriteRenderer = GetComponent<SpriteRenderer> ();
	    spriteRenderer.color = Color.clear;
		bossHeathBar.SetActive (false);
		doors.SetActive (false);
		source = GetComponent<AudioSource> ();
		CONSTANTS.backgroundMusic.PlayOneShot (bgm);
	}
	
	// Update is called once per frame
	void Update () {
		switch (bossState) {
		case BossState.NULL:
			if (isPlayerInRoom) {
				doors.SetActive (true);
				timer += Time.deltaTime;
				if (!source.isPlaying) {
					source.Play ();
				}
				spriteRenderer.color = Color.Lerp (Color.clear, Color.white, timer);
				if (timer >= 1) {
					rocket1 = Instantiate (Resources.Load ("HomingRocket"), new Vector2 (transform.position.x + 10, transform.position.y + 10), Quaternion.identity) as GameObject;
					rocket2 = Instantiate (Resources.Load ("HomingRocket"), new Vector2 (transform.position.x - 10, transform.position.y + 10), Quaternion.identity) as GameObject;
					bossState = BossState.IDLE;
					bossHeathBar.SetActive (true);
					HealthBarControl ();
					timer = 0;
				}
			}
			break;
		case BossState.IDLE:
			timer += Time.deltaTime / timeToIdle;
			if (timer >= 1) {
				fireState = FireState.EYESGROWING;
				beamState = BeamState.INDICATORBEAMS;
				bossState = PickAttackMethod ();
				timer = 0;
			}
			break;
		case BossState.CASTINGFIRE:
			switch (fireState) {
			case FireState.EYESGROWING:
				timer += Time.deltaTime / timeToChangeEyes;
				leftEye.transform.localScale = new Vector3 (Mathf.Lerp (0, .75f, timer), Mathf.Lerp (0, .75f, timer), 1);
				rightEye.transform.localScale = new Vector3 (Mathf.Lerp (0, .75f, timer), Mathf.Lerp (0, .75f, timer), 1);
				if (timer >= 1) {
					fireState = FireState.FIRESPAWNING;
					timer = 0;
				}
				break;
			case FireState.FIRESPAWNING:
				timer += Time.deltaTime / durationToSpawnFire;
				subTimer += Time.deltaTime / fireSpawnInterval;
				if (subTimer >= 1) {
					subTimer = 0;
					Instantiate (Resources.Load ("BossFireBall"), transform.position + (Vector3.up * fireSpawnDifferenceY) + (Vector3.right * Random.Range ((int)-fireSpawnRangeX, (int)fireSpawnRangeX + 1)), Quaternion.identity);
				}
				if (timer >= 1) {
					fireState = FireState.IDLE;
					timer = 0;
				}
				break;
			case FireState.IDLE:
				timer += Time.deltaTime / timeToIdleForFire;
				if (timer >= 1) {
					fireState = FireState.EYESSHRINKING;
					timer = 0;
				}
				break;
			case FireState.EYESSHRINKING:
				timer += Time.deltaTime / timeToIdleForFire;
				leftEye.transform.localScale = new Vector3 (Mathf.Lerp (.75f, 0, timer), Mathf.Lerp (.75f, 0, timer), 1);
				rightEye.transform.localScale = new Vector3 (Mathf.Lerp (.75f, 0, timer), Mathf.Lerp (.75f, 0, timer), 1);
				if (timer >= 1) {
					bossState = BossState.IDLE;
					timer = 0;
				}
				break;
			}
			break;
		case BossState.CASTINGBEAMS:
			switch (beamState) {
			case BeamState.INDICATORBEAMS:
				timer += Time.deltaTime / durationToSpawnIndicatorBeams;
				subTimer += Time.deltaTime / indicatorBeamsSpawnInterval;
				if (subTimer >= 1) {
					subTimer = 0;
					if (indicatorPositions.Contains ((int)player.transform.position.x)) {
						newIndicatorPosition = (int)player.transform.position.x;
						int index = 3;
						while (true) {
							if (!indicatorPositions.Contains (newIndicatorPosition + index) && newIndicatorPosition + index < transform.position.x + fireSpawnRangeX) {
								newIndicatorPosition += index;
								break;
							} else if (!indicatorPositions.Contains(newIndicatorPosition - index) && newIndicatorPosition - index > transform.position.x - fireSpawnRangeX){
								newIndicatorPosition -= index;
								break;
							}
							index += 3;
						}
					} else {
						newIndicatorPosition = (int)player.transform.position.x;
					}
					indicatorBeams.Add(Instantiate (Resources.Load ("IndicatorBeam"), (Vector3.right * newIndicatorPosition) + (Vector3.up * transform.position.y), Quaternion.identity) as GameObject);
					indicatorPositions.Add(newIndicatorPosition);
				}
				if (timer >= 1) {
					beamState = BeamState.IDLE;
					timer = 0;
				}
				break;
			case BeamState.IDLE:
				timer += Time.deltaTime / timeToIdleForBeams;
				foreach (GameObject obj in indicatorBeams) {
					obj.GetComponent<IndicatorBeams> ().Deactivate ();
				}
				if (timer >= 1) {
					beamState = BeamState.FULLBEAMS;
					timer = 0;
				}
				break;
			case BeamState.FULLBEAMS:
				timer += Time.deltaTime / durationForFullBeams;
				foreach (GameObject obj in indicatorBeams) {
					Instantiate (Resources.Load ("FullBeam"), obj.transform.position, Quaternion.identity);
					Destroy (obj.gameObject);
				}
				indicatorBeams.Clear ();
				indicatorPositions.Clear ();
				if (timer >= 1) {
					bossState = BossState.IDLE;
					timer = 0;
				}
				break;
			}
			break;
		case BossState.RESET:
			foreach (GameObject obj in indicatorBeams) {
				Destroy (obj.gameObject);
			}
			indicatorBeams.Clear ();
			doors.SetActive (false);
			timer = 0;
			bossHeathBar.SetActive (false);
			isPlayerInRoom = false;
			bossHealth = bossMaxHealth;
			leftEye.transform.localScale = new Vector3 (0, 0, 1);
			rightEye.transform.localScale = new Vector3 (0, 0, 1);

			spriteRenderer.color = Color.clear;
			bossState = BossState.NULL;
			break;
		case BossState.DIEING:
			GetComponent<BoxCollider2D> ().enabled = false;
			Destroy (rocket1);
			Destroy (rocket2);
			timer += Time.deltaTime / 5;
			subTimer += Time.deltaTime * 5;
			if (subTimer >= 1) {
				subTimer = 0;
				Instantiate (Resources.Load ("Explosion"), new Vector3 (Random.Range (transform.position.x - 4f, transform.position.x + 4f), Random.Range (transform.position.y - 4f, transform.position.y + 4f)), Quaternion.identity);
			}
			if (timer >= 1) {
				doors.SetActive (false);

				Destroy (gameObject);
			}
			break;
		}
		if (bossState != BossState.NULL && spriteRenderer.color != Color.white) {
			colorTimer += Time.deltaTime * 10;
			spriteRenderer.color = Color.Lerp (Color.white - Color.black * .5f, Color.white, colorTimer);
		} else {
			colorTimer = 0;
		}
	}

	BossState PickAttackMethod()
	{
		return (BossState)Random.Range (2, 4); 
	}

	public void ResetBoss()
	{
		bossState = BossState.RESET;
	}

	public void PlayerEnteredRoom(GameObject player)
	{
		isPlayerInRoom = true;
		this.player = player;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Laser") {
			TakeDamage ();
			Destroy (collider.gameObject);
		}
	}

	void TakeDamage()
	{
		bossHealth -= 50;
		spriteRenderer.color = Color.white - Color.black * .5f;
		HealthBarControl ();
		if (bossHealth <= 0) {
			ReadyForBossDeath ();
		}
	}

	void HealthBarControl()
	{
		healthBar.anchoredPosition = new Vector2 (healthBarOrigin + (healthBar.rect.width * (bossHealth / bossMaxHealth)), healthBar.anchoredPosition.y);
	}

	void ReadyForBossDeath()
	{
		leftEye.transform.localScale = new Vector3 (0, 0, 1);
		rightEye.transform.localScale = new Vector3 (0, 0, 1);
		timer = 0;
		foreach (GameObject obj in indicatorBeams) {
			Destroy (obj.gameObject);
		}
		indicatorBeams.Clear ();
		bossHeathBar.SetActive (false);
		subTimer = 0;
		spriteRenderer.color = Color.white;
		bossState = BossState.DIEING;
	}
}
