using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterScript : RecycledEffectScript
{

    [SerializeField]
    LayerMask layerMask;
    Rigidbody2D rbody;
    Collider2D col;

    protected override void Awake()
    {
        base.Awake();
        rbody = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
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
    public override RecycledEffectScript Initialize(EffectRecyclerType type, Vector3 startPosition, float startRotation, Vector3 velocity, float rotationSpeed, float timeUntilDeletion, float timeUntilFadeStart)
    {
        transform.position = startPosition;
        transform.eulerAngles = new Vector3(0, 0, startRotation);
        rbody.velocity = velocity;
        return this;
    }

    /// <summary>
    /// Updates the blood spatter
    /// </summary>
    void Update()
    {
        //Eventually probably want to rotate it in the direction it is falling or something, idk
    }

    /// <summary>
    /// Called whenever something collides with this object
    /// </summary>
    /// <param name="collider">The collider we collided with</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Destroy(rbody);
            Destroy(col);
            Destroy(this);
        }
    }

    /// <summary>
    /// Called when the effect has finished. used to close up anything that needs to be stopped while the object is inactive.
    /// Anything that is stopped needs to be reenabled in the initialize method.
    /// </summary>
    protected override void EffectFinished()
    {
        //We don't want this object to be recycled as it should always stay there, so do nothing
    }
}
