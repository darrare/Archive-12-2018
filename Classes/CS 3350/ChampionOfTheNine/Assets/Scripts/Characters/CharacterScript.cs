using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent class for character scripts
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class CharacterScript : DamagableObjectScript
{
    #region Fields

    [SerializeField]AudioSource walkAudio;
    [SerializeField]Transform fireLocation;
    [SerializeField]Transform groundCheck;
    [SerializeField]LayerMask whatIsGround;
    [SerializeField]GameObject arm;
    [SerializeField]GameObject deadPrefab;
    [SerializeField]Color energyColor;

    protected Animator animator;
    protected Rigidbody2D rbody;
    protected Timer gCDTimer;
    protected Timer secondaryCDTimer;
    protected Timer powerCDTimer;
    protected Timer specialCDTimer;
    protected AudioClip mainAbilitySound;
    protected AudioClip secondaryAbilitySound;
    protected AudioClip powerAbilitySound;
    protected AudioClip specialAbilitySound;
    protected float maxEnergy;
    protected float moveSpeed;
    protected float jumpSpeed;
    protected string targetTag;
    
    AudioClip jumpSound;
    AudioClip landSound;
    Vector3 baseScale;
    Vector3 flippedScale;
    EnergyChangedHandler energyChanged = Blank;
    float energy;
    bool controllable = true;

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the character's energy, setting the energy bar appropriately
    /// </summary>
    protected virtual float Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            energyChanged(energy / maxEnergy);
        }
    }

    /// <summary>
    /// Gets and sets the angle of the character's arm
    /// </summary>
    protected float ArmAngle
    {
        set { arm.transform.rotation = Quaternion.Euler(0, 0, Utilities.GetAngleDegrees(value, transform.localScale.x)); }
        get { return Utilities.GetAngleDegrees(arm.transform.rotation.eulerAngles.z, transform.localScale.x); }
    }

    /// <summary>
    /// Gets and sets whether or not the character is controllable
    /// </summary>
    protected bool Controllable
    {
        get { return controllable; }
        set
        {
            if (!value)
            { rbody.velocity = Vector2.zero; }
            controllable = value;
        }
    }

    /// <summary>
    /// Gets the character's energy color
    /// </summary>
    public Color EnergyColor
    { get { return energyColor; } }

    /// <summary>
    /// Gets whether or not the character is grounded
    /// </summary>
    public bool Grounded
    { get { return Physics2D.OverlapCircle(groundCheck.position, Constants.GROUND_CHECK_RADIUS, whatIsGround); } }

    /// <summary>
    /// Gets the character's global cooldown timer
    /// </summary>
    public Timer GCDTimer
    { get { return gCDTimer; } }

    /// <summary>
    /// Gets the character's secondary cooldown timer
    /// </summary>
    public Timer SecondaryCDTimer
    { get { return secondaryCDTimer; } }

    /// <summary>
    /// Gets the character's power cooldown timer
    /// </summary>
    public Timer PowerCDTimer
    { get { return powerCDTimer; } }

    /// <summary>
    /// Gets the character's special cooldown timer
    /// </summary>
    public Timer SpecialCDTimer
    { get { return specialCDTimer; } }

    /// <summary>
    /// Gets the character's arm object
    /// </summary>
    public GameObject Arm
    { get { return arm; } }

    /// <summary>
    /// Gets the position of the fire location
    /// </summary>
    public Vector2 FireLocation
    { get { return fireLocation.position; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Updates the character; not called on normal update cycle, called by controller
    /// </summary>
    public virtual void UpdateChar()
    {
        // Updates cooldown timers
        gCDTimer.Update();
        powerCDTimer.Update();
        secondaryCDTimer.Update();
        specialCDTimer.Update();

        animator.SetFloat(Constants.XVELOCTIY_FLAG, Mathf.Abs(rbody.velocity.x));

        // Set jump animation/play sounds
        if (Grounded)
        {
            if (!animator.GetBool(Constants.GROUNDED_FLAG))
            {
                animator.SetBool(Constants.GROUNDED_FLAG, true);
                GameManager.Instance.PlaySoundPitched(audioSource, landSound);
            }
        }
        else
        {
            if (animator.GetBool(Constants.GROUNDED_FLAG))
            {
                animator.SetBool(Constants.GROUNDED_FLAG, false);
                GameManager.Instance.PlaySoundPitched(audioSource, jumpSound);
            }
        }
    }

    /// <summary>
    /// Initializes the character script; called from controller
    /// </summary>
    /// <param name="targetTag">the tag of the targeted objects</param>
    /// <param name="energyChanged">the handler for when the energy changes</param>
    /// <param name="healthBar">the health bar</param>
    /// <param name="timerBars">the array of timer bars</param>
    /// <param name="healthMult">the health multiplier</param>
    public virtual void Initialize(string targetTag, EnergyChangedHandler energyChanged, Image healthBar, Image[] timerBars, float healthMult = 1)
    {
        maxHealth *= healthMult;
        this.energyChanged = energyChanged;
        this.targetTag = targetTag;
        if (healthBar != null)
        { this.healthBar = healthBar; }
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseScale = transform.localScale;
        flippedScale = new Vector3(-baseScale.x, baseScale.y, baseScale.z);
        walkAudio.clip = GameManager.Instance.GameSounds[Constants.CHAR_WALK_SND];
        hitSound = GameManager.Instance.GameSounds[Constants.CHAR_HIT_SND];
        deathSound = GameManager.Instance.GameSounds[Constants.CHAR_DEATH_SND];
        jumpSound = GameManager.Instance.GameSounds[Constants.CHAR_JUMP_SND];
        landSound = GameManager.Instance.GameSounds[Constants.CHAR_LAND_SND];
        base.Initialize();
    }

    /// <summary>
    /// Sets the angle of the character's arm, flipping the character as needed
    /// </summary>
    /// <param name="angle">the new angle</param>
    public virtual void SetArmAngle(float angle)
    {
        if (controllable)
        {
            // Flips the character if needed
            if (transform.localScale.x > 0 && angle > 90 && angle < 270)
            { transform.localScale = flippedScale; }
            else if (transform.localScale.x < 0 && (angle <= 90 || angle >= 270))
            { transform.localScale = baseScale; }
            ArmAngle = angle; 
        }
    }

    /// <summary>
    /// Moves the character using the given movement input
    /// </summary>
    /// <param name="input">the movement input</param>
    public virtual void Move(float input)
    {
        // Plays/stops sound
        if (Mathf.Abs(rbody.velocity.x) > 0 && !walkAudio.isPlaying)
        { walkAudio.Play(); }
        else if (rbody.velocity.x == 0 && walkAudio.isPlaying)
        { walkAudio.Stop(); }

        if (controllable)
        {
            // Handles horizontal movement
            float movement = input * moveSpeed;
            rbody.velocity = new Vector2(movement, rbody.velocity.y);
        }
    }

    /// <summary>
    /// Makes the character jump
    /// </summary>
    public virtual void Jump()
    {
        if (controllable)
        { rbody.velocity = new Vector2(0, jumpSpeed); }
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    public abstract void FireMainAbility();

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    public abstract void FireSecondaryAbility();

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    public abstract void FirePowerAbility();

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    public abstract void FireSpecialAbility();

    #endregion

    #region Protected Methods

    /// <summary>
    /// Fires a projectile attack straight forward from the character
    /// </summary>
    /// <param name="prefab">the projectile prefab</param>
    /// <param name="energyCost">the energy cost of the attack</param>
    /// <param name="cooldown">the cooldown timer to start</param>
    /// <param name="damage">the projectile's damage</param>
    /// <param name="projSpeed">the projectile's movement speed</param>
    /// <returns>the projectile, if one was fired</returns>
    protected virtual ProjScript FireStraightProjectileAttack(GameObject prefab, float energyCost, Timer cooldown, float damage, 
        float projSpeed, DamageHandler damageHandler = null)
    {
        ProjScript projectile = FireProjectileAttack(prefab, energyCost, cooldown);
        if (projectile != null)
        { projectile.Initialize(FireLocation, ArmAngle, targetTag, damage, projSpeed, damageHandler); }
        return projectile;
    }

    /// <summary>
    /// Fires a projectile attack
    /// </summary>
    /// <param name="prefab">the projectile prefab</param>
    /// <param name="energyCost">the energy cost of the attack</param>
    /// <param name="cooldown">the cooldown timer to start</param>
    /// <returns>the projectile, if one was fired</returns>
    protected virtual ProjScript FireProjectileAttack(GameObject prefab, float energyCost, Timer cooldown)
    {
        ProjScript projScript = null;
        if (energy >= energyCost)
        {
            // Creates the projectile
            projScript = Instantiate<GameObject>(prefab).GetComponent<ProjScript>();

            // Subtracts energy and starts timer
            energy -= energyCost;
            cooldown.Start();
        }
        return projScript;
    }

    /// <summary>
    /// Handles the character dying
    /// </summary>
    protected override void Death()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        try
        { 
            Instantiate<GameObject>(deadPrefab).GetComponent<DeadScript>().Initialize(GetComponent<SpriteRenderer>().color, 
                transform.localScale, transform.position);
            GetComponent<CharacterControllerScript>().Death();
        }
        catch (NullReferenceException) { }
    }

    #endregion

    private static void Blank(float value) { }
}
