using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecycledEffectScript : MonoBehaviour
{
    protected EffectRecyclerType Type;
    protected Vector3 Velocity; //Will always treat Z property as 0
    protected float RotationSpeed;
    protected float TimeUntilDeletion;
    protected float TimeUntilFadeStart;
    protected float LifeSpanTimer;
    protected SpriteRenderer Renderer;
    protected bool IsActive;
    protected Color Clear = new Color(1, 1, 1, 0);
    protected bool IsCoroutineRunning = false;

    /// <summary>
    /// Initializes whatever this is once
    /// </summary>
    protected virtual void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

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
    public abstract RecycledEffectScript Initialize(EffectRecyclerType type, Vector3 startPosition, float startRotation, Vector3 velocity, float rotationSpeed, float timeUntilDeletion, float timeUntilFadeStart);

    /// <summary>
    /// Called when the effect has finished. used to close up anything that needs to be stopped while the object is inactive.
    /// Anything that is stopped needs to be reenabled in the initialize method.
    /// </summary>
    protected abstract void EffectFinished();

    /// <summary>
    /// If for whatever reason the effect needs to go away more quickly
    /// </summary>
    public virtual void EndEffectEarly()
    {
        EffectFinished();
    }
}
