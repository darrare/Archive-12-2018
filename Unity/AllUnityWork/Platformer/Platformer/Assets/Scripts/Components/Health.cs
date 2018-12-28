using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    float health;

    [SerializeField]
    UnityEvent onDeath;

    /// <summary>
    /// Accessor for the health value
    /// </summary>
    public float CurHealth
    {
        get { return health; }
        set { health = value; }
    }
        

    /// <summary>
    /// Called by other scripts whenever the object this is attached to takes damage. Calls the UnityEvent when object is "dead"
    /// </summary>
    /// <param name="amount">The amount of damage to take</param>
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            onDeath.Invoke();
        }
    }
}
