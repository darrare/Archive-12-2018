using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script for objects that can be paused
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class PauseableObjectScript : MonoBehaviour
{
    #region Fields

    protected Rigidbody2D rbody;
    protected Animator anim;
    Vector2 pausedVelocity = Vector2.zero;
    bool paused = false;
    bool defaultKinematic;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets whether the object is paused
    /// </summary>
    public bool Paused
    {
        get { return paused; }
        set
        {
            if (value)
            {
                pausedVelocity = rbody.velocity;
                rbody.velocity = Vector2.zero;
            }
            else
            { rbody.velocity = pausedVelocity; }
            rbody.isKinematic = value || defaultKinematic;
            paused = value;
            if (anim != null)
            { anim.enabled = !value; }
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the object
    /// </summary>
    protected virtual void Initialize()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        defaultKinematic = rbody.isKinematic;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected virtual void Update()
    {
        if (!paused)
        { NotPausedUpdate(); }
    }

    /// <summary>
    /// Updates the object while it isn't paused
    /// </summary>
    protected abstract void NotPausedUpdate();

    #endregion
}
