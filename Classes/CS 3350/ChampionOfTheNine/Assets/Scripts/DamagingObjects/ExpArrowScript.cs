using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls exploding ranger arrows
/// </summary>
public class ExpArrowScript : ProjScript
{
    #region Fields

    [SerializeField]GameObject explosion;

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
            ExplosionScript explosionScript = ((GameObject)Instantiate(explosion, transform.position, transform.localRotation)).GetComponent<ExplosionScript>();
            explosionScript.Initialize(Damage, targetTag);
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Destroy(gameObject);
        }
    }

    #endregion
}
