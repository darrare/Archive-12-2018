using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script for a mage character
/// </summary>
public class MageScript : CharacterScript
{
    #region Fields

    [SerializeField]GameObject ice;
    [SerializeField]GameObject meteor;
    [SerializeField]GameObject lightning;
    [SerializeField]GameObject beam;
    [SerializeField]AudioSource lightningSound;

    LightningSpellScript lightningProj = null;
    Timer lightningTimer;
    Timer drainTimer;
    List<SpriteRenderer> beams;
    List<DamagableObjectScript> drainTargets;
    Dictionary<Color, Color> beamColors;

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
        maxHealth = Constants.MAGE_HEALTH;
        moveSpeed = Constants.MAGE_MOVE_SPEED;
        jumpSpeed = Constants.MAGE_JUMP_SPEED;
        maxEnergy = Constants.MAGE_ENERGY;
        gCDTimer = new Timer(Constants.MAGE_GCD);
        secondaryCDTimer = new Timer(Constants.LIGHTNING_CD);
        powerCDTimer = new Timer(Constants.METEOR_CD);
        specialCDTimer = new Timer(Constants.DRAIN_CD);
        lightningTimer = new Timer(Constants.LIGHTNING_CAST_TIME);
        lightningTimer.Register(LightningTimerFinished);
        beams = new List<SpriteRenderer>();
        drainTimer = new Timer(Constants.DRAIN_TIME);
        drainTimer.Register(DrainTimerFinished);
        drainTargets = new List<DamagableObjectScript>();
        beamColors = new Dictionary<Color, Color>();
        beamColors.Add(Constants.BEAM_COLOR_1, Constants.BEAM_COLOR_2);
        beamColors.Add(Constants.BEAM_COLOR_2, Constants.BEAM_COLOR_1);

        // Loads sounds
        mainAbilitySound = GameManager.Instance.GameSounds[Constants.ICE_CAST_SND];
        secondaryAbilitySound = GameManager.Instance.GameSounds[Constants.LIGHTNING_CAST_SND];
        powerAbilitySound = GameManager.Instance.GameSounds[Constants.METEOR_CAST_SND];
        specialAbilitySound = GameManager.Instance.GameSounds[Constants.DRAIN_SND];
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
        { Energy = Mathf.Min(maxEnergy, Energy + (Constants.MAGE_REGEN * Time.deltaTime)); }

        // Updates lightning
        if (lightningProj != null)
        {
            if (Energy >= Constants.LIGHTNING_COST_PER_SEC * Time.deltaTime)
            {
                lightningProj.SetLocationAndDirection(FireLocation, ArmAngle);
                Energy -= (Constants.LIGHTNING_COST_PER_SEC * Time.deltaTime);
                lightningTimer.Update();
            }
            else
            { lightningTimer.Finish(); }
        }

        // Updates beams
        if (drainTimer.IsRunning)
        {
            // Updates flash
            if (drainTimer.ElapsedSeconds % Constants.DRAIN_FLASH_TIME < Time.deltaTime)
            {
                foreach (SpriteRenderer beam in beams)
                { beam.color = beamColors[beam.color]; }
            }

            // Damages targets and increases mana
            for (int i = drainTargets.Count - 1; i >= 0; i--)
            {
                if (drainTargets[i].Damage(Constants.DRAIN_DAMAGE * (Time.deltaTime / Constants.DRAIN_TIME)))
                { Energy = Mathf.Min(maxEnergy, Energy + (Constants.DRAIN_MANA_PER_TARGET * (Time.deltaTime / Constants.DRAIN_TIME))); }
                else
                { drainTargets.RemoveAt(i); }
            }

            drainTimer.Update();
        }
    }

    /// <summary>
    /// Fires the character's main ability
    /// </summary>
    public override void FireMainAbility()
    {
        ProjScript projectile = FireStraightProjectileAttack(ice, Constants.ICE_COST, gCDTimer, Constants.ICE_DAMAGE, Constants.ICE_SPEED);
        if (projectile != null)
        { GameManager.Instance.PlaySoundPitched(audioSource, mainAbilitySound); }
    }

    /// <summary>
    /// Fires the character's secondary ability
    /// </summary>
    public override void FireSecondaryAbility()
    {
        if (!gCDTimer.IsRunning)
        {
            lightningTimer.Start();
            if (lightningProj == null)
            {
                lightningProj = Instantiate<GameObject>(lightning).GetComponent<LightningSpellScript>();
                lightningProj.Initialize(FireLocation, ArmAngle, targetTag, Constants.LIGHTNING_DAMAGE, 0);
                lightningSound.Play();
            }
        }
    }

    /// <summary>
    /// Fires the character's power ability
    /// </summary>
    public override void FirePowerAbility()
    {
        if (!powerCDTimer.IsRunning)
        {
            ProjScript projectile = FireProjectileAttack(meteor, Constants.METEOR_COST, gCDTimer);
            if (projectile != null)
            {
                GameManager.Instance.PlaySoundPitched(audioSource, powerAbilitySound);
                powerCDTimer.Start();
                Vector2 shotLocation = (Vector2)transform.position + Constants.METEOR_START_LOC;
                projectile.Initialize(shotLocation, Utilities.GetAngleDegrees(shotLocation, Utilities.MousePosition), targetTag,
                    Constants.METEOR_DAMAGE, Constants.METEOR_SPEED);
            }
        }
    }

    /// <summary>
    /// Fires the character's special ability
    /// </summary>
    public override void FireSpecialAbility()
    {
        if (!gCDTimer.IsRunning && !specialCDTimer.IsRunning && !drainTimer.IsRunning)
        {
            // Sends beams to all nearby enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject obj in enemies)
            {
                if (Vector2.Distance(FireLocation, obj.transform.position) < Constants.DRAIN_RANGE)
                {
                    drainTargets.Add(obj.GetComponent<DamagableObjectScript>());
                    Vector2 topLocation = new Vector2((obj.transform.position.x - FireLocation.x) / 2, Mathf.Max(obj.transform.position.y -
                        FireLocation.y, 0) + Random.Range(Constants.DRAIN_MIN_HEIGHT, Constants.DRAIN_MAX_HEIGHT)) + FireLocation;
                    Vector2 location = FireLocation;
                    bool goingLeft = FireLocation.x > obj.transform.position.x;

                    // Calculates beam
                    for (float i = 1; i <= Constants.DRAIN_SEGMENTS; i++)
                    { CalcAndSpawnBeam(i, ref location, topLocation, FireLocation, goingLeft); }
                    for (float i = Constants.DRAIN_SEGMENTS - 1; i >= 0; i--)
                    { CalcAndSpawnBeam(i, ref location, topLocation, obj.transform.position, goingLeft); }
                }
            }

            // Activates cooldowns if anything was hit
            if (drainTargets.Count > 0)
            {
                gCDTimer.Start();
                drainTimer.Start();
                specialCDTimer.Start();
                Controllable = false;
            }
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles the lightning timer finishing
    /// </summary>
    protected virtual void LightningTimerFinished()
    {
        Destroy(lightningProj.gameObject);
        lightningProj = null;
        gCDTimer.Start();
        lightningSound.Stop();
    }

    /// <summary>
    /// Handles the drain timer finishing
    /// </summary>
    protected virtual void DrainTimerFinished()
    {
        foreach (SpriteRenderer beam in beams)
        { Destroy(beam.gameObject); }
        beams.Clear();
        drainTargets.Clear();
        Controllable = true;
    }

    /// <summary>
    /// Calculates the position of the next segment of the beam
    /// </summary>
    /// <param name="num">the index of the current beam position</param>
    /// <param name="location">the current beam location</param>
    /// <param name="topLocation">the location of the top of the beam</param>
    /// <param name="endpointLocation">the location of the other end of this half of the beam</param>
    /// <param name="goingLeft">whether or not the beam is going left</param>
    protected void CalcAndSpawnBeam(float num, ref Vector2 location, Vector2 topLocation, Vector2 endpointLocation, bool goingLeft)
    {
        Vector2 oldLocation = location;
        location.x = Mathf.Lerp(endpointLocation.x, topLocation.x, num / Constants.DRAIN_SEGMENTS);
        location.y = Mathf.Lerp(endpointLocation.y, topLocation.y, 1 - Mathf.Pow(0.5f, num));
        float distance = Vector2.Distance(oldLocation, location);
        float angle = Mathf.Asin((location.y - oldLocation.y) / distance) * Mathf.Rad2Deg;
        if (goingLeft)
        { angle = 180 - angle; }
        GameObject beamInst = (GameObject)Instantiate(beam, oldLocation, Quaternion.Euler(0, 0, angle));
        beamInst.transform.localScale = new Vector3(distance, 1, 1);
        beams.Add(beamInst.GetComponent<SpriteRenderer>());
    }

    #endregion
}
