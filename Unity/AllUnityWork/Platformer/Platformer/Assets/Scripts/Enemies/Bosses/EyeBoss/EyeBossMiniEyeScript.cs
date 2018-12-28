using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeBossMiniEyeScript : RecycledEffectScript
{
    //These variables are randomized to give each spawn a different feel
    float turnSpeed;
    float chaseSpeed;
    float accelerationModifier;

    float speed;
    Vector2 targetVelocity;
    Vector2 curveAmount;

    Rigidbody2D rBody;

    float maxHealth = 0;

    /// <summary>
    /// Only called once
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        rBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Initializes the effect
    /// </summary>
    /// <param name="type">The type of effect this is (used for calling methods in EffectManager)</param>
    /// <param name="startPosition">The starting position of the object</param>
    /// <param name="startRotation">The starting rotation of the object</param>
    /// <param name="velocity">The initial velocity of the effect (private update methods will handle any weird changes)</param>
    /// <param name="rotationSpeed">A rotation speed if one is required. 0 otherwise</param>
    /// <param name="timeUntilDeletion">The time that this object should be alive. After this point, this object is hidden and added to the InactiveEffects list in EffectManager</param>
    /// <param name="timeUntilFadeStart">The time until this object starts to fade out, if this object has a fade to it.</param>
    public override RecycledEffectScript Initialize(EffectRecyclerType type, Vector3 startPosition, float startRotation, Vector3 velocity, float rotationSpeed, float timeUntilDeletion, float timeUntilFadeStart)
    {
        IsActive = true;
        Renderer.color = Color.white;
        LifeSpanTimer = 0;
        Type = type;
        transform.position = startPosition;
        Velocity = velocity;
        TimeUntilDeletion = timeUntilDeletion;
        if (timeUntilFadeStart != -1f)
            TimeUntilFadeStart = timeUntilFadeStart;
        else
            TimeUntilFadeStart = TimeUntilDeletion;

        if (maxHealth == 0)
        {
            maxHealth = GetComponent<Health>().CurHealth;
        }
        else
        {
            GetComponent<Health>().CurHealth = maxHealth;
        }

        chaseSpeed = Random.Range(Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED - Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED * Constants.EYEBALL_BOSS_SPAWN_RANDOMIZATION_MULTIPLIER, Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED + Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED * Constants.EYEBALL_BOSS_SPAWN_RANDOMIZATION_MULTIPLIER);
        turnSpeed = Random.Range(Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED * Constants.EYEBALL_BOSS_SPAWN_TURN_SPEED_MODIFIER - Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED * Constants.EYEBALL_BOSS_SPAWN_TURN_SPEED_MODIFIER * Constants.EYEBALL_BOSS_SPAWN_RANDOMIZATION_MULTIPLIER, Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED * Constants.EYEBALL_BOSS_SPAWN_TURN_SPEED_MODIFIER + Constants.EYEBALL_BOSS_SPAWN_CHASE_SPEED * Constants.EYEBALL_BOSS_SPAWN_TURN_SPEED_MODIFIER * Constants.EYEBALL_BOSS_SPAWN_RANDOMIZATION_MULTIPLIER);
        accelerationModifier = Random.Range(Constants.EYEBALL_BOSS_SPAWN_ACCELERATION_MODIFIER - 3 *Constants.EYEBALL_BOSS_SPAWN_ACCELERATION_MODIFIER * Constants.EYEBALL_BOSS_SPAWN_RANDOMIZATION_MULTIPLIER, Constants.EYEBALL_BOSS_SPAWN_ACCELERATION_MODIFIER + 3 * Constants.EYEBALL_BOSS_SPAWN_ACCELERATION_MODIFIER * Constants.EYEBALL_BOSS_SPAWN_RANDOMIZATION_MULTIPLIER);

        return this;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, (Vector2)transform.position + rBody.velocity));

            targetVelocity = (GameManager.Instance.Player.transform.position - transform.position).normalized;
            Velocity += Velocity * Time.deltaTime + (Vector3)(.5f * (targetVelocity * accelerationModifier) * Time.deltaTime * Time.deltaTime);
            Velocity.Normalize();

            speed = Mathf.Lerp(chaseSpeed, turnSpeed, Mathf.Abs((Velocity - (Vector3)targetVelocity).magnitude) / 2);

            rBody.velocity = Velocity * speed;
            LifeSpanTimer += Time.deltaTime;
            if (LifeSpanTimer >= TimeUntilDeletion)
            {
                EffectFinished();
            }
        }
    }

    /// <summary>
    /// Called when the effect has finished. used to close up anything that needs to be stopped while the object is inactive.
    /// Anything that is stopped needs to be reenabled in the initialize method.
    /// </summary>
    protected override void EffectFinished()
    {
        EffectManager.Instance.CreateExplosion(transform.position, Constants.EYEBALL_BOSS_SPAWN_EXPLOSION_SCALE, Constants.EYEBALL_BOSS_SPAWN_EXPLOSION_DURATION, Constants.EYEBALL_BOSS_SPAWN_EXPLOSION_FADE_TIME, Constants.EYEBALL_BOSS_SPAWN_EXPLOSION_DAMAGE);
        IsActive = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }

    /// <summary>
    /// Called when this objects health component reaches 0
    /// </summary>
    public void Destroy()
    {
        EffectFinished();
    }

    /// <summary>
    /// Whenever something collides with this eye
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == GameManager.Instance.Player.gameObject)
        {
            EffectFinished();
        }
    }
}
