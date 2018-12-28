using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFireScript : RecycledEffectScript
{
    Vector2 originalPos;
    float damageValue; //this will default to Constants.EXPLOSION_FIRE_DAMAGE if not set by EffectManager.CreateExplosion();

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
        originalPos = startPosition;
        transform.eulerAngles = new Vector3(0, 0, startRotation);
        Velocity = velocity;
        RotationSpeed = rotationSpeed;
        TimeUntilDeletion = timeUntilDeletion;
        GetComponent<Collider2D>().enabled = true;
        if (timeUntilFadeStart != -1f)
            TimeUntilFadeStart = timeUntilFadeStart;
        else
            TimeUntilFadeStart = TimeUntilDeletion;

        AudioManager.Instance.PlayOneShot(SoundEffect.Explosion, .5f);

        return this;
    }

    /// <summary>
    /// Sets the damage value of this fire
    /// </summary>
    /// <param name="damageValue">The amount of damage this fire should do</param>
    /// <param name="scale">The scale of the explosion (so we can make smaller ones)</param>
    public void SecondInitialize(float damageValue, float scale = 1)
    {
        this.damageValue = damageValue;
        transform.localScale = Vector3.one * scale;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            transform.position = originalPos + Random.insideUnitCircle * .1f;

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
        GetComponent<Collider2D>().enabled = false;
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
    /// Called when the effect has finished. used to close up anything that needs to be stopped while the object is inactive.
    /// Anything that is stopped needs to be reenabled in the initialize method.
    /// </summary>
    protected override void EffectFinished()
    {
        IsActive = false;
        GetComponent<Collider2D>().enabled = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }

    /// <summary>
    /// Typical on trigger enter
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject == GameManager.Instance.Player.gameObject)
        {
            GameManager.Instance.Player.DamageTaken(damageValue, transform.position);
        }
    }
}
