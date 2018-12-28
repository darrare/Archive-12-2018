using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the warrior's throwing axe
/// </summary>
public class AxeScript : ProjScript
{
    #region Fields

    [SerializeField]GameObject pickup;

    #endregion

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

    /// <summary>
    /// Updates the projectile's angle
    /// </summary>
    protected override void UpdateAngle()
    {
        // Rotates the axe
        transform.Rotate(0, 0, Constants.AXE_ROT_SPEED * Time.deltaTime);
    }

    /// <summary>
    /// Creates the axe pickup when the axe is destroyed
    /// </summary>
    protected void OnDestroy()
    {
        Instantiate(pickup, transform.position, pickup.transform.rotation);
    }

    #endregion
}
