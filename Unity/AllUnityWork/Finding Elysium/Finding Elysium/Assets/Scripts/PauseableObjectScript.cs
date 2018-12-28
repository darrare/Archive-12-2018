using UnityEngine;

/// <summary>
/// Abstract parent script for objects that can be paused
/// </summary>
public abstract class PauseableObjectScript : MonoBehaviour
{
    #region Fields

    protected Rigidbody rbody;
    Vector2 pausedVelocity = Vector2.zero;
    bool paused;
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
            if (rbody)
            {
                if (value)
                {
                    pausedVelocity = rbody.velocity;
                    rbody.velocity = Vector2.zero;
                }
                else
                { rbody.velocity = pausedVelocity; }
                rbody.isKinematic = value || defaultKinematic;
            }
            paused = value;
            if (Anim != null)
            { Anim.enabled = !value; }
        }
    }

    /// <summary>
    /// Gets the object's animator
    /// </summary>
    public Animator Anim { get; private set; }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the object
    /// </summary>
    public virtual void Initialize()
    {
        rbody = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();
        if (rbody)
        { defaultKinematic = rbody.isKinematic; }
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
