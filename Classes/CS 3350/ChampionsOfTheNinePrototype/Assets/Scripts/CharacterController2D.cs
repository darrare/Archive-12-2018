//using UnityEngine;
//using System.Collections;
//
//public class CharacterController2D : MonoBehaviour {
//	private float speed;
//	private float normalSpeed = 2f;
//	private float sprintSpeed = 3f;
//	private float jumpForceInitial = 150f;
//	private float jumpForceBonus = 25f;
//	private float numberUpdatesJumpForce = 0;
//	private float numberUpdatesJumpForceMax = 10;
//	private bool facingRight = true;
//	private Animator anim;
//	private bool grounded = false;
//	public Transform groundCheck;
//	private float groundRadius = .05f;
//	public LayerMask whatIsGround;
//	private AudioSource audioSource;
//	public AudioClip jumpSound;
//	public AudioClip tongueSound;
//	public AudioClip tongueSoundThrow;
//	private bool canLick = true;
//	private bool suckUpEnemy = false;
//	private GameObject objectToEat;
//	private Sprite spriteToShoot;
//	private GameObject objectToShoot;
//
//
//	// Use this for initialization
//	void Start () {
//		anim = GetComponent<Animator> ();
//		objectToShoot = new GameObject();
//		audioSource = GetComponent<AudioSource> ();
//		objectToShoot.AddComponent<SpriteRenderer> ().sortingLayerName = "Foreground";
//		objectToShoot.AddComponent<Rigidbody2D> ().fixedAngle = true;
//		objectToShoot.GetComponent<Rigidbody2D> ().gravityScale = 0;
//		objectToShoot.AddComponent<CircleCollider2D> ().radius = .2f;
//		objectToShoot.GetComponent<CircleCollider2D> ().isTrigger = true;
//		objectToShoot.AddComponent<ProjectileScript> ();
//		objectToShoot.transform.position = new Vector3 (5000, 5000, 0);
//		GetComponent<Collider2D> ().enabled = false;
//	}
//
//	void FixedUpdate () {
//		if (suckUpEnemy) {
//			objectToEat.transform.position = new Vector3(objectToEat.transform.position.x - ((objectToEat.transform.position.x - transform.position.x) / 20), 
//			                                             objectToEat.transform.position.y - ((objectToEat.transform.position.y - transform.position.y) / 20), 0);
//			objectToEat.transform.localScale = objectToEat.transform.localScale * .90f;
//		}
//
//		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
//		anim.SetBool("Ground", grounded);
//
//		float move = Input.GetAxis ("Horizontal");
//
//		anim.SetFloat ("Speed", Mathf.Abs (move));
//
//		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * speed, GetComponent<Rigidbody2D>().velocity.y);
//		if (move > 0 && !facingRight) {
//			Flip ();
//		} 
//		else if (move < 0 && facingRight) {
//			Flip ();
//		}
//	}
//
//	void Update()
//	{
//		if (Input.GetButtonDown ("Special") && canLick) {
//			if (!anim.GetBool ("Full")){
//				TongueOut ();
//				audioSource.PlayOneShot (tongueSound);
//			}else if (anim.GetBool ("Full")){
//				SpitObject ();
//				audioSource.PlayOneShot (tongueSoundThrow);
//			}
//		}
//
//		if (anim.GetBool ("TongueOut")) {
//			GetComponent<Collider2D> ().enabled = true;
//		} else if (!anim.GetBool ("TongueOut")) {
//			GetComponent<Collider2D> ().enabled = false;
//		}
//
//		if (Input.GetButton ("Sprint")) {
//			speed = sprintSpeed;
//		} 
//		else {
//			speed = normalSpeed;
//		}
//		if (grounded && Input.GetButtonDown ("Jump")) {
//			numberUpdatesJumpForce = 0;
//			anim.SetBool ("Ground", false);
//			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForceInitial));
//			audioSource.PlayOneShot (jumpSound);
//		} else if (Input.GetButton ("Jump") && numberUpdatesJumpForce <= numberUpdatesJumpForceMax) {
//			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForceBonus));
//			numberUpdatesJumpForce++;
//		}
//	}
//
//	void SpitObject()
//	{
//		anim.SetBool ("Full", false);
//		anim.SetBool ("TongueOut", true);
//		canLick = false;
//		Invoke ("LickCoolDown", .3f);
//		objectToShoot.GetComponent<SpriteRenderer> ().sprite = spriteToShoot;
//		GameObject projectile = Instantiate (objectToShoot) as GameObject;
//		projectile.tag = "Projectile";
//		projectile.transform.localScale = new Vector3 (1, 1, 1);
//		projectile.transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
//		if (facingRight) {
//			projectile.GetComponent<Rigidbody2D> ().velocity = new Vector3 (3f, 0, 0);
//		} else {
//			projectile.GetComponent<Rigidbody2D> ().velocity = new Vector3 (-3f, 0, 0);
//		}
//	}
//
//	void TongueOut()
//	{
//		anim.SetBool ("TongueOut", true);
//		canLick = false;
//		Invoke ("LickCoolDown", .3f);
//	}
//
//	void LickCoolDown()
//	{
//		anim.SetBool ("TongueOut", false);
//		canLick = true;
//	}
//
//	void OnTriggerStay2D(Collider2D collision)
//	{
//		if (collision.tag == "Enemy" && anim.GetBool ("TongueOut") && anim.GetBool ("Full") == false) {
//			anim.SetBool ("Full", true);
//			objectToEat = collision.gameObject;
//			collision.gameObject.GetComponent<MonoBehaviour>().enabled = false;
//			foreach(Collider2D collider in collision.gameObject.GetComponents<Collider2D>()){
//				collider.enabled = false;
//			}
//			suckUpEnemy = true;
//			spriteToShoot = objectToEat.GetComponent<SpriteRenderer>().sprite;
//			Invoke ("StopSuckingUpEnemy", .3f);
//		}
//	}
//
//	void StopSuckingUpEnemy()
//	{
//		suckUpEnemy = false;
//		Destroy (objectToEat);
//	}
//
//	void Flip(){
//		facingRight = !facingRight;
//		Vector3 theScale = transform.localScale;
//		theScale.x *= -1;
//		transform.localScale = theScale;
//	}
//
//	public bool FacingRight()
//	{
//		return facingRight;
//	}
//
//	public bool IsTongueOut()
//	{
//		return anim.GetBool ("TongueOut");
//	}
//}
