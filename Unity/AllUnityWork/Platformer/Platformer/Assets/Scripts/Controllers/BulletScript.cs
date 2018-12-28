using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : RecycledEffectScript
{
    float randomAngleForShrapanel;

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
        GetComponent<BoxCollider2D>().enabled = true;
        LifeSpanTimer = 0;
        Type = type;
        transform.position = startPosition;
        transform.eulerAngles = new Vector3(0, 0, startRotation);
        Velocity = velocity;
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
        if (collision.tag == "SolidObject")
        {
            AudioManager.Instance.PlayOneShot(SoundEffect.BulletHitWall, .3f);
            for (int i = 0; i < Constants.SHRAPNEL_NUM_TO_CREATE; i++)
            {
                randomAngleForShrapanel = Random.Range(0f, 360f);
                EffectManager.Instance.AddNewEffect(EffectRecyclerType.Shrapnel, transform.position, randomAngleForShrapanel, new Vector2(Mathf.Cos(randomAngleForShrapanel * Mathf.Deg2Rad), Mathf.Sin(randomAngleForShrapanel * Mathf.Deg2Rad)).normalized, 0, Constants.SHRAPNEL_TIME_UNTIL_DELETION, Constants.SHRAPNEL_TIME_UNTIL_FADE);
            }

            EffectFinished();
        }
        else if (collision.tag == "Enemy")
        {
            GenerateBloodSplatter((transform.position - collision.transform.position).normalized);
            collision.GetComponent<Health>().TakeDamage(Constants.GUN_DAMAGE);

            AudioManager.Instance.PlayOneShot(SoundEffect.BulletHitWall, .3f);
            for (int i = 0; i < Constants.SHRAPNEL_NUM_TO_CREATE; i++)
            {
                randomAngleForShrapanel = Random.Range(0f, 360f);
                EffectManager.Instance.AddNewEffect(EffectRecyclerType.Shrapnel, transform.position, randomAngleForShrapanel, new Vector2(Mathf.Cos(randomAngleForShrapanel * Mathf.Deg2Rad), Mathf.Sin(randomAngleForShrapanel * Mathf.Deg2Rad)).normalized, 0, Constants.SHRAPNEL_TIME_UNTIL_DELETION, Constants.SHRAPNEL_TIME_UNTIL_FADE);
            }

            EffectFinished();
        }
        else if (collision.tag == "Nuke")
        {
            GenerateBloodSplatter((transform.position - collision.transform.position).normalized);
            collision.GetComponent<ScriptBossNukeScript>().TakeDamage(Constants.GUN_DAMAGE);

            AudioManager.Instance.PlayOneShot(SoundEffect.BulletHitWall, .3f);
            for (int i = 0; i < Constants.SHRAPNEL_NUM_TO_CREATE; i++)
            {
                randomAngleForShrapanel = Random.Range(0f, 360f);
                EffectManager.Instance.AddNewEffect(EffectRecyclerType.Shrapnel, transform.position, randomAngleForShrapanel, new Vector2(Mathf.Cos(randomAngleForShrapanel * Mathf.Deg2Rad), Mathf.Sin(randomAngleForShrapanel * Mathf.Deg2Rad)).normalized, 0, Constants.SHRAPNEL_TIME_UNTIL_DELETION, Constants.SHRAPNEL_TIME_UNTIL_FADE);
            }

            EffectFinished();
        }
    }

    /// <summary>
    /// Generates the blood splatter effect when you shoot an enemy
    /// </summary>
    void GenerateBloodSplatter(Vector2 direction)
    {
        for (int i = 0; i < Constants.BLOOD_SPLATTER_COUNT; i++)
        {
            EffectManager.Instance.AddNewEffect(EffectRecyclerType.BloodSplatter, transform.position, 0f, Utilities.RotateDegrees(direction, Random.Range(-Constants.BLOOD_SPLATTER_RANDOM_ANGLE_RANGE, Constants.BLOOD_SPLATTER_RANDOM_ANGLE_RANGE)) * Random.Range(Constants.BLOOD_SPLATTER_INIT_SPEED / 2, Constants.BLOOD_SPLATTER_INIT_SPEED), 0, 0, 0);
        }
    }

    /// <summary>
    /// Updates the bullet
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
        IsActive = false;
        Renderer.color = Clear;
        IsCoroutineRunning = false;
        GetComponent<BoxCollider2D>().enabled = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }
}
