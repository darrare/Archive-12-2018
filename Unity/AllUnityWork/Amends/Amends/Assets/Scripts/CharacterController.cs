using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    float speed = 7f;
    float horizAxis;
    float accelConst = 28f; // 4 times speed, so we should accellerate in .25f seconds
    float decelConst = .90f;
    float airDecelConst = .98f;
    float jumpInitSpeed = 9f;
    float doubleJumpInitSpeed = 13f;
    float timeToJumpHigh = .20f;
    float timerForJumpHigh = 0f;
    Rigidbody2D rBody;
    Animator anim;
    bool canDoubleJump = true;
    bool isGrounded = false;

    /// <summary>
    /// Whether or not the character is grounded. This property is updated every frame from CharacterGroundCheck.cs.
    /// </summary>
    public bool IsGrounded
    {
        get { return isGrounded; }
        set
        {
            isGrounded = value;
            if (value)
            {
                canDoubleJump = true;
                timerForJumpHigh = 0;
            }
        }
    }

    /// <summary>
    /// Gets the current velocity of the player
    /// </summary>
    public Vector2 Velocity
    {
        get { return rBody.velocity; }
    }

	// Use this for initialization
	void Start ()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        horizAxis = Input.GetAxisRaw("Horizontal");
        //Handles all of the player mobility when they are connected to the ground
        if (IsGrounded)
        {
            if (horizAxis == 0)
            {
                rBody.velocity = new Vector2(Mathf.Clamp(rBody.velocity.x * (decelConst - Time.deltaTime), -speed, speed), rBody.velocity.y);
            }
            else
            {
                rBody.velocity = new Vector2(Mathf.Clamp(rBody.velocity.x + horizAxis * accelConst * Time.deltaTime, -speed, speed), rBody.velocity.y);
            }
            
            if (Input.GetKey(KeyCode.Space))
            {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpInitSpeed);
            }
        }
        //Handles all of the player mobility when they are in the air.
        else
        {
            timerForJumpHigh += Time.deltaTime;
            if (horizAxis == 0)
            {
                rBody.velocity = new Vector2(Mathf.Clamp(rBody.velocity.x * (airDecelConst - Time.deltaTime), -speed, speed), rBody.velocity.y);
            }
            else
            {
                rBody.velocity = new Vector2(Mathf.Clamp(rBody.velocity.x + horizAxis * accelConst / 2 * Time.deltaTime, -speed, speed), rBody.velocity.y);
            }
            
            //Handles the double jump
            if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, doubleJumpInitSpeed);
                canDoubleJump = false;
                SpawnJumpEffect();
            }
            //Handles the "hold space to jump higher" effect.
            else if (Input.GetKey(KeyCode.Space) && timerForJumpHigh <= timeToJumpHigh && canDoubleJump)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpInitSpeed);
            }
        }

        //Controls gravity stuff. Won't work if doing a mobile port since it would be a weird key to press. TODO: Figure out if I actually want this mechanic in the game
        if (Input.GetKey(KeyCode.S))
        {
            rBody.gravityScale = 8f;
        }
        else
        {
            rBody.gravityScale = 4f;
        }

        anim.SetFloat("Speed", Mathf.Abs(rBody.velocity.x + horizAxis * 5));
        anim.SetBool("IsGrounded", IsGrounded);

        //Handles which direction to face the player.
        if (horizAxis > .2f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizAxis < -.2f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    /// <summary>
    /// Spawns the little particle system under the player whenever they do a double jump
    /// </summary>
    void SpawnJumpEffect()
    {
        GameObject jumpEffect = Instantiate(Resources.Load("JumpEffect")) as GameObject;
        jumpEffect.transform.position = (Vector3)transform.position - Vector3.up * .4f;
        Destroy(jumpEffect, 1f);
    }
}
