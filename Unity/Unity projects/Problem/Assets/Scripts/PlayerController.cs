using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	enum PlayerState { IDLE, RUNNING, JUMPING, FLIPPING };
	PlayerState playerState;

	float moveSpeed = 6;
	float normalSpeed = 6;
	float sprintSpeed = 9;

	float initJumpVelocity = 11;
	float jumpBonusTime = 0f;
	float jumpBonusMax = .25f;
	float jumpTimer = 0;
	bool isGrounded = false;

	float currentAxisValue;
	bool canShoot;
	bool alternate = false;

	bool readyForDoubleJump = false;
	bool doubleJumped = false;
	bool isFacingRight = true;

	public Transform jumpParticleSpawnLocation;
	public GameObject sprite;
	public AudioClip[] jumpSounds;
	AudioSource audioSource;
	Rigidbody2D rBody;
	Animator anim;
	Transform groundTransform;

	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D> ();
		anim = sprite.GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
	}

	/// <summary>
	/// Gets or sets a value indicating whether this instance is grounded.
	/// </summary>
	/// <value><c>true</c> if this instance is grounded; otherwise, <c>false</c>.</value>
	public bool IsGrounded
	{
		get { return isGrounded; }
		set { isGrounded = value; }
	}

	/// <summary>
	/// Set by the Character's child object that detects whether or not the player is on solid ground and sends back the object the player is standing on.
	/// </summary>
	/// <value>Whatever object the player is standing on</value>
	public Transform GroundTransform
	{
		set { groundTransform = value; }
	}

	/// <summary>
	/// Controls all physics and animation
	/// </summary>
	void Update () {
		if (Time.timeScale == 0) {
			return;
		}
		//Attach the player to whatever it is standing on so that the player's functionality works with moving platforms. (and other objects like fish)
		transform.parent = groundTransform;

		//Modify players speed based on whether or not the sprint button is being held
		if (InputManager.GetButton ("Sprint")) {
			moveSpeed = sprintSpeed;
		} else {
			moveSpeed = normalSpeed;
		}

		//Shoot controls. Similar to a paintball style of shooting where you can only press one at a time.
		if (InputManager.GetButtonDown ("Shoot0") || InputManager.GetButtonDown("Shoot1")) {
			ShootLaser ();
		}
			
		//Move the player horizontally based on the input sent in by the Horizontal axis
		rBody.velocity = new Vector2 (InputManager.GetAxisRaw ("Horizontal") * moveSpeed, rBody.velocity.y);

		//Controls which direction the player is facing
		if (rBody.velocity.x > 0) {
			isFacingRight = true;
			transform.localScale = new Vector2 (-1, 1);
		} else if (rBody.velocity.x < 0) {
			isFacingRight = false;
			transform.localScale = new Vector2 (1, 1);
		}

		//Check to see if the player is idle or running and set the player accordingly
		if (InputManager.GetAxisRaw("Horizontal") == 0 && playerState != PlayerState.FLIPPING) {
			playerState = PlayerState.IDLE;
		} else if (isGrounded) {
			playerState = PlayerState.RUNNING;
		}
			
		//Jump controls to allow the user to make the character jump high or low
		if (InputManager.GetButton ("Jump") && jumpBonusTime < jumpBonusMax) {
			rBody.velocity = new Vector2 (rBody.velocity.x, initJumpVelocity);
			if (playerState != PlayerState.JUMPING && jumpBonusTime == 0) {
				audioSource.PlayOneShot (jumpSounds [0]);
			}
			jumpBonusTime += Time.deltaTime;
			playerState = PlayerState.JUMPING;

		} else if (isGrounded) {
			jumpBonusTime = 0;
			readyForDoubleJump = false;
			doubleJumped = false;
		} else if (!InputManager.GetButton ("Jump") && !readyForDoubleJump) {
			jumpBonusTime = jumpBonusMax;
			readyForDoubleJump = true;
		} 

		//Jump controls for double jumping
		if (!doubleJumped && readyForDoubleJump && InputManager.GetButton("Jump")) {
			rBody.velocity = new Vector2 (rBody.velocity.x, initJumpVelocity * 1.5f);
			Instantiate (Resources.Load ("DoubleJumpParticleEffect") as GameObject).transform.position = jumpParticleSpawnLocation.transform.position;
			readyForDoubleJump = false;
			doubleJumped = true;
			playerState = PlayerState.FLIPPING;
			audioSource.PlayOneShot (jumpSounds [1]);
		}
			
		//Controls the player rotation for flipping and setting back to normal when player lands.
		if (playerState != PlayerState.FLIPPING) {
			sprite.transform.eulerAngles = Vector3.zero;
		} else {
			sprite.transform.eulerAngles = new Vector3 (0, 0, sprite.transform.eulerAngles.z + Time.deltaTime * 1000);
		}

		//Prevents a bug where the player gets caught in weird states
		if (!isGrounded && playerState != PlayerState.FLIPPING) {
			playerState = PlayerState.JUMPING;
		}

		//Sets the animation to whatever the playerstate is.
		anim.SetInteger ("PlayerState", (int)(playerState + 1));
	}

	/// <summary>
	/// Called from any object that may just kill the player.
	/// </summary>
	public void KillPlayer()
	{
		//Levelcontroller is on every level and it controls things such as respawnable items and eventually score/etc.
		CONSTANTS.levelController.StartRespawnTimer ();
		Instantiate (Resources.Load ("Explosion"), transform.position, Quaternion.identity); //Boom
	}

	/// <summary>
	/// Creates a laser at the right spot and fires it in the right direction
	/// </summary>
	void ShootLaser()
	{
		GameObject laser = Instantiate (Resources.Load ("Laser"), transform.GetChild (2).transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Laser> ().SetOwner ("Player");
		if (isFacingRight) {
			laser.GetComponent<Rigidbody2D> ().velocity = new Vector2 (15, 0);
		} else {
			laser.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-15, 0);
		}

	}
}
