using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script for a warrior character
/// </summary>
public class WarriorScript : CharacterScript
{
    #region Fields

    [SerializeField]GameObject explosion;
    [SerializeField]GameObject axe;
    [SerializeField]GameObject sword;
    Image boostBar;
    Timer boostTimer;
    Transform swordTransform;
    Collider2D swordCollider;
    SwordScript swordScript;
    float leapStartX;
    float leapTargetX;
    float slashStartRot;
    float slashEndRot;
    bool leaping = false;
    bool slashing = false;
    bool hasAxe = true;
    bool recharge = false;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets whether or not the warrior is leaping
    /// Also sets if the warrior is controllable
    /// </summary>
    private bool Leaping
    {
        get { return leaping; }
        set 
        { 
            leaping = value;
            Controllable = !value;
        }
    }

    /// <summary>
    /// Gets or sets whether or not the warrior is slashing
    /// </summary>
    private bool Slashing
    {
        get { return slashing; }
        set
        {
            slashing = value;
            swordCollider.enabled = value;
        }
    }

    /// <summary>
    /// Gets the warrior's current damage multiplier
    /// </summary>
    private float DamageMult
    { get { return 1 + (Constants.WARRIOR_MAX_DAMAGE_BOOST * (Energy / maxEnergy)); } }

    /// <summary>
    /// Gets and sets the character's energy, setting the energy bar appropriately
    /// </summary>
    protected override float Energy
    {
        get { return base.Energy; }
        set
        {
            if (!boostTimer.IsRunning)
            {
                // Handles the energy maxing out
                if (value >= Constants.WARRIOR_ENERGY)
                {
                    base.Energy = Constants.WARRIOR_ENERGY;
                    gCDTimer.TotalSeconds = Constants.WARRIOR_RECHARGE_TIME;
                    gCDTimer.Register(RechargeTimerFinished);
                    gCDTimer.Start();
                    recharge = true;
                    Slashing = false;
                    GameManager.Instance.SpawnParticle(Constants.RECHARGE_PART, transform.position + new Vector3(0, 1.6f)).transform.SetParent(transform);
                }
                else
                {
                    base.Energy = value;
                    swordScript.Damage = Constants.SLASH_DAMAGE * DamageMult;
                }
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the character script; called from controller
    /// </summary>
    /// <param name="targetTag">the tag of the targeted objects</param>
    /// <param name="energyChanged">the handler for when the energy changes</param>
    /// <param name="healthBar">the health bar</param>
    /// <param name="timerBars">the array of timer bars</param>
    /// <param name="healthMult">the health multiplier</param>
    public override void Initialize(string targetTag, EnergyChangedHandler energyChanged, Image healthBar, Image[] timerBars, float healthMult = 1)
    {
        // Sets fields
        maxHealth = Constants.WARRIOR_HEALTH;
        moveSpeed = Constants.WARRIOR_MOVE_SPEED;
        jumpSpeed = Constants.WARRIOR_JUMP_SPEED;
        maxEnergy = Constants.WARRIOR_ENERGY;
        gCDTimer = new Timer(Constants.WARRIOR_GCD);
        secondaryCDTimer = new Timer(Constants.LIGHTNING_CD);
        powerCDTimer = new Timer(Constants.LEAP_CD);
        specialCDTimer = new Timer(Constants.WARRIOR_BOOST_CD);
        boostTimer = new Timer(Constants.WARRIOR_BOOST_TIME);
        boostTimer.Register(BoostTimerFinished);
        swordTransform = sword.transform;
        swordScript = sword.GetComponent<SwordScript>();
        swordScript.Initialize(Constants.SLASH_DAMAGE, targetTag, SlashDamageHandler);
        swordCollider = sword.GetComponent<Collider2D>();
        swordCollider.enabled = false;

        // Loads sounds
        mainAbilitySound = GameManager.Instance.GameSounds[Constants.ICE_CAST_SND];
        secondaryAbilitySound = GameManager.Instance.GameSounds[Constants.RANGER_SHOOT_SND];
        powerAbilitySound = GameManager.Instance.GameSounds[Constants.LEAP_SND];
        specialAbilitySound = GameManager.Instance.GameSounds[Constants.WARRIOR_BOOST_SND];
        if (timerBars != null)
        { boostBar = timerBars[0]; }
        base.Initialize(targetTag, energyChanged, healthBar, timerBars, healthMult);
        Energy = 0;
    }

    /// <summary>
    /// Updates the character; not called on normal update cycle, called by controller
    /// </summary>
    public override void UpdateChar()
    {
        base.UpdateChar();

        // Updates energy
        if (recharge)
        { Energy = Mathf.Max(0, Energy - (Constants.WARRIOR_ADR_RECHARGE_LOSS * Time.deltaTime)); }
        else
        { Energy = Mathf.Max(0, Energy - (Constants.WARRIOR_ADR_LOSS * Time.deltaTime)); }
        boostTimer.Update();

        if (Slashing)
        {
            // Updates slash
            if (gCDTimer.IsRunning)
            {
                ArmAngle = Mathf.Lerp(slashStartRot, slashEndRot, gCDTimer.ElapsedSeconds / gCDTimer.TotalSeconds);
                swordTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -80, gCDTimer.ElapsedSeconds / gCDTimer.TotalSeconds));
            }
            else
            {
                Slashing = false;
                swordTransform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (Leaping)
        {
            // Updates leap
            if (((leapTargetX > leapStartX && transform.position.x >= leapTargetX) || 
                (leapTargetX < leapStartX && transform.position.x <= leapTargetX) || 
                rbody.velocity.x == 0) && !animator.GetBool(Constants.LEAP_FLAG))
            {
                rbody.velocity = Vector2.down * 5;
                Arm.SetActive(false);
                animator.SetBool(Constants.LEAP_FLAG, true);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if ((Grounded || rbody.velocity == Vector2.zero) && animator.GetBool(Constants.LEAP_FLAG))
            {
                Arm.SetActive(true);
                animator.SetBool(Constants.LEAP_FLAG, false);
                Leaping = false;
                GameManager.Instance.PlaySoundPitched(audioSource, GameManager.Instance.GameSounds[Constants.EXP_ARROW_SND]);
                ((GameObject)Instantiate(explosion, transform.position, transform.rotation)).GetComponent<ExplosionScript>().Initialize(Constants.LEAP_DAMAGE * 
                    DamageMult, targetTag, LeapDamageHandler);
            }
        }

        // Updates boost bar
        try { boostBar.fillAmount = 1 - (boostTimer.ElapsedSeconds / boostTimer.TotalSeconds); }
        catch (System.NullReferenceException) { }
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    public override void FireMainAbility()
    {
        if (Controllable && !gCDTimer.IsRunning)
        {
            gCDTimer.Start();
            Slashing = true;
            slashStartRot = ArmAngle + (Constants.SLASH_HALF_ANGLE * Mathf.Sign(transform.localScale.x));
            slashEndRot = ArmAngle - (Constants.SLASH_HALF_ANGLE * Mathf.Sign(transform.localScale.x));
        }
    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    public override void FireSecondaryAbility()
    {
        if (hasAxe && Controllable && !gCDTimer.IsRunning && !secondaryCDTimer.IsRunning)
        {
            if (FireStraightProjectileAttack(axe, Constants.AXE_ENERGY, gCDTimer, Constants.AXE_DAMAGE * DamageMult, 
                Constants.AXE_SPEED, AxeDamageHandler) != null)
            {
                hasAxe = false;
                secondaryCDTimer.Start();
                secondaryCDTimer.IsRunning = false;
                GameManager.Instance.PlaySoundPitched(audioSource, secondaryAbilitySound);
            }
        }
    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    public override void FirePowerAbility()
    {
        if (!gCDTimer.IsRunning && !powerCDTimer.IsRunning)
        {
            float leapAngle = Utilities.CalculateLaunchAngle(transform.position, Utilities.MousePosition, Constants.LEAP_SPEED, Constants.CHAR_GRAV_SCALE);
            if (!float.IsNaN(leapAngle))
            {
                Leaping = true;
                powerCDTimer.Start();
                gCDTimer.Start();
                GameManager.Instance.PlaySoundPitched(audioSource, powerAbilitySound);

                leapStartX = transform.position.x;
                leapTargetX = Utilities.MousePosition.x;
                transform.localRotation = Quaternion.Euler(0, 0, leapAngle - 90);
                rbody.velocity = new Vector2(Mathf.Cos(leapAngle * Mathf.Deg2Rad) * Constants.LEAP_SPEED,
                    Mathf.Sin(leapAngle * Mathf.Deg2Rad) * Constants.LEAP_SPEED);
            }
        }
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    public override void FireSpecialAbility()
    {
        if (!specialCDTimer.IsRunning)
        {
            gCDTimer.Finish();
            Energy = 99;
            GameManager.Instance.PlaySoundPitched(audioSource, specialAbilitySound);
            GameManager.Instance.SpawnParticle(Constants.WARRIOR_BOOST_PART, Arm.transform.position).transform.SetParent(transform);
            boostTimer.Start();
            specialCDTimer.Start();
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles when the warrior enters a collision
    /// </summary>
    /// <param name="collision">the collsion</param>
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // Picks up the axe pickup
        if (!hasAxe && collision.gameObject.tag == Constants.AXE_PICKUP_TAG)
        {
            hasAxe = true;
            Destroy(collision.gameObject);
            secondaryCDTimer.IsRunning = true;
        }
    }

    /// <summary>
    /// Handles the slash damaging something
    /// </summary>
    /// <param name="targetLived">whether or not the target lived</param>
    protected void SlashDamageHandler(bool targetLived)
    {
        Energy += Constants.SLASH_ADR;
    }

    /// <summary>
    /// Handles the axe damaging something
    /// </summary>
    /// <param name="targetLived">whether or not the target lived</param>
    protected void AxeDamageHandler(bool targetLived)
    {
        Energy += Constants.AXE_ADR;
    }

    /// <summary>
    /// Handles the leap damaging something
    /// </summary>
    /// <param name="targetLived">whether or not the target lived</param>
    protected void LeapDamageHandler(bool targetLived)
    {
        Energy += Constants.LEAP_ADR;
    }

    /// <summary>
    /// Handles the global cooldown timer when in recharge mode finishing
    /// </summary>
    protected void RechargeTimerFinished()
    {
        gCDTimer = new Timer(Constants.WARRIOR_GCD);
        recharge = false;
    }

    /// <summary>
    /// Handles the boost timer finishing
    /// </summary>
    protected void BoostTimerFinished()
    {
        Energy = 100;
    }

    #endregion
}
