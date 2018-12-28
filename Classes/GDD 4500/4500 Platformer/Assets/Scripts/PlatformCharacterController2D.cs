using UnityEngine;
using System.Collections;

public class PlatformCharacterController2D : MonoBehaviour {
	private bool grounded = true;
	private float jumpForceInitial = 250f;
	private float jumpForceBonus = 20f;
	private float numberUpdatesJumpForce = 0;
	private float numberUpdatesJumpForceMax = 10;
	public Transform groundCheck;
	public Transform groundCheck1;
	private float groundRadius = .05f;
	public LayerMask whatIsGround;
	private Collider2D triggerCheck;
	private Rigidbody2D rigidBody;
	private Animator anim;
	private bool facingRight = true;
	private Vector3 spawnLocation;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		spawnLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float move = Input.GetAxis ("Horizontal");
		rigidBody.velocity = new Vector2 (move * 8, rigidBody.velocity.y);

		//grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		triggerCheck = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		if (triggerCheck == null) {
			triggerCheck = Physics2D.OverlapCircle (groundCheck1.position, groundRadius, whatIsGround);
		}

		if (triggerCheck != null && triggerCheck.isTrigger == true) {
			grounded = false;
		} else if (triggerCheck != null) {
			grounded = true;
		} else {
			grounded = false;
		}

		triggerCheck = null;

		if (grounded && Input.GetButtonDown ("Jump")) {
			numberUpdatesJumpForce = 0;
			rigidBody.AddForce (new Vector2 (0, jumpForceInitial));
		} else if (Input.GetButton ("Jump") && numberUpdatesJumpForce <= numberUpdatesJumpForceMax) {
			rigidBody.AddForce (new Vector2 (0, jumpForceBonus));
			numberUpdatesJumpForce++;
		}

		anim.SetBool("ground", grounded);
		anim.SetFloat ("speed", Mathf.Abs (move));

		if (move > 0 && !facingRight) {
			Flip ();
		} 
		else if (move < 0 && facingRight) {
			Flip ();
		}
	}
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Respawn()
	{
		transform.position = spawnLocation;
		rigidBody.velocity = Vector3.zero;
		GameObject.Find ("Timer").GetComponent<Timer> ().ResetTimer ();
	}
}
