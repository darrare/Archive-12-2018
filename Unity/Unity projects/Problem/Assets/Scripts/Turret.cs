using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Turret : MonoBehaviour {
	enum CombatState {IDLE, INCOMBAT, EXITINGCOMBAT};
	CombatState combatState = CombatState.IDLE;
	Transform target;
	GameObject turret;
	float angleDifference = 90;
	float defaultAngle;
	Vector3 angle;

	float timer = 0;
	public float shootInterval = .5f;

	// Use this for initialization
	void Start () {
		turret = transform.GetChild (0).gameObject;
		defaultAngle = transform.eulerAngles.z;
		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("LevelBuilder")) {
			target = CONSTANTS.levelController.player.transform;
			combatState = CombatState.INCOMBAT;
		}

	}

	public float FireRate
	{
		get { return shootInterval; }
		set { shootInterval = value; }
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            combatState = CombatState.EXITINGCOMBAT;
			if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("LevelBuilder")) {
				target = CONSTANTS.levelController.player.transform;
				combatState = CombatState.INCOMBAT;
			}
        }
		if (combatState == CombatState.IDLE) {
			turret.transform.eulerAngles = new Vector3 (0, 0, (Mathf.PingPong (Time.time, 2) - 1) * angleDifference + defaultAngle);
		} else if (combatState == CombatState.INCOMBAT) {
			angle = new Vector3(0, 0, -Mathf.Atan2 (target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y) * Mathf.Rad2Deg + 90);
//			if (angle.z < 0) {
//				angle = new Vector3 (0, 0, angle.z + 360);
//			}
			turret.transform.eulerAngles = angle;//Vector3.Lerp (turret.transform.eulerAngles, angle, Time.deltaTime * 10);
			timer += Time.deltaTime;
			if (timer >= shootInterval) {
				Shoot ();
				timer = 0;
			}
		} else if (combatState == CombatState.EXITINGCOMBAT) {
			if (Mathf.Abs(((Mathf.PingPong (Time.time, 2) - 1) * angleDifference + defaultAngle) - turret.transform.eulerAngles.z) <= 2)
			{
				combatState = CombatState.IDLE;
			}
		}
	}

	void Shoot()
	{
		GameObject laser = Instantiate (Resources.Load ("Laser"), transform.GetChild (0).GetChild(0).transform.position, Quaternion.identity) as GameObject;
		laser.transform.eulerAngles = turret.transform.eulerAngles;
		laser.GetComponent<Laser> ().SetOwner ("Enemy");
		Vector2 direction = target.transform.position - turret.transform.position;
		direction.Normalize ();
		direction *= 15;
		laser.GetComponent<Rigidbody2D> ().velocity = direction;
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			combatState = CombatState.INCOMBAT;
			target = collider.gameObject.transform;
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("LevelBuilder")) {
			return;
		}
		if (collider.tag == "Player") {
			combatState = CombatState.EXITINGCOMBAT;
			target = null;
		}
	}
}
