using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	enum MobilityDirection { MOVELEFT, STAYSTILL, MOVERIGHT };
	MobilityDirection mobilityDirection;
	enum CombatStatus { INCOMBAT, IDLECOMBAT };
	CombatStatus combatStatus = CombatStatus.IDLECOMBAT;

	Rigidbody2D rBody;
	float walkSpeed = 4;
	float runSpeed = 7;
	float combatRange = 5f;
	float jumpVelocity = 12f;
	bool isFacingRight = false;

	Transform target;
	bool isFacingWall = false;
	RaycastHit2D rayCastUp;
	RaycastHit2D rayCastDown;
	float combatTimer = 0;
	float combatResetTime = 5;

	float timer = .98f;
	float shootDelay = 1f;

	int random;
	Vector2 origin;

	GameObject aggroIcon;
	Animator anim;

	// Use this for initialization
	void Start () {
		RandomlyChangeDirection ();
		rBody = GetComponent<Rigidbody2D> ();
		origin = transform.position;
		aggroIcon = transform.GetChild (1).gameObject;
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		isFacingWall = Physics2D.Linecast (transform.position, transform.position - Vector3.right * transform.localScale.x, 1 << LayerMask.NameToLayer ("SolidGround"));
		if (isFacingWall) {
			if (mobilityDirection == MobilityDirection.MOVELEFT) {
				mobilityDirection = MobilityDirection.MOVERIGHT;
			} else if (mobilityDirection == MobilityDirection.MOVERIGHT) {
				mobilityDirection = MobilityDirection.MOVELEFT;
			}
		}

		if (target == null) {
			combatStatus = CombatStatus.IDLECOMBAT;
		}
			
		rayCastUp = Physics2D.Linecast (transform.position + (Vector3.right * transform.localScale.x * 10) - Vector3.up * 2, transform.position - (Vector3.right * transform.localScale.x * 10) + Vector3.up * 2, 1 << LayerMask.NameToLayer ("Player"));
		rayCastDown = Physics2D.Linecast (transform.position + (Vector3.right * transform.localScale.x * 10) - Vector3.up * 2, transform.position - (Vector3.right * transform.localScale.x * 10) - Vector3.up * 2, 1 << LayerMask.NameToLayer ("Player"));
		if (rayCastUp) {
			target = rayCastUp.collider.transform;
			combatTimer = 0;
			combatStatus = CombatStatus.INCOMBAT;
		} else if (rayCastDown) {
			target = rayCastDown.collider.transform;
			combatTimer = 0;
			combatStatus = CombatStatus.INCOMBAT;
		} else if (combatTimer >= combatResetTime) {
			combatStatus = CombatStatus.IDLECOMBAT;
		} else {
			combatTimer += Time.deltaTime;
		}

		if (combatStatus == CombatStatus.IDLECOMBAT) {
			aggroIcon.SetActive (false);
			switch (mobilityDirection) {
			case MobilityDirection.MOVELEFT:
				rBody.velocity = new Vector2 (-walkSpeed, rBody.velocity.y);
				break;
			case MobilityDirection.STAYSTILL:
				rBody.velocity = Vector2.zero;
				break;
			case MobilityDirection.MOVERIGHT:
				rBody.velocity = new Vector2 (walkSpeed, rBody.velocity.y);
				break;
			}
		} else if (combatStatus == CombatStatus.INCOMBAT) {
			aggroIcon.SetActive (true);
			if (Mathf.Abs (transform.position.x - target.position.x) > combatRange) {
				if (transform.position.x > target.position.x) {
					rBody.velocity = new Vector2 (-runSpeed, rBody.velocity.y);
				} else {
					rBody.velocity = new Vector2 (runSpeed, rBody.velocity.y);
				}
			} else {
				rBody.velocity = new Vector2 (0, rBody.velocity.y);
				if (transform.position.x > target.position.x) {
					isFacingRight = false;
				} else {
					isFacingRight = true;
				}

				if (rBody.velocity.y == 0) {
					rBody.velocity = new Vector2 (rBody.velocity.x, jumpVelocity);
				}
			}
			timer += Time.deltaTime;
			if (timer >= shootDelay) {
				timer = 0;
				ShootLaser ();
			}
		}
		if (rBody.velocity.x > 0) {
			isFacingRight = true;
		} else if (rBody.velocity.x < 0) {
			isFacingRight = false;
		}

		if (isFacingRight) {
			transform.localScale = new Vector2 (-1, 1);
		} else if (!isFacingRight){
			transform.localScale = new Vector2 (1, 1);
		}


		if (rBody.velocity.y != 0) {
			anim.SetInteger ("gameState", 2);
		} else if (rBody.velocity.x != 0) {
			anim.SetInteger ("gameState", 1);
		} else {
			anim.SetInteger ("gameState", 0);
		}
	}

	void RandomlyChangeDirection()
	{
		random = Random.Range (0, 3);
		if (random == 0) {
			mobilityDirection = MobilityDirection.MOVELEFT;
		} else if (random == 1) {
			mobilityDirection = MobilityDirection.STAYSTILL;
		} else if (random == 2) {
			mobilityDirection = MobilityDirection.MOVERIGHT;
		}
		Invoke ("RandomlyChangeDirection", Random.Range (1f, 3f));
	}

	void ShootLaser()
	{
		GameObject laser = Instantiate (Resources.Load ("Laser"), transform.GetChild (0).transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Laser> ().SetOwner ("Enemy");
		if (isFacingRight) {
			laser.GetComponent<Rigidbody2D> ().velocity = new Vector2 (10, 0);
		} else {
			laser.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-10, 0);
		}
	}

	public void KillEnemy()
	{
		Instantiate (Resources.Load ("Explosion"), transform.position, Quaternion.identity);
		CONSTANTS.levelController.AddItemToRespawnList ("Enemy", origin, transform.rotation);
		Destroy(gameObject);
	}
}
