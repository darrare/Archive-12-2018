using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script that controls an object that damages things
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class DamagingObjectScript : PauseableObjectScript
{
    #region Fields

    float damage;
    protected string targetTag;
    protected HitType hit = HitType.None;
    protected DamageHandler damageHandler;

    #endregion
    
    #region Properties

    /// <summary>
    /// Gets or sets the damage dealt by the object
    /// </summary>
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the object
    /// </summary>
    /// <param name="damage">the damage</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    /// <param name="damageHandler">the delegate to call when the object damages something (optional)</param>
    public virtual void Initialize(float damage, string targetTag, DamageHandler damageHandler = null)
    {
        base.Initialize();
        this.damage = damage;
        this.targetTag = targetTag;
        this.damageHandler = damageHandler;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles the projectile colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!Paused)
        {
            // Checks for if enemy
            if (other.gameObject.tag == targetTag)
            {
                bool targetLived = other.gameObject.GetComponent<DamagableObjectScript>().Damage(damage);
                if (damageHandler != null)
                { damageHandler(targetLived); }
                hit = HitType.Target;
            }
            else if (other.gameObject.layer == Constants.GROUND_LAYER)
            { hit = HitType.Ground; }
            else if (other.gameObject.tag == Constants.ATTACK_TAG && other.gameObject.GetComponent<DamagingObjectScript>().targetTag != targetTag)
            { hit = HitType.TargetAttack; }
            else
            { hit = HitType.None; }
        }
    }

    #endregion
}

#region Damage delegate

/// <summary>
/// Delegate for handling when the object damages something
/// </summary>>
/// <param name="targetLived">whether or not the target lived</param>
public delegate void DamageHandler(bool targetLived);

#endregion
