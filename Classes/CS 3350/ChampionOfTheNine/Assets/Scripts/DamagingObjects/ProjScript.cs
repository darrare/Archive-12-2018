using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract parent script that controls projectiles
/// </summary>
public abstract class ProjScript : DamagingObjectScript
{
    #region Fields

    [SerializeField]protected AudioClip hitSound;
    protected float moveSpeed;
    Timer lifeTimer;

    #endregion

    #region Public Methods
    
    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="fromPosition">the position of the projectile</param>
    /// <param name="toPosition">the target position</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    /// <param name="damage">the projectile's damage</param>
    /// <param name="moveSpeed">the projectile's movement speed</param>
    /// <param name="damageHandler">the delegate to call when the object damages something (optional)</param>
    public virtual void Initialize(Vector2 fromPosition, Vector2 toPosition, string targetTag, float damage, float moveSpeed, DamageHandler damageHandler = null)
    {
        Initialize(targetTag, damage, moveSpeed, damageHandler);
        SetLocationAndDirection(fromPosition, toPosition);
    }

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="position">the position of the projectile</param>
    /// <param name="angle">the angle, in degrees</param>
    /// <param name="targetTag">the tag of the targeted characters</param>
    /// <param name="damage">the projectile's damage</param>
    /// <param name="moveSpeed">the projectile's movement speed</param>
    /// <param name="damageHandler">the delegate to call when the object damages something (optional)</param>
    public virtual void Initialize(Vector2 position, float angle, string targetTag, float damage, float moveSpeed, DamageHandler damageHandler = null)
    {
        Initialize(targetTag, damage, moveSpeed, damageHandler);
        SetLocationAndDirection(position, angle);
    }

    /// <summary>
    /// Sets the projectile's position and direction
    /// </summary>
    /// <param name="position">the position of the projectile</param>
    /// <param name="angle">the angle, in degrees</param>
    public void SetLocationAndDirection(Vector2 position, float angle)
    {
        // Sets position and direction
        transform.position = position;
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        rbody.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * moveSpeed, Mathf.Sin(angle * Mathf.Deg2Rad) * moveSpeed);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the projectile
    /// </summary>
    /// <param name="targetTag">the tag of the targeted characters</param>
    /// <param name="damage">the projectile's damage</param>
    /// <param name="moveSpeed">the projectile's movement speed</param>
    /// <param name="damageHandler">the delegate to call when the object damages something (optional)</param>
    protected virtual void Initialize(string targetTag, float damage, float moveSpeed, DamageHandler damageHandler = null)
    {
        base.Initialize(damage, targetTag, damageHandler);
        this.moveSpeed = moveSpeed;
        lifeTimer = new Timer(0);
        if (moveSpeed > 0)
        {
            rbody.velocity = new Vector2(moveSpeed, 0);
            lifeTimer.TotalSeconds = Constants.PROJ_MAX_DIST / moveSpeed;
            lifeTimer.Register(LifeTimerFinished);
            lifeTimer.Start();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void NotPausedUpdate()
    {
        UpdateAngle();
        lifeTimer.Update();
    }

    /// <summary>
    /// Sets the projectile's position and direction
    /// </summary>
    /// <param name="fromPosition">the position of the projectile</param>
    /// <param name="toPosition">the target position</param>
    protected void SetLocationAndDirection(Vector2 fromPosition, Vector2 toPosition)
    {
        // Calculates shot angle
        float shotAngle = Mathf.Asin((toPosition.y - fromPosition.y) / Vector2.Distance(toPosition, fromPosition));
        if (toPosition.x - fromPosition.x < 0)
        { shotAngle = Mathf.PI - shotAngle; }

        // Sets position and direction
        transform.position = fromPosition;
        transform.localRotation = Quaternion.Euler(0, 0, shotAngle * Mathf.Rad2Deg);

        rbody.velocity = new Vector2(Mathf.Cos(shotAngle) * moveSpeed, Mathf.Sin(shotAngle) * moveSpeed);
    }

    /// <summary>
    /// Updates the angle of the projectile
    /// </summary>
    protected virtual void UpdateAngle()
    {
        float shotAngle = Mathf.Atan(rbody.velocity.y / rbody.velocity.x);
        if (rbody.velocity.x < 0)
        { shotAngle -= Mathf.PI; }
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * shotAngle);
    }

    /// <summary>
    /// Handles the life timer finishing
    /// </summary>
    protected virtual void LifeTimerFinished()
    {
        Destroy(gameObject);
    }

    #endregion
}
