using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	float speed = 5f;
	float move;
	float moveVertical;
	Rigidbody2D rigidBody;
	bool facingLeft = false;
	Animator anim;

	public Transform groundCheck;
	public LayerMask whatIsGround;
	float groundRadius = .1f;
	bool grounded = false;

	GameObject rightLight;
	GameObject leftLight;
	bool lightsEnabled = false;

	float maxOxygen = 1000;
	float curOxygen = 1000;
	float maxHealth = 1000;
	float curHealth = 1000;
	float maxBattery = 1000;
	float curBattery = 1000;

	GameObject damageTaken;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		rightLight = transform.FindChild ("LightRight").gameObject;
		leftLight = transform.FindChild ("LightLeft").gameObject;
		rightLight.SetActive (false);
		leftLight.SetActive (false);
		damageTaken = GameObject.Find ("DamageTaken");
		damageTaken.SetActive (false);

		InvokeRepeating ("ResourceReduce", 0, 1);
	}

	public float Health
	{
		get { return curHealth;}
		set { curHealth = value; }
	}
	public float Oxygen
	{
		get { return curOxygen;}
		set { curOxygen = value; }
	}
	public float Battery
	{
		get { return curBattery;}
		set { curBattery = value; }
	}
	
	// Update is called once per frame
	void Update () {
		//Controls player movement
		if (Input.GetButton ("Action")) {
			anim.SetBool ("underAction", true);
			rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
		} else {
			anim.SetBool ("underAction", false);
			move = Input.GetAxis ("Horizontal");
			if (move != 0) {
				anim.SetBool ("isMoving", true);
			} else {
				anim.SetBool ("isMoving", false);
			}
			if (Physics2D.gravity.y == 0)
			{
				moveVertical = Input.GetAxis ("Vertical");
				rigidBody.velocity = new Vector2 (move * speed / 2, moveVertical * speed / 2);
			} 
			else 
			{
				rigidBody.velocity = new Vector2 (move * speed, rigidBody.velocity.y);
			}
			if (Physics2D.gravity.y > 0)
			{
				transform.localScale = new Vector3(transform.localScale.x, -1, 1);
			}
			else
			{
				transform.localScale = new Vector3(transform.localScale.x, 1, 1);
			}

			
			grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
			if (grounded && Input.GetButtonDown ("Jump"))
			{
				if (Physics2D.gravity.y < 0)
				{
					rigidBody.velocity = new Vector2 (rigidBody.velocity.x, speed);
				}
				else
				{
					rigidBody.velocity = new Vector2 (rigidBody.velocity.x, -speed);
				}
			}
			
			//Controls move player is facing
			if (move > 0) {
				facingLeft = false;
			} else if (move < 0) {
				facingLeft = true;
			}
			if (facingLeft) {
				if (lightsEnabled && curBattery > 0)
				{
					leftLight.SetActive(true);
					rightLight.SetActive (false);
				}
				transform.localScale = new Vector3 (-1, transform.localScale.y, 1);
			} else {
				if (lightsEnabled && curBattery > 0)
				{
					leftLight.SetActive(false);
					rightLight.SetActive (true);
				}
				transform.localScale = new Vector3 (1, transform.localScale.y, 1);
			}
			if (lightsEnabled && curBattery <= 0)
			{
				leftLight.SetActive(false);
				rightLight.SetActive (false);
			}

			//Laser Control
			if (Input.GetButtonDown("Fire1") && curBattery >= 20)
			{
				Instantiate (Resources.Load("greenLaserRay"));
			}
		}
	}

	void ResourceReduce()
	{
		curOxygen -= 10;
		if (curOxygen <= 0) {
			PlayerDeath ();
		}
		if (lightsEnabled && curBattery > 0) {
			curBattery -= 10;
		}
	}

	public void DamagePlayer(float amount)
	{
		curHealth -= amount;
		if (curHealth <= 0) {
			PlayerDeath ();
		}
		damageTaken.SetActive (true);
		Invoke ("ResetDamageTaken", .25f);
	}

	void ResetDamageTaken()
	{
		damageTaken.SetActive (false);
	}

	void PlayerDeath()
	{
		//do something here
	}

	public void EnableLights()
	{
		lightsEnabled = true;
	}
}
