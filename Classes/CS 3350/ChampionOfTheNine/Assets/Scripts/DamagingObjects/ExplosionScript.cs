using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls an explosion
/// </summary>
public class ExplosionScript : DamagingObjectScript
{
    /// <summary>
    /// Updates the object while it isn't paused
    /// </summary>
    protected override void NotPausedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        { Destroy(gameObject); }
    }
}
