using UnityEngine;
using System.Collections;

public class PlatformCharacterController2D : MonoBehaviour {
	private bool grounded = true;
	private float jumpForceInitial = 200f;
	private float jumpForceBonus = 20f;
	private float numberUpdatesJumpForce = 0;
	private float numberUpdatesJumpForceMax = 10;
	public Transform groundCheck;
	private float groundRadius = .05f;
	public LayerMask whatIsGround;
	private Collider2D triggerCheck;
	private Rigidbody2D rigidBody;
	private Animator anim;
	private bool facingRight = true;
	private int bandsFound = 0;
	private int enemiesDestroyed = 0;
	private Vector2 spawnLocation;

	Vector2 respawnLocation;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		if (GAMECONSTANTS.playerMapPosition.x != 9999) {
			transform.position = GAMECONSTANTS.playerMapPosition;
		}
		if (GAMECONSTANTS.playerLocation [0] != 9999) {
			Invoke ("LoadPosition", .1f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float move = Input.GetAxis ("Horizontal");
		rigidBody.velocity = new Vector2 (move * 3, rigidBody.velocity.y);

		//grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		triggerCheck = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);

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
			GetComponent<AudioSource>().Play ();
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

	public static void Respawn()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void BandFound()
	{
		bandsFound++;
	}

	public int GetBandsFound()
	{
		return bandsFound;
	}

	public void DestroyedEnemy()
	{
		enemiesDestroyed++;
	}

	public int EnemiesDestroyed()
	{
		return enemiesDestroyed;
	}

	void LoadPosition()
	{
		transform.position = new Vector2 (GAMECONSTANTS.playerLocation [0], GAMECONSTANTS.playerLocation [1]);
		GAMECONSTANTS.playerLocation = new float[2] {9999, 9999};
	}
}
