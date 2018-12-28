using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBossProjectileScript : RecycledEffectScript
{
    /// <summary>
    /// Initializes the effect
    /// </summary>
    /// <param name="type">The type of effect this is (used for calling methods in EffectManager)</param>
    /// <param name="startPosition">The starting position of the object</param>
    /// <param name="startRotation">The starting rotation of the object</param>
    /// <param name="velocity">The initial velocity of the effect (private update methods will handle any weird changes)</param>
    /// <param name="rotationSpeed">A rotation speed if one is required. 0 otherwise</param>
    /// <param name="timeUntilDeletion">The time that this object should be alive. After this point, this object is hidden and added to the InactiveEffects list in EffectManager</param>
    /// <param name="timeUntilFadeStart">The time until this object starts to fade out, if this object has a fade to it.</param>
    public override RecycledEffectScript Initialize(EffectRecyclerType type, Vector3 startPosition, float startRotation, Vector3 velocity, float rotationSpeed, float timeUntilDeletion, float timeUntilFadeStart)
    {
        IsActive = true;
        Renderer.color = Color.white;
        LifeSpanTimer = 0;
        Type = type;
        transform.position = startPosition;
        transform.eulerAngles = new Vector3(0, 0, startRotation);
        Velocity = velocity * Constants.EYEBALL_BOSS_PROJECTILE_SPEED;
        RotationSpeed = rotationSpeed;
        TimeUntilDeletion = timeUntilDeletion;
        if (timeUntilFadeStart != -1f)
            TimeUntilFadeStart = timeUntilFadeStart;
        else
            TimeUntilFadeStart = TimeUntilDeletion;

        return this;
    }

    /// <summary>
    /// whenever something enters this trigger
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "SolidObject" && Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) < Constants.EYEBALL_BOSS_PROJECTILE_DISTANCE_FROM_PLAYER_TO_EXPLODE) || collision.gameObject == GameManager.Instance.Player.gameObject)
        {
            EffectFinished();
        }
    }

    /// <summary>
    /// Updates the spawn
    /// </summary>
    void Update()
    {
        if (IsActive)
        {
            transform.position += Velocity * Constants.GUN_BULLET_SPEED * Time.deltaTime;

            LifeSpanTimer += Time.deltaTime;
            if (!IsCoroutineRunning && LifeSpanTimer >= TimeUntilFadeStart)
            {
                StartCoroutine(FadeOut());
            }
        }
    }

    /// <summary>
    /// Fades out the sprite over TimeUntilDeletion - TimeUntilFadeStarts
    /// </summary>
    IEnumerator FadeOut()
    {
        IsCoroutineRunning = true;
        while (LifeSpanTimer < TimeUntilDeletion)
        {
            float tempVal = (LifeSpanTimer - TimeUntilFadeStart) / (TimeUntilDeletion - TimeUntilFadeStart);
            Renderer.color = Color.Lerp(Color.white, Clear, (LifeSpanTimer - TimeUntilFadeStart) / (TimeUntilDeletion - TimeUntilFadeStart));
            yield return null;
        }
        EffectFinished();
    }

    /// <summary>
    /// Called when the effect has finished. used to close up anything that needs to be stopped while the object is inactive.
    /// Anything that is stopped needs to be reenabled in the initialize method.
    /// </summary>
    protected override void EffectFinished()
    {
        EffectManager.Instance.CreateExplosion(transform.position, Constants.EYEBALL_BOSS_PROJECTILE_EXPLOSION_SCALE, Constants.EYEBALL_BOSS_PROJECTILE_EXPLOSION_DURATION, Constants.EYEBALL_BOSS_PROJECTILE_EXPLOSION_TIME_UNTIL_FADE, Constants.EYEBALL_BOSS_PROJECTILE_EXPLOSION_DAMAGE);
        IsActive = false;
        Renderer.color = Clear;
        IsCoroutineRunning = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }

    /// <summary>
    /// Called whenever the health script reaches 0
    /// </summary>
    public void DestroyProjectile()
    {
        EffectFinished();
    }
}
