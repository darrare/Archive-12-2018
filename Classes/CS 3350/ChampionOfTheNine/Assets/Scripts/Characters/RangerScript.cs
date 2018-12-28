using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script for a ranger character
/// </summary>
public class RangerScript : CharacterScript
{
    #region Fields

    [SerializeField]GameObject arrow;
    [SerializeField]GameObject pierceArrow;
    [SerializeField]GameObject expArrow;

    Image pierceBar;
    Image boostBar;
    Timer pierceShootWindow;
    Timer pierceShootCD;
    Timer boostTimer;

    float cooldownMult = 1;
    float arrowSpeedMult = 1;
    float arrowDamageMult = 1;
    float energyRegenMult = 1;

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
        maxHealth = Constants.RANGER_HEALTH;
        moveSpeed = Constants.RANGER_MOVE_SPEED;
        jumpSpeed = Constants.RANGER_JUMP_SPEED;
        maxEnergy = Constants.RANGER_ENERGY;
        gCDTimer = new Timer(Constants.RANGER_GCD);
        pierceShootWindow = new Timer(Constants.PIERCE_SHOOT_WINDOW);
        pierceShootCD = new Timer(Constants.PIERCE_SHOOT_CD);
        powerCDTimer = new Timer(Constants.PIERCE_ABILITY_CD);
        boostTimer = new Timer(Constants.RANGER_BOOST_TIME);
        specialCDTimer = new Timer(Constants.RANGER_BOOST_CD);
        secondaryCDTimer = new Timer(Constants.EXP_ARROW_CD);
        boostTimer.Register(BoostTimerFinished);
        pierceShootWindow.Register(PierceWindowFinished);

        // Loads sounds
        mainAbilitySound = GameManager.Instance.GameSounds[Constants.RANGER_SHOOT_SND];
        secondaryAbilitySound = mainAbilitySound;
        powerAbilitySound = mainAbilitySound;
        specialAbilitySound = GameManager.Instance.GameSounds[Constants.RANGER_BOOST_SND];
        if (timerBars != null)
        {
            pierceBar = timerBars[0];
            boostBar = timerBars[1];
        }
        base.Initialize(targetTag, energyChanged, healthBar, timerBars, healthMult);
        Energy = maxEnergy;
    }

    /// <summary>
    /// Updates the character; not called on normal update cycle, called by controller
    /// </summary>
    public override void UpdateChar()
    {
        base.UpdateChar();

        // Updates energy
        if (Energy < maxEnergy)
        { Energy = Mathf.Min(maxEnergy, Energy + (Constants.RANGER_REGEN * energyRegenMult * Time.deltaTime)); }

        // Updates timers
        pierceShootCD.Update();
        pierceShootWindow.Update();
        boostTimer.Update();

        // Updates ability bars
        try
        {
            boostBar.fillAmount = 1 - (boostTimer.ElapsedSeconds / boostTimer.TotalSeconds);
            pierceBar.fillAmount = 1 - (pierceShootWindow.ElapsedSeconds / pierceShootWindow.TotalSeconds);
        }
        catch (System.NullReferenceException) { }
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    public override void FireMainAbility()
    {
        ProjScript projectile = FireStraightProjectileAttack(arrow, Constants.BASIC_ARROW_COST, gCDTimer, Constants.BASIC_ARROW_DAMAGE * arrowDamageMult,
            Constants.BASIC_ARROW_SPEED * arrowSpeedMult);
        if (projectile != null)
        { GameManager.Instance.PlaySoundPitched(audioSource, mainAbilitySound); }
    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    public override void FireSecondaryAbility()
    {
        if (!secondaryCDTimer.IsRunning)
        {
            ProjScript projectile = FireStraightProjectileAttack(expArrow, Constants.EXP_ARROW_COST, gCDTimer, Constants.EXP_ARROW_DAMAGE * arrowDamageMult,
                Constants.EXP_ARROW_SPEED * arrowSpeedMult);
            if (projectile != null)
            {
                secondaryCDTimer.Start();
                GameManager.Instance.PlaySoundPitched(audioSource, secondaryAbilitySound);
            }
        }
    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    public override void FirePowerAbility()
    {
        // Fires piercing arrow ability if possible
        if (!powerCDTimer.IsRunning && !pierceShootCD.IsRunning)
        {
            // Starts window if this is the first shot
            if (!pierceShootWindow.IsRunning)
            { pierceShootWindow.Start(); }

            // Fires arrow
            ProjScript projectile = FireStraightProjectileAttack(pierceArrow, Constants.PIERCE_ARROW_COST, pierceShootCD, Constants.PIERCE_ARROW_DAMAGE *
                arrowDamageMult, Constants.PIERCE_ARROW_SPEED * arrowSpeedMult);
            if (projectile != null)
            { GameManager.Instance.PlaySoundPitched(audioSource, mainAbilitySound); }
        }
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    public override void FireSpecialAbility()
    {
        if (!specialCDTimer.IsRunning)
        {
            // Change multipliers
            moveSpeed = Constants.RANGER_MOVE_SPEED * Constants.RANGER_BOOST_MOVE_MULT;
            jumpSpeed = Constants.RANGER_JUMP_SPEED * Constants.RANGER_BOOST_JUMP_MULT;
            cooldownMult = Constants.RANGER_BOOST_CD_MULT;
            arrowSpeedMult = Constants.RANGER_BOOST_ARROW_SPEED_MULT;
            arrowDamageMult = Constants.RANGER_BOOST_ARROW_DAMAGE_MULT;
            energyRegenMult = Constants.RANGER_BOOST_ENERGY_REGEN_MULT;

            GameManager.Instance.SpawnParticle(Constants.RANGER_BOOST_PART, Arm.transform.position).transform.SetParent(transform);
            audioSource.PlayOneShot(specialAbilitySound);
            boostTimer.Start();
            specialCDTimer.Start();
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles the pierce ability window finishing
    /// </summary>
    protected void PierceWindowFinished()
    {
        powerCDTimer.Start();
    }

    /// <summary>
    /// Handles the boost timer finishing
    /// </summary>
    protected void BoostTimerFinished()
    {
        // Change multipliers
        moveSpeed = Constants.RANGER_MOVE_SPEED;
        jumpSpeed = Constants.RANGER_JUMP_SPEED;
        cooldownMult = 1;
        arrowSpeedMult = 1;
        arrowDamageMult = 1;
        energyRegenMult = 1;
    }

    /// <summary>
    /// Resets the cooldown timer lengths
    /// </summary>
    protected void UpdateTimerLengths()
    {
        gCDTimer.TotalSeconds = Constants.RANGER_GCD * cooldownMult;
        pierceShootCD.TotalSeconds = Constants.PIERCE_SHOOT_CD * cooldownMult;
        powerCDTimer.TotalSeconds = Constants.PIERCE_ABILITY_CD * cooldownMult;
    }

    #endregion
}
