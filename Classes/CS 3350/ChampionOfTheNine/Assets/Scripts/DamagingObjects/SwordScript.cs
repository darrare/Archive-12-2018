using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a a warrior's sword
/// </summary>
public class SwordScript : DamagingObjectScript
{
    #region Fields

    BoxCollider2D boxCollider;

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the object
    /// </summary>
    /// <param name="damage">the damage</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    /// <param name="damageHandler">the delegate to call when the object damages something (optional)</param>
    public override void Initialize(float damage, string targetTag, DamageHandler damageHandler = null)
    {
        base.Initialize(damage, targetTag, damageHandler);
        boxCollider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Updates the object while it isn't paused
    /// </summary>
    protected override void NotPausedUpdate()
    { }

    /// <summary>
    /// Handles the sword continuing to collide with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (hit == HitType.Target)
        { GameManager.Instance.SpawnParticle(Constants.BLOOD_PART, boxCollider.bounds.center); }
    }

    #endregion
}
