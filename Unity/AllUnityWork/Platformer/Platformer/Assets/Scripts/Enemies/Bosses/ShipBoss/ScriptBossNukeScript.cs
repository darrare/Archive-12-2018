using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBossNukeScript : RecycledEffectScript
{
    delegate void Behaviour();
    Behaviour Explode;

    float health;

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
        Type = type;
        Renderer.color = Color.white;
        transform.position = startPosition;
        Velocity = velocity;
        GetComponent<Collider2D>().enabled = true;

        if (type == EffectRecyclerType.MiniNuke)
        {
            Explode = MiniExplosion;
            transform.localScale = Vector3.one * .5f;
            health = Constants.SHIP_BOSS_MINI_NUKE_HP;
        }
        else
        {
            Explode = BigExplosion;
            transform.localScale = Vector3.one * 2f;
            health = Constants.SHIP_BOSS_NUKE_HP;
        }

        AudioManager.Instance.PlayOneShot(SoundEffect.NukeFall, 1f);

        return this;
    }



    /// <summary>
    /// Update the nuke
    /// </summary>
    void Update()
    {
        Velocity += Constants.SHIP_BOSS_NUKE_DROP_ACCELERATION * Time.deltaTime;
        transform.position += Velocity * Time.deltaTime;
    }

    /// <summary>
    /// Mini nuke explosion
    /// </summary>
    void MiniExplosion()
    {
        EffectManager.Instance.CreateExplosion(transform.position, 3f, Constants.SHIP_BOSS_MINI_NUKE_EXPLOSION_DURATION, Constants.SHIP_BOSS_MINI_NUKE_EXPLOSION_FADE_TIME);
        GameManager.Instance.Camera.ApplyCameraShake(Constants.SHIP_BOSS_NUKE_CAMERA_SHAKE_INTENSITY, Constants.SHIP_BOSS_NUKE_CAMERA_SHAKE_DURATION);
        EffectFinished();
    }

    /// <summary>
    /// Big nuke explosion
    /// </summary>
    void BigExplosion()
    {
        EffectManager.Instance.CreateExplosion(transform.position, 13f, Constants.SHIP_BOSS_NUKE_EXPLOSION_DURATION, Constants.SHIP_BOSS_NUKE_EXPLOSION_FADE_TIME);
        GameManager.Instance.Camera.ApplyCameraShake(Constants.SHIP_BOSS_NUKE_CAMERA_SHAKE_INTENSITY, Constants.SHIP_BOSS_NUKE_CAMERA_SHAKE_DURATION);
        EffectFinished();
    }

    /// <summary>
    /// Whenever this collides with an object
    /// </summary>
    /// <param name="collider">Collider we collided wtih</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "SolidObject")
        {
            Explode();
        }
    }

    /// <summary>
    /// Takes damage
    /// </summary>
    /// <param name="amount">the amount of damage to take</param>
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            //TODO: do something when the thing is destroyed to siginify it has been destroyed
            EffectFinished();
        }
    }

    /// <summary>
    /// Called when the effect has finished. used to close up anything that needs to be stopped while the object is inactive.
    /// Anything that is stopped needs to be reenabled in the initialize method.
    /// </summary>
    protected override void EffectFinished()
    {
        IsActive = false;
        Renderer.color = Clear;
        GetComponent<Collider2D>().enabled = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }
}
