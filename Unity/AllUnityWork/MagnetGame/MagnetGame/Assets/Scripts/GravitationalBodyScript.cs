using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBodyScript : MonoBehaviour
{
    float scale;
    float effectRadius;
    float effectStrength;

    Vector3 rotateVal = new Vector3(-6f, 18f, 0);

	// Use this for initialization
	void Start ()
    {
        scale = transform.localScale.x;
        effectRadius = scale * 5;
        effectStrength = scale * 10;
        ObjectiveScript.Instance.GravitationalBodies.Add(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(rotateVal * Time.deltaTime);
	}

    /// <summary>
    /// Gets the value at which this gravitational body effects the objective.
    /// </summary>
    /// <param name="position">The position of the objective</param>
    /// <returns>The amount to modify the objectives velocity</returns>
    public Vector3 GetPullEffector(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        Vector3 direction = transform.position - position; //might be backwards, double check
        direction.Normalize();

        //If we are out of range, simply return zero
        if (distance > effectRadius)
        {
            return Vector3.zero;
        }

        float scalar = 1 - (distance / effectRadius);
        return direction * scalar * effectStrength;
    }
}
