using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script for character controllers
/// </summary>
public abstract class CharacterControllerScript : PauseableObjectScript
{
    #region Fields

    protected CharacterScript character;

    #endregion

    #region Properties

    /// <summary>
    /// Returns the tag of this character's target
    /// </summary>
    protected abstract string TargetTag
    { get; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the character dying
    /// </summary>
    public abstract void Death();

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the character controller
    /// </summary>
    /// <param name="healthBar">the health bar</param>
    /// <param name="timerBars">the timer bar array</param>
    /// <param name="healthMult">the health multiplier</param>
    protected virtual void Initialize(Image healthBar, Image[] timerBars, float healthMult = 1)
    {
        base.Initialize();
        character = GetComponent<CharacterScript>();
        character.Initialize(TargetTag, CharacterEnergyChanged, healthBar, timerBars, healthMult);
    }

    /// <summary>
    /// Updates the object while it isn't paused
    /// </summary>
    protected override void NotPausedUpdate()
    {
        character.UpdateChar();
    }

    /// <summary>
    /// Handles the character's energy level changing
    /// </summary>
    /// <param name="pct">the percentage of energy the player has</param>
    protected virtual void CharacterEnergyChanged(float pct)
    { }

    #endregion
}

#region Delegates

/// <summary>
/// Delegate to handle the character's energy changing
/// </summary>
/// <param name="input">the new value</param>
public delegate void EnergyChangedHandler(float value);

#endregion
