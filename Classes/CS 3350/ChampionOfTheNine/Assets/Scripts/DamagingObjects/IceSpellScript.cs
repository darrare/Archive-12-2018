using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script that controls an ice spell
/// </summary>
public class IceSpellScript : ProjScript
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
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            GameManager.Instance.SpawnParticle(Constants.ICE_PART, transform.position); 
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected void Start()
    {
        GameManager.Instance.SpawnParticle(Constants.ICE_PART, transform.position);
    }

    #endregion
}
