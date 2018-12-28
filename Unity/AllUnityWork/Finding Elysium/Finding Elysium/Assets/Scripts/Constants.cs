using System.Collections;
using System.Collections.Generic;

public static class Constants
{
    public const float DISTANCE_TO_DESPAWN_PLANETS = 30f;

    #region file location headers
    public const string RUNTIME_PREFABS_LOCATION = "RuntimePrefabs/";
    #endregion

    #region ShipController

    public const float SHIP_SPEED = 5f;

    #endregion

    #region planet spawn stuff

    public const float PLANET_ROTATION_OFFSET = .1f;
    public const float PLANET_SPAWN_DISTANCE_MAX = 300f;
    public const int PLANETS_TO_SPAWN = 1000;
    public const float PLANET_MIN_DISTANCE_FROM_OTHER_PLANET = 3f;

    #endregion

    #region Planet Weights and size

    /// <summary>
    /// The weight of each type of planet type.
    /// </summary>
    public static readonly Dictionary<PlanetType, float> PLANET_TYPE_WEIGHTS = new Dictionary<PlanetType, float>
    {
        { PlanetType.PrimitiveTech, .2f }, { PlanetType.BasicTech, .6f }, { PlanetType.AdvancedTech, .8f },
        { PlanetType.DryResources, .2f }, { PlanetType.NormalResources, .6f }, { PlanetType.RichResources, .8f },
        { PlanetType.Hostile, .2f }, { PlanetType.Neutral, .6f }, { PlanetType.Friendly, .8f },
        { PlanetType.Small, .25f }, { PlanetType.Medium, .75f }, { PlanetType.Large, 1f },
    };

    /// <summary>
    /// The weight of each resource type that can be on a planet
    /// </summary>
    public static readonly Dictionary<ResourceType, float> RESOURCE_TYPE_WEIGHTS = new Dictionary<ResourceType, float>
    {
        { ResourceType.Food, .6f },
        { ResourceType.Oil, .2f },
        { ResourceType.Water, .5f },
        { ResourceType.Uranium, .1f },
    };

    /// <summary>
    /// The weight of each resource type that can be on a planet
    /// </summary>
    public static readonly Dictionary<TechType, float> TECH_TYPE_WEIGHTS = new Dictionary<TechType, float>
    {
        { TechType.Agriculture, .6f },
        { TechType.Cybernetics, .2f },
        { TechType.Futuristic, .2f },
        { TechType.Medical, .6f },
    };

    /// <summary>
    /// The base size for the planet type
    /// </summary>
    public static readonly Dictionary<PlanetType, float> PLANET_SIZE_BASE_VALUES = new Dictionary<PlanetType, float>
    {
        { PlanetType.Small, 1 },
        { PlanetType.Medium, 4 },
        { PlanetType.Large, 10 },
    };

    /// <summary>
    /// The base size for the planet type
    /// </summary>
    public static readonly Dictionary<PlanetType, float> PLANET_SIZE_MAX_OFFSET = new Dictionary<PlanetType, float>
    {
        { PlanetType.Small, .5f },
        { PlanetType.Medium, 1 },
        { PlanetType.Large, 2 },
    };



    #endregion

}
