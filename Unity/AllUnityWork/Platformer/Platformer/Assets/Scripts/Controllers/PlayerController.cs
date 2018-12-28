using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerRaycastController))]
public class PlayerController : MonoBehaviour
{
    #region fields

    int curBoostCount = 0;

    bool isBoostTimerActive = false;
    float boostTimer = 0;

    float gravity;
    float gravityNorm;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    Vector2 playerInput;

    SpriteRenderer spriteRenderer;
    PlayerRaycastController controller;

    float reactingToDamageTimer = 0;
    float immuneToDamageTimer = 0;

    float timeChangeTimer = 0;

    #endregion

    #region properties

    /// <summary>
    /// Gets the players velocity
    /// </summary>
    public Vector3 Velocity
    { get { return velocity; } }

    /// <summary>
    /// Gets if the player is reacting to damage or not
    /// </summary>
    public bool IsReactingToDamage
    { get; private set; }

    /// <summary>
    /// If the player is immune to damage. Generally after reacting to damage
    /// </summary>
    public bool IsImmuneToDamage
    { get; private set; }

    #endregion

    #region private methods

    /// <summary>
    /// Initializes the playercontroller
    /// </summary>
    void Awake ()
    {
        GameManager.Instance.Player = this;
        controller = GetComponent<PlayerRaycastController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.blue;
        IsReactingToDamage = false;
        IsImmuneToDamage = false;

        gravity = -(2 * Constants.PLAYER_MAX_JUMP_HEIGHT) / Mathf.Pow(Constants.PLAYER_TIME_TO_JUMP_APEX, 2);
        gravityNorm = gravity;
        maxJumpVelocity = Mathf.Abs(gravity) * Constants.PLAYER_TIME_TO_JUMP_APEX;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * Constants.PLAYER_MIN_JUMP_HEIGHT);
    }
	
	/// <summary>
    /// updates the playercontroller
    /// </summary>
	void Update ()
    {
        //Check user input
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!IsReactingToDamage)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnJumpInputDown();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                OnJumpInputUp();
            }

            if (playerInput.y < 0)
            {
                gravity = gravityNorm * Constants.PLAYER_FAST_FALL_MULTIPLIER;
            }
            else
            {
                gravity = gravityNorm;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            timeChangeTimer = Mathf.Clamp(timeChangeTimer + Time.deltaTime, 0, Constants.PLAYER_TIME_LERP_DURATION);
            Time.timeScale = Mathf.Lerp(Constants.PLAYER_NORMAL_TIME_SPEED, Constants.PLAYER_SLOWED_TIME_SPEED, timeChangeTimer / Constants.PLAYER_TIME_LERP_DURATION);
        }
        else
        {
            timeChangeTimer = Mathf.Clamp(timeChangeTimer - Time.deltaTime, 0, Constants.PLAYER_TIME_LERP_DURATION);
            Time.timeScale = Mathf.Lerp(Constants.PLAYER_NORMAL_TIME_SPEED, Constants.PLAYER_SLOWED_TIME_SPEED, timeChangeTimer / Constants.PLAYER_TIME_LERP_DURATION);
        }


        //Handle users input
        CalculateVelocity();
        controller.Move(velocity * Time.deltaTime, playerInput);

        if (controller.Collisions.Above || controller.Collisions.Below)
        {
            if (controller.Collisions.SlidingDownMaxSlope)
            {
                velocity.y += controller.Collisions.SlopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
        if (controller.Collisions.Below)
        {
            curBoostCount = 0;
        }

        //handle the player being immune to damage
        if (IsImmuneToDamage)
        {
            immuneToDamageTimer += Time.deltaTime;
            if (immuneToDamageTimer >= Constants.PLAYER_IMMUNE_TO_DAMAGE_DURATION)
            {
                IsImmuneToDamage = false;
            }
        }
    }

    /// <summary>
    /// The player has sent the input to jump (GetKeyDown(KeyCode.Space))
    /// </summary>
    void OnJumpInputDown()
    {

        if (controller.Collisions.Below)
        {
            if (controller.Collisions.SlidingDownMaxSlope)
            {
                if (playerInput.x != -Mathf.Sign(controller.Collisions.SlopeNormal.x))
                { // not jumping against max slope
                    velocity.y = maxJumpVelocity * controller.Collisions.SlopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.Collisions.SlopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
                AudioManager.Instance.PlayOneShot(SoundEffect.Jump);
            }
        }
        else if (curBoostCount < Constants.PLAYER_MAX_BOOST_COUNT && !isBoostTimerActive)
        {
            if (playerInput.x != 0 || playerInput.y != 0)
            {
                velocity = playerInput.normalized * Constants.PLAYER_INITIAL_BOOST_MULTIPLIER;
                isBoostTimerActive = true;
                curBoostCount += 1;
                AudioManager.Instance.PlayOneShot(SoundEffect.Dash);
            }
            else
            {
                velocity = Vector2.up * Constants.PLAYER_INITIAL_BOOST_MULTIPLIER;
                isBoostTimerActive = true;
                curBoostCount += 1;
                AudioManager.Instance.PlayOneShot(SoundEffect.Dash);
            }
        }
    }

    /// <summary>
    /// The player has released the input to jump (GetKeyUp(KeyCode.Space))
    /// </summary>
    void OnJumpInputUp()
    {
        if (curBoostCount == 0 && velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    /// <summary>
    /// Calculates the players velocity
    /// </summary>
    void CalculateVelocity()
    {
        if (isBoostTimerActive && !IsReactingToDamage)
        {
            boostTimer += Time.deltaTime;

            velocity += (Vector3)(playerInput.normalized * Constants.PLAYER_INITIAL_BOOST_MULTIPLIER) * Mathf.Lerp(Constants.PLAYER_INPUT_BOOST_INIT_MODIFIER, Constants.PLAYER_INPUT_BOOST_END_MODIFIER, boostTimer / Constants.PLAYER_BOOST_DURATION);
            if (velocity.magnitude > Constants.PLAYER_INITIAL_BOOST_MULTIPLIER)
            {
                velocity = velocity.normalized * Constants.PLAYER_INITIAL_BOOST_MULTIPLIER;
            }

            if (boostTimer >= Constants.PLAYER_BOOST_DURATION)
            {
                isBoostTimerActive = false;
                boostTimer = 0;
                velocity *= .3f;
            }
        }
        else if (IsReactingToDamage)
        {
            reactingToDamageTimer += Time.deltaTime;
            velocity.x = Mathf.SmoothDamp(velocity.x, 0, ref velocityXSmoothing, (controller.Collisions.Below) ? Constants.PLAYER_ACCELERATION_TIME_GROUNDED : Constants.PLAYER_ACCELERATION_TIME_AIRBORNE);
            velocity.y += gravity * Time.deltaTime / 3; //Divided by 3 so it doesn't have as much of an effect

            if (velocity.y < gravity * .7f)
            {
                velocity.y = gravity * .7f;
            }

            if (reactingToDamageTimer >= Constants.PLAYER_REACTING_TO_DAMAGE_DURATION)
            {
                IsReactingToDamage = false;
                IsImmuneToDamage = true;
                immuneToDamageTimer = 0;
                spriteRenderer.color = Color.blue;
            }
        }
        else
        {
            
            velocity.x = Mathf.SmoothDamp(velocity.x, playerInput.x * Constants.PLAYER_MOVE_SPEED, ref velocityXSmoothing, (controller.Collisions.Below) ? Constants.PLAYER_ACCELERATION_TIME_GROUNDED : Constants.PLAYER_ACCELERATION_TIME_AIRBORNE);
            velocity.y += gravity * Time.deltaTime;
            
            if (velocity.y < gravity * .7f)
            {
                velocity.y = gravity * .7f;
            }
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// Called whenever the player takes damage
    /// </summary>
    /// <param name="amount">The amount of damage the player took.</param>
    /// <param name="col">The collision so we can push the character back slightly.</param>
    public void DamageTaken(float amount, Collision2D col)
    {
        if (!IsReactingToDamage && !IsImmuneToDamage)
        {
            IsReactingToDamage = true;
            reactingToDamageTimer = 0;
            Vector2 averagedPoint = new Vector2();
            foreach (ContactPoint2D c in col.contacts)
            {
                averagedPoint += c.point;
            }
            averagedPoint /= col.contacts.Length;
            velocity = ((Vector2)transform.position - averagedPoint).normalized;
            velocity *= Constants.PLAYER_REACTING_TO_DAMAGE_KNOCKBACK_MULTIPLIER;
            spriteRenderer.color = Color.red;
            curBoostCount = Mathf.Clamp(curBoostCount - 1, 0, Constants.PLAYER_MAX_BOOST_COUNT);
        }

    }

    /// <summary>
    /// Good for round objects
    /// </summary>
    /// <param name="amount">The amount of damage the player took.</param>
    /// <param name="position">The position of the thing hitting us so we can knock ourselves back</param>
    public void DamageTaken(float amount, Vector2 position)
    {
        if (!IsReactingToDamage && !IsImmuneToDamage)
        {
            IsReactingToDamage = true;
            reactingToDamageTimer = 0;
            velocity = ((Vector2)transform.position - position).normalized;
            velocity *= Constants.PLAYER_REACTING_TO_DAMAGE_KNOCKBACK_MULTIPLIER;
            spriteRenderer.color = Color.red;
            curBoostCount = Mathf.Clamp(curBoostCount - 1, 0, Constants.PLAYER_MAX_BOOST_COUNT);
        }
    }

    /// <summary>
    /// Useful for not round objects
    /// </summary>
    /// <param name="amount">The amount of damage the player took.</param>
    /// <param name="knockBackAngle">The direction we want to knock the player back</param>
    public void DamageTaken(float amount, float knockBackAngle)
    {
        if (!IsReactingToDamage && !IsImmuneToDamage)
        {
            IsReactingToDamage = true;
            reactingToDamageTimer = 0;
            velocity = Constants.PLAYER_REACTING_TO_DAMAGE_KNOCKBACK_MULTIPLIER * Utilities.LookAtDirection(knockBackAngle);
            spriteRenderer.color = Color.red;
            curBoostCount = Mathf.Clamp(curBoostCount - 1, 0, Constants.PLAYER_MAX_BOOST_COUNT);
        }
    }

    #endregion
}
