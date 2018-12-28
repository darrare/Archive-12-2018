using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script that controls a meteor spell
/// </summary>
public class MeteorSpellScript : ProjScript
{
    #region Fields

    [SerializeField]Transform meteor;
    [SerializeField]GameObject explosion;

    #endregion

    #region Protected Methods

    /// <summary>
    /// Updates the projectile's angle
    /// </summary>
    protected override void UpdateAngle()
    {
        // Rotates the meteor
        meteor.Rotate(0, 0, Constants.METEOR_ROT_SPEED * Time.deltaTime);
    }

    /// <summary>
    /// Handles the projectile colliding with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (hit == HitType.Ground)
        {
            ExplosionScript explosionScript = ((GameObject)Instantiate(explosion, transform.position, transform.localRotation)).GetComponent<ExplosionScript>();
            explosionScript.Initialize(Damage, targetTag);
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Destroy(gameObject);
        }
    }

    #endregion
}
