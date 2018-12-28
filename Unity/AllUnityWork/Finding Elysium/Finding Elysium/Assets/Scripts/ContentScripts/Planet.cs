using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Planet : PauseableObjectScript
{
    float scale;
    bool isEnabled = true;

    /// <summary>
    /// The type of the planet
    /// </summary>
    public List<PlanetType> PlanetTypes
    { get; private set; }

    /// <summary>
    /// The types of resources on this planet
    /// </summary>
    public List<ResourceType> ResourceTypes
    { get; private set; }

    /// <summary>
    /// The tech on this planet
    /// </summary>
    public List<TechType> TechTypes
    { get; private set; }

    /// <summary>
    /// The rotation of the planet per minute of game time
    /// </summary>
    public Vector3 Rotation
    { get; private set; }

    /// <summary>
    /// The scale of the planet
    /// </summary>
    public float Scale
    { get { return scale; } }

    /// <summary>
    /// The radius of the sphere collider
    /// </summary>
    public float Radius
    { get { return GetComponent<SphereCollider>().radius; } }

    /// <summary>
    /// Enables and disables if out of range of player
    /// </summary>
    public bool IsEnabled
    {
        get { return isEnabled; }
        set
        {
            isEnabled = value;
            transform.gameObject.SetActive(value);
        }
    }



    /// <summary>
    /// Initializes the planet with a random type/size, and random resources/tech
    /// </summary>
    void Awake()
    {

    }

    public override void Initialize()
    {
        base.Initialize();
        PlanetTypes = Utilities.GetRandomPlanetType(0, 1, out scale);
        ResourceTypes = Utilities.GetRandomResourceType(0, 1);
        TechTypes = Utilities.GetRandomTechType(0, 1);
        Rotation = Utilities.GetRandomRotation();
        transform.localScale = new Vector3(scale, scale, scale);
        transform.GetComponent<SphereCollider>().radius = Utilities.GetAppropriateRadiusForCollider(scale);
    }

    protected override void NotPausedUpdate()
    {
        transform.Rotate(Rotation);
    }

    void DrawPlanet()
    {

    }

}

public enum PlanetType
{
    PrimitiveTech, BasicTech, AdvancedTech,
    DryResources, NormalResources, RichResources,
    Hostile, Neutral, Friendly,
    Small, Medium, Large,
}

public enum ResourceType
{
    Oil, 
    Water,
    Food,
    Uranium,
}

public enum TechType
{
    Agriculture,
    Medical, 
    Cybernetics,
    Futuristic,
}