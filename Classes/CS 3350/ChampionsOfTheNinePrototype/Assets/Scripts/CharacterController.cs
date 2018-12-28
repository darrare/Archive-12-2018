using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	private bool facingRight = true;
	private bool grounded = true;
	private float jumpForceInitial = 150f;
	private float jumpForceBonus = 25f;
	private float numberUpdatesJumpForce = 0;
	private float numberUpdatesJumpForceMax = 10;
	public Transform groundCheck;
	private float groundRadius = .05f;
	public LayerMask whatIsGround;

	private float curMana;
	private float maxMana = 500;

	private float curHealth;
	private float maxHealth = 500;

	// Use this for initialization
	void Start () {
		curMana = maxMana;
		curHealth = maxHealth;
		InvokeRepeating ("ManaOverTime", 0, .2f);
	}

	void ManaOverTime()
	{
		if (curMana < maxMana - 10) {
			curMana += 10;
		} else if (curMana < maxMana) {
			curMana = maxMana;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float move = Input.GetAxis ("Horizontal");
		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * 5, GetComponent<Rigidbody2D>().velocity.y);

		if (move > 0 && !facingRight) {
			Flip ();
		} 
		else if (move < 0 && facingRight) {
			Flip ();
		}

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);

		if (grounded && Input.GetButtonDown ("Jump")) {
			numberUpdatesJumpForce = 0;
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForceInitial));
		} else if (Input.GetButton ("Jump") && numberUpdatesJumpForce <= numberUpdatesJumpForceMax) {
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForceBonus));
			numberUpdatesJumpForce++;
		}
	}

	public void RemoveMana(float amount)
	{
		curMana -= amount;
	}

	public float Mana()
	{
		return curMana;
	}

	public float MaxMana()
	{
		return maxMana;
	}

	public void RemoveHealth(float amount)
	{
		curHealth -= amount;
	}
	
	public float Health()
	{
		return curHealth;
	}
	
	public float MaxHealth()
	{
		return maxHealth;
	}

	void Flip()
	{

	}
}
