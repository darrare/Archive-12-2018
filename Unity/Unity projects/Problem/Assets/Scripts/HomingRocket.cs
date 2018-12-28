using UnityEngine;
using System.Collections;

public class HomingRocket : MonoBehaviour {
	GameObject target;

	public bool randomizeValues = true;
	float turnSpeedMin = 5;
	float turnSpeedMax = 8;
	float chaseSpeedMin = 20;
	float chaseSpeedMax = 25;
	float accelerationModifierMin = 2500;
	float accelerationModifierMax = 3000;


	public float turnSpeed = 5f;
	public float chaseSpeed = 25f;
	float speed;
	Vector2 velocity;
	Vector2 targetVelocity;
	Vector2 curveAmount;
	public float accelerationModifier = 2000;
	float speedLerpModifier = 1f; 
	float curveRatio;

	// Use this for initialization
	void Awake () {
		target = GameObject.Find ("Character");
		if (randomizeValues) {
			turnSpeed = Random.Range (turnSpeedMin, turnSpeedMax);
			chaseSpeed = Random.Range (chaseSpeedMin, chaseSpeedMax);
			accelerationModifier = Random.Range (accelerationModifierMin, accelerationModifierMax);
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
		}
		transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(GetComponent<Rigidbody2D> ().velocity.y, GetComponent<Rigidbody2D> ().velocity.x) * Mathf.Rad2Deg);


		if (target != null) {
			targetVelocity = target.transform.position - transform.position;
			targetVelocity.Normalize ();
		}
		velocity = GetComponent<Rigidbody2D> ().velocity;
		velocity += velocity * Time.deltaTime + .5f * (targetVelocity * accelerationModifier) * Time.deltaTime * Time.deltaTime;
		velocity.Normalize ();


		curveAmount = velocity - targetVelocity;
		curveRatio = Mathf.Abs (curveAmount.magnitude);


		if (curveRatio >= .3f) {
			//speed = turnSpeed;
			speed = Mathf.Lerp (speed, turnSpeed, Time.deltaTime * speedLerpModifier);
		} else {
			speed = Mathf.Lerp (speed, chaseSpeed, Time.deltaTime * speedLerpModifier);
		}
		GetComponent<Rigidbody2D> ().velocity = velocity * speed;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") {
			collider.GetComponent<PlayerController> ().KillPlayer ();
			GameObject newExplosion = Instantiate (Resources.Load ("Explosion"), transform.position, Quaternion.identity) as GameObject;
			newExplosion.transform.localScale *= 3;
			Destroy (gameObject);
		}
	}
}
