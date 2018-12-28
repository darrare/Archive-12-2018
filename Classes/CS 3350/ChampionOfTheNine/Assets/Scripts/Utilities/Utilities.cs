using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class for various utility functions
/// </summary>
public static class Utilities
{
    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    public static Vector2 MousePosition
    { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }

    /// <summary>
    /// Gets the angle between two points
    /// </summary>
    /// <param name="start">the start point</param>
    /// <param name="end">the end point</param>
    /// <returns>the angle, in degrees</returns>
    public static float GetAngleDegrees(Vector2 start, Vector2 end)
    {
        return GetAngleDegrees(Mathf.Asin((end.y - start.y) / Vector2.Distance(end, start)) * Mathf.Rad2Deg, end.x - start.x);
    }

    /// <summary>
    /// Calculates the angle at which an object should be launched to reach the target position
    /// </summary>
    /// <param name="launchPosition">the launch position</param>
    /// <param name="targetPosition">the target position</param>
    /// <param name="objectSpeed">the speed at which the object will be launched</param>
    /// <param name="gravityScale">value by which to scale gravity (defaults to 1)</param>
    /// <returns>the angle</returns>
    public static float CalculateLaunchAngle(Vector2 launchPosition, Vector2 targetPosition, float objectSpeed, float gravityScale = 1)
    {
        Vector2 disp = launchPosition - targetPosition;

        // Calculates equation components
        float g = Physics2D.gravity.y * gravityScale;
        float speedSquared = Mathf.Pow(objectSpeed, 2);
        float topSqrt = Mathf.Sqrt(Mathf.Pow(speedSquared, 2) - (g * ((g * Mathf.Pow(disp.x, 2)) + (2 * disp.y * speedSquared))));
        float bottom = g * disp.x;

        // Calculates angles
        float angle1 = Mathf.Atan((speedSquared + topSqrt) / bottom) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan((speedSquared - topSqrt) / bottom) * Mathf.Rad2Deg;

        // Picks and returns better angle
        if (disp.x > 0)
        { return Mathf.Max(angle1, angle2) + 180; }
        else
        { return Mathf.Min(angle1, angle2); }
    }

    /// <summary>
    /// Gets an angle flipped appropriately
    /// </summary>
    /// <param name="angle">the angle</param>
    /// <param name="flipTest">what to test for flipping</param>
    /// <returns>the adjusted angle</returns>
    public static float GetAngleDegrees(float angle, float flipTest)
    {
        if (flipTest < 0)
        { angle = 180 - angle; }
        while (angle < 0)
        { angle += 360; }
        return angle;
    }
}
