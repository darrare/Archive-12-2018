using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    protected delegate void Behaviour();
    protected Behaviour behaviour;

    protected float speed;
    protected float health;
    protected bool isAggro = false;
    protected float aggroDistance;

    protected Rigidbody2D rBody;

	/// <summary>
    /// initializes anything on the enemy
    /// </summary>
	protected virtual void Start ()
    {
        rBody = GetComponent<Rigidbody2D>();
	}
	
	/// <summary>
    /// Updates the enemy every frame
    /// </summary>
	protected virtual void Update ()
    {
        if (isAggro)
        {
            behaviour();
        }
        else if (Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position) < aggroDistance)
        {
            OnAggro();
        }
        
	}

    /// <summary>
    /// Called whenever this enemy starts attacking the player
    /// </summary>
    protected virtual void OnAggro()
    {
        isAggro = true;
    }

    /// <summary>
    /// Called whenever this enemy dies
    /// </summary>
    protected virtual void OnDeath()
    {
        isAggro = false;
        GameManager.Instance.Camera.RemoveTransformFromTargets(transform);
    }

    /// <summary>
    /// Deals damage to the enemy
    /// </summary>
    /// <param name="amount">The amount of damage to take</param>
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// whenenver something collides with this trigger
    /// </summary>
    /// <param name="col">The collider that we hit</param>
    protected virtual void OnTriggerStay2D(Collider2D col)
    {

    }
}
