using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utilities
{
    /// <summary>
    /// Generates the planet type randomly.
    /// </summary>
    /// <param name="min">The minimum value to roll on this planet. Raise to increase chance of "good" planet</param>
    /// <param name="max">The maximum value to roll on this planet. Lower to increase chance of "bad" planet</param>
    /// <param name="radius">The radius of the planet</param>
    /// <returns>A list of planet types that defines the planet</returns>
    public static List<PlanetType> GetRandomPlanetType(float min, float max, out float radius)
    {
        #region Error checking parameters

        min = Mathf.Clamp(min, 0, 1);
        max = Mathf.Clamp(max, 0, 1);
        if (min > max)
        {
            float temp = min;
            min = max;
            max = temp;
        }

        #endregion
        float value = Random.Range(min, max);
        List<PlanetType> planet = new List<PlanetType>();
        radius = 0; //Prevents error

        #region Handling planet type and size
        //Handle tech level
        if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.PrimitiveTech]) { planet.Add(PlanetType.PrimitiveTech); }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.BasicTech]) { planet.Add(PlanetType.BasicTech); }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.AdvancedTech]) { planet.Add(PlanetType.AdvancedTech); }

        value = Random.Range(min, max);

        //Handle resource level
        if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.DryResources]) { planet.Add(PlanetType.DryResources); }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.NormalResources]) { planet.Add(PlanetType.NormalResources); }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.RichResources]) { planet.Add(PlanetType.RichResources); }

        value = Random.Range(min, max);

        //Handle hostility level
        if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.Hostile]) { planet.Add(PlanetType.Hostile); }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.Neutral]) { planet.Add(PlanetType.Neutral); }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.Friendly]) { planet.Add(PlanetType.Friendly); }

        value = Random.Range(min, max);

        //Handle size
        if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.Small])
        {
            planet.Add(PlanetType.Small);
            radius = Random.Range(Constants.PLANET_SIZE_BASE_VALUES[PlanetType.Small] - Constants.PLANET_SIZE_MAX_OFFSET[PlanetType.Small], Constants.PLANET_SIZE_BASE_VALUES[PlanetType.Small] + Constants.PLANET_SIZE_MAX_OFFSET[PlanetType.Small]);
        }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.Medium])
        {
            planet.Add(PlanetType.Medium);
            radius = Random.Range(Constants.PLANET_SIZE_BASE_VALUES[PlanetType.Medium] - Constants.PLANET_SIZE_MAX_OFFSET[PlanetType.Medium], Constants.PLANET_SIZE_BASE_VALUES[PlanetType.Medium] + Constants.PLANET_SIZE_MAX_OFFSET[PlanetType.Medium]);
        }
        else if (value <= Constants.PLANET_TYPE_WEIGHTS[PlanetType.Large])
        {
            planet.Add(PlanetType.Large);
            radius = Random.Range(Constants.PLANET_SIZE_BASE_VALUES[PlanetType.Large] - Constants.PLANET_SIZE_MAX_OFFSET[PlanetType.Large], Constants.PLANET_SIZE_BASE_VALUES[PlanetType.Large] + Constants.PLANET_SIZE_MAX_OFFSET[PlanetType.Large]);
        }
        #endregion

        return planet;
    }

    /// <summary>
    /// Generates the resources on the planet randomly
    /// </summary>
    /// <param name="min">The minimum value to roll on this planet. Raise to increase chance of "good" planet</param>
    /// <param name="max">The maximum value to roll on this planet. Lower to increase chance of "bad" planet</param>
    /// <returns>A list of resources the planet contains</returns>
    public static List<ResourceType> GetRandomResourceType(float min, float max)
    {
        #region Error checking parameters

        min = Mathf.Clamp(min, 0, 1);
        max = Mathf.Clamp(max, 0, 1);
        if (min > max)
        {
            float temp = min;
            min = max;
            max = temp;
        }

        #endregion
        float value = Random.Range(min, max);
        List<ResourceType> planet = new List<ResourceType>();

        #region Handling Resource Type
        foreach (KeyValuePair<ResourceType, float> weight in Constants.RESOURCE_TYPE_WEIGHTS)
        {
            value = Random.Range(min, max);
            if (value <= weight.Value) { planet.Add(weight.Key); }
        }
        #endregion

        return planet;
    }

    /// <summary>
    /// Generates the tech on the planet randomly
    /// </summary>
    /// <param name="min">The minimum value to roll on this planet. Raise to increase chance of "good" planet</param>
    /// <param name="max">The maximum value to roll on this planet. Lower to increase chance of "bad" planet</param>
    /// <returns>A list of tech the planet contains</returns>
    public static List<TechType> GetRandomTechType(float min, float max)
    {
        #region Error checking parameters

        min = Mathf.Clamp(min, 0, 1);
        max = Mathf.Clamp(max, 0, 1);
        if (min > max)
        {
            float temp = min;
            min = max;
            max = temp;
        }

        #endregion
        float value = Random.Range(min, max);
        List<TechType> planet = new List<TechType>();

        #region Handling Tech Type
        foreach (KeyValuePair<TechType, float> weight in Constants.TECH_TYPE_WEIGHTS)
        {
            value = Random.Range(min, max);
            if (value <= weight.Value) { planet.Add(weight.Key); }
        }
        #endregion

        return planet;
    }

    /// <summary>
    /// Gets a random vector3 that is appropriate as a rotation of the planet
    /// </summary>
    /// <returns>A vector 3 representing the rotation</returns>
    public static Vector3 GetRandomRotation()
    {
        return new Vector3(Random.Range(-Constants.PLANET_ROTATION_OFFSET, Constants.PLANET_ROTATION_OFFSET), Random.Range(-Constants.PLANET_ROTATION_OFFSET, Constants.PLANET_ROTATION_OFFSET), Random.Range(-Constants.PLANET_ROTATION_OFFSET, Constants.PLANET_ROTATION_OFFSET));
    }


    /// <summary>
    /// Gets the appropriate radius for the planet based on its scale
    /// </summary>
    /// <param name="scale">the scale of the planet</param>
    /// <returns>The appropriate radius</returns>
    public static float GetAppropriateRadiusForCollider(float scale)
    {
        //Under the rule that @1 scale radius = 1;, and @10 scale radius = .6... Roughly
        return .6f * Mathf.Pow(.8f, scale) + .6f;
    }


}



