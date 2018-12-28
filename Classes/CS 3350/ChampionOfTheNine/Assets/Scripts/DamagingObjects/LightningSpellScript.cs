using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script that controls a lightning spell
/// </summary>
public class LightningSpellScript : ProjScript
{
    /// <summary>
    /// Updates the projectile's angle
    /// </summary>
    protected override void UpdateAngle()
    { }

    /// <summary>
    /// Handles the lightning continuing to collide with something
    /// </summary>
    /// <param name="other">the other collider</param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
