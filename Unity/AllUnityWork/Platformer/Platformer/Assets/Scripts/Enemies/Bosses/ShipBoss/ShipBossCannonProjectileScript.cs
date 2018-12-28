using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBossCannonProjectileScript : RecycledEffectScript
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
        Velocity = velocity;
        TimeUntilDeletion = timeUntilDeletion;
        if (timeUntilFadeStart != -1f)
            TimeUntilFadeStart = timeUntilFadeStart;
        else
            TimeUntilFadeStart = TimeUntilDeletion;

        return this;
    }

    /// <summary>
    /// Called when this effect is finished
    /// </summary>
    protected override void EffectFinished()
    {
        IsActive = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }

    /// <summary>
    /// Update the projectile
    /// </summary>
    void Update ()
    {
        if (IsActive)
        {
            transform.position += Velocity * Constants.SHIP_BOSS_CANNON_PROJECTILE_SPEED * Time.deltaTime;

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
        Renderer.color = Clear;
        IsCoroutineRunning = false;
        EffectFinished();
    }

    /// <summary>
    /// Whenever this collides with something
    /// </summary>
    /// <param name="col">The collider we collided with</param>
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameManager.Instance.Player.DamageTaken(Constants.SHIP_BOSS_CANNON_DAMAGE, transform.position);
        }
    }

}
