using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBossTendrilPieceScript : RecycledEffectScript
{
    Color? color;

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
        if (color == null)
        {
            color = Renderer.color;
        }
        else
        {
            Renderer.color = (Color)color;
        }

        IsActive = true;
        GetComponent<HingeJoint2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        transform.position = startPosition;
        Type = type;
        transform.eulerAngles = new Vector3(0, 0, startRotation);
        return this;
    }

    /// <summary>
    /// Called from the EyeBossTendrilScript to end this specific effect
    /// </summary>
    public void EndEffect()
    {
        EffectFinished();
    }

    /// <summary>
    /// Called when the effect is finished
    /// </summary>
    protected override void EffectFinished()
    {
        IsActive = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Renderer.color = Clear;
        if (GetComponents<HingeJoint2D>().Length == 2)
        {
            DestroyImmediate(GetComponents<HingeJoint2D>()[1]);
        }
        if (GetComponent<SpringJoint2D>())
        {
            DestroyImmediate(GetComponent<SpringJoint2D>());
        }
        GetComponent<HingeJoint2D>().enabled = false;
        EffectManager.Instance.EffectFinished(Type, this);
    }
}
