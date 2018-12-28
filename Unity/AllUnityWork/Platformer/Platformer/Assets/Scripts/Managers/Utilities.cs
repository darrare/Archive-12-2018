using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utilities
{
    /// <summary>
    /// rotates a vector by an amount of radians
    /// </summary>
    /// <param name="vec">The original vector we want to rotate</param>
    /// <param name="radians">The amount of radians we want to rotate by</param>
    /// <returns>The rotated vector</returns>
    public static Vector2 RotateRadians(Vector2 vec, float radians)
    {
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(cos * vec.x - sin * vec.y, sin * vec.x + cos * vec.y);
    }

    /// <summary>
    /// Roates a vector by an amount of degrees
    /// </summary>
    /// <param name="vec">The vector to rotate</param>
    /// <param name="degrees">the amount of degrees to rotate by</param>
    /// <returns>the rotated vector</returns>
    public static Vector2 RotateDegrees(Vector2 vec, float degrees)
    {
        return RotateRadians(vec, Mathf.Deg2Rad * degrees);
    }

    /// <summary>
    /// Takes two positions and finds the right Z coordinate euler angle to make the object face the other object
    /// </summary>
    /// <param name="position"></param>
    /// <param name="targetPosition"></param>
    /// <returns>The angle of the direction</returns>
    public static float EulerAnglesLookAt(Vector2 position, Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - position).normalized;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Gets the Vector2 direction this object is facing from the rotation angle
    /// </summary>
    /// <param name="eulerAnglesZ">The rotation angle</param>
    /// <returns>The directional vector</returns>
    public static Vector2 LookAtDirection(float eulerAnglesZ)
    {
        return new Vector2(Mathf.Cos(eulerAnglesZ * Mathf.Deg2Rad), Mathf.Sin(eulerAnglesZ * Mathf.Deg2Rad));
    }

    /// <summary>
    /// Gets a random position within the box collider 2D relative to the box itself (width = 2 box, left side would return vector2.x = -1)
    /// </summary>
    /// <param name="t">The transform with an attached box collider</param>
    /// <returns>The random position, relative to the box (width = 2 box, left side would return vector2.x = -1)</returns>
    public static Vector2 GetRandomPositionInBoxCollider2D(Transform t)
    {
        float x = Random.Range(-t.GetComponent<BoxCollider2D>().bounds.extents.x, t.GetComponent<BoxCollider2D>().bounds.extents.x) / t.localScale.x;
        float y = Random.Range(-t.GetComponent<BoxCollider2D>().bounds.extents.y, t.GetComponent<BoxCollider2D>().bounds.extents.y) / t.localScale.y;
        return new Vector2(x, y);
    }

    /// <summary>
    /// Finds the closest point to P on a line segment from A to B
    /// </summary>
    /// <param name="A">First point of a line</param>
    /// <param name="B">Second point of a line</param>
    /// <param name="P">Point we have and want to find closest point on line to</param>
    /// <returns>Closest point on the line</returns>
    public static Vector2 GetClosestPointOnLineSegment(Vector2 A, Vector2 B, Vector2 P)
    {
        Vector2 AP = P - A;       //Vector from A to P   
        Vector2 AB = B - A;       //Vector from A to B  

        float magnitudeAB = AB.sqrMagnitude;     //Magnitude of AB vector (it's length squared)     
        float ABAPproduct = Vector2.Dot(AP, AB);    //The DOT product of a_to_p and a_to_b     
        float distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

        if (distance < 0)     //Check if P projection is over vectorAB     
            return A;
        else if (distance > 1)
            return B;
        else
            return A + AB * distance;
    }
}
