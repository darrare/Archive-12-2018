using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script that controls AI characters
/// </summary>
public abstract class AIScript : CharacterControllerScript
{
    #region Fields

    protected float targetRange;
    protected GameObject target;
    [SerializeField]Transform lineStart;
    [SerializeField]Transform lineEnd;

    #endregion

    #region Properties

    /// <summary>
    /// Returns the tag of this character's target
    /// </summary>
    protected override string TargetTag
    { get { return Constants.PLAYER_TAG; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the character dying
    /// </summary>
    public override void Death()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start()
    {
        base.Initialize(null, null);
        InvokeRepeating("FindTarget", Constants.AI_SCAN_DELAY, Constants.AI_SCAN_DELAY);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void NotPausedUpdate()
    {
        base.NotPausedUpdate();

        // Finds a new target if it doesn't have one
        if (target == null)
        { FindTarget(); }
        else
        {
            if (Vector2.Distance(transform.position, target.transform.position) > targetRange)
            {
                // Out of range, move towards target
                if (Physics2D.Linecast(lineStart.position, lineEnd.position, 1 << Constants.GROUND_LAYER) && character.Grounded)
                { character.Jump(); }
                float direction = Mathf.Sign(target.transform.position.x - transform.position.x);
                character.Move(direction);
                character.SetArmAngle(90 - (direction * 135));
            }
            else
            {
                // In range, attack
                character.Move(0);
                Attack();
            }
        }
    }

    /// <summary>
    /// Attacks the target
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    /// Finds the nearest target, if any
    /// </summary>
    protected void FindTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(TargetTag);
        float nearestDistanceSqr = Mathf.Infinity;
        foreach (GameObject obj in targets) 
        {
            float distanceSqr = (obj.transform.position - transform.position).sqrMagnitude;
            if (distanceSqr < nearestDistanceSqr) 
            {
                target = obj;
                nearestDistanceSqr = distanceSqr;
            }
        }
    }

    #endregion
}
