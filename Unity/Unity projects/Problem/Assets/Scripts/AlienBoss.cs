using UnityEngine;
using System.Collections;

public class AlienBoss : MonoBehaviour {
	public float bossMaxHealth = 5000;
	public Vector2 leftPosition, rightPosition;
	float bossHealth = 5000;
	float healthBarOrigin;
	public RectTransform healthBar;
	public GameObject bossHeathBar;
	AudioSource source;
	public AudioSource weaponSource;
	public AudioClip bgm;
	public AudioClip chargeUp;
	public GameObject doors;
	public GameObject player;
	bool isPlayerInRoom = false;
	SpriteRenderer spriteRenderer;
	bool wasPreviouslyLeft = true;
	public TNTCannon tntCannonLeft, tntCannonRight;
	public GameObject destroyWhenDead;

	enum BossState { NULL, IDLERIGHT, IDLELEFT, SHOOTING, RESET, DIEING };
	BossState bossState = BossState.NULL;

	float timer = 0;
	float subTimer = 0;
	float angle = 0;
	Vector2 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		bossHealth = bossMaxHealth;
		healthBarOrigin = healthBar.anchoredPosition.x;
		bossHeathBar.SetActive (false);
		doors.SetActive (false);
		source = GetComponent<AudioSource> ();
		CONSTANTS.backgroundMusic.PlayOneShot (bgm);
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.color = Color.clear;
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
					bossState = BossState.IDLERIGHT;
					bossHeathBar.SetActive (true);
					HealthBarControl ();
					timer = 0;
				}
			}

			break;
		case BossState.IDLERIGHT:
			transform.eulerAngles = Vector3.zero;
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, rightPosition, timer / 5);
			transform.localScale = new Vector3 (-1, 1, 1);
			if (timer >= 1.5f) {
				bossState = BossState.SHOOTING;
				transform.localScale = new Vector3 (1, 1, 1);
				wasPreviouslyLeft = false;
				timer = 0;
			}
			break;
		case BossState.IDLELEFT:
			transform.eulerAngles = Vector3.zero;
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, leftPosition, timer / 5);
			transform.localScale = new Vector3 (1, 1, 1);
			if (timer >= 1.5f) {
				bossState = BossState.SHOOTING;
				transform.localScale = new Vector3 (-1, 1, 1);
				wasPreviouslyLeft = true;
				timer = 0;
			}
			break;
		case BossState.SHOOTING:
			if (timer == 0) {
				weaponSource.PlayOneShot (chargeUp);
				if (!wasPreviouslyLeft) {
					GameObject particles = Instantiate (Resources.Load ("AlienBossLaserCharge"), transform.GetChild(0).transform.position + new Vector3(-2, -2, -1), Quaternion.identity) as GameObject;
					particles.transform.SetParent (destroyWhenDead.transform);
				} else {
					GameObject particles = Instantiate (Resources.Load ("AlienBossLaserCharge"), transform.GetChild(0).transform.position + new Vector3(2, -2, -1), Quaternion.identity) as GameObject;
					particles.transform.localScale = new Vector3 (-1, 1, 1);
					particles.transform.SetParent (destroyWhenDead.transform);
				}

			}

			timer += Time.deltaTime;
			if (player != null)
				FacePlayer ();
		
			if (timer >= 8) {
				if (wasPreviouslyLeft) {
					bossState = BossState.IDLERIGHT;
				} else {
					bossState = BossState.IDLELEFT;
				}
				timer = 0;
				subTimer = 0;
			}
			else if (timer >= 4) {
				subTimer += Time.deltaTime;
				if (subTimer >= .5f) {
					GameObject laser = Instantiate (Resources.Load ("BossLaser"), transform.GetChild (0).transform.position, Quaternion.identity) as GameObject;
					laser.transform.eulerAngles = transform.eulerAngles;
					laser.transform.localScale = new Vector3 (3, 3, 3);
					laser.transform.SetParent (destroyWhenDead.transform);
					Vector2 direction = player.transform.position - transform.position;
					direction.Normalize ();
					direction *= 15;
					laser.GetComponent<Rigidbody2D> ().velocity = direction;

					subTimer = 0;
				}
			}
			break;
		case BossState.RESET:
			doors.SetActive (false);
			timer = 0;
			bossHeathBar.SetActive (false);
			isPlayerInRoom = false;
			bossHealth = bossMaxHealth;
			bossState = BossState.NULL;
			transform.position = startPosition;
			transform.localScale = new Vector3 (1, 1, 1);
			spriteRenderer.color = Color.clear;
			foreach (Transform child in destroyWhenDead.transform) {
				Destroy (child.gameObject);
			}
			break;
		case BossState.DIEING:
			GetComponent<PolygonCollider2D> ().enabled = false;
			tntCannonLeft.CanShoot = false;
			tntCannonRight.CanShoot = false;
			foreach (Transform child in destroyWhenDead.transform) {
				Destroy (child.gameObject);
			}
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
	}


	public void ResetBoss()
	{
		bossState = BossState.RESET;
		tntCannonLeft.CanShoot = false;
		tntCannonRight.CanShoot = false;
	}

	public void PlayerEnteredRoom(GameObject player)
	{
		this.player = player;
		isPlayerInRoom = true;
		tntCannonLeft.CanShoot = true;
		tntCannonRight.CanShoot = true;
	}

	void TakeDamage()
	{
		bossHealth -= 50;
		HealthBarControl ();
		if (bossHealth <= 0) {
			ReadyForBossDeath ();
		}
	}

	void ReadyForBossDeath()
	{
		bossHeathBar.SetActive (false);
		timer = 0;
		subTimer = 0;
		bossState = BossState.DIEING;
	}

	void HealthBarControl()
	{
		healthBar.anchoredPosition = new Vector2 (healthBarOrigin + (healthBar.rect.width * (bossHealth / bossMaxHealth)), healthBar.anchoredPosition.y);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Laser") {
			TakeDamage ();
			Destroy (collider.gameObject);
		}
	}

	void FacePlayer()
	{
		if (wasPreviouslyLeft) {
			angle = -Mathf.Atan2 (player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y) * Mathf.Rad2Deg + 90;
		} else {
			angle = -Mathf.Atan2 (player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y) * Mathf.Rad2Deg - 90;
		}

		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, angle);
	}
}
