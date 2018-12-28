using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls piercing ranger arrows
/// </summary>
public class PierceArrowScript : ProjScript
{
    #region Protected Methods

    /// <summary>
    /// Handles the projectile colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (hit == HitType.Target)
        { GameManager.Instance.SpawnParticle(Constants.BLOOD_PART, transform.position); }
        else if (hit == HitType.Ground)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Destroy(gameObject); 
        }
    }

    #endregion
}
