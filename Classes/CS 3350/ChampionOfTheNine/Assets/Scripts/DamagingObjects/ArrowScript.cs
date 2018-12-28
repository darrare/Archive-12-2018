using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls basic ranger arrows
/// </summary>
public class ArrowScript : ProjScript
{
    #region Protected Methods

    /// <summary>
    /// Handles the projectile colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (hit != HitType.None)
        {
            if (hit == HitType.Target)
            { GameManager.Instance.SpawnParticle(Constants.BLOOD_PART, transform.position); }
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Destroy(gameObject); 
        }
    }

    #endregion
}
