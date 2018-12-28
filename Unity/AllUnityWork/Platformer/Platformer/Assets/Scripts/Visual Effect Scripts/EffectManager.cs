using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


/// <summary>
/// An enumeration used for referencing stuff that is for all of the effect recycling behaviours
/// </summary>
public enum EffectRecyclerType
{
    BulletCasing,
    Shrapnel,
    Bullet,
    BloodSplatter,
    ExplosionFire,
    MiniNuke,
    BigNuke,
    CannonProjectile,
    TendrilMain,
    TendrilPiece,
    IndicatorBeam,
    EyeBossBeam,
    EyeBossProjectile,
    EyeBossMiniEye,
}

public class EffectManager
{
    #region fields

    static EffectManager instance;
    GameObject temp;

    #endregion

    #region singleton and constructor

    /// <summary>
    /// Singleton for the effect manager
    /// </summary>
    public static EffectManager Instance
    { get { return instance ?? (instance = new EffectManager()); } }

    /// <summary>
    /// Constructor for the effect manager 
    /// </summary>
    private EffectManager()
    {
        ActiveEffects = new Dictionary<EffectRecyclerType, List<RecycledEffectScript>>();
        InactiveEffects = new Dictionary<EffectRecyclerType, List<RecycledEffectScript>>();
        EffectPrefabs = new Dictionary<EffectRecyclerType, GameObject>();

        foreach (EffectRecyclerType t in Enum.GetValues(typeof(EffectRecyclerType)))
        {
            ActiveEffects.Add(t, new List<RecycledEffectScript>());
            InactiveEffects.Add(t, new List<RecycledEffectScript>());
            EffectPrefabs.Add(t, Resources.Load<GameObject>("Prefabs/" + t.ToString()));
        }

        RecyclerParent = new GameObject("Recycleables").transform;
        Object.DontDestroyOnLoad(RecyclerParent.gameObject);
    }

    #endregion

    #region properties

    /// <summary>
    /// The gameObject that is used to store all of the objects that need to be
    /// recycled so that our hierarchy isn't a mess when we run the game.
    /// </summary>
    public Transform RecyclerParent
    { get; private set; }

    /// <summary>
    /// Store the prefabs of each of the objects
    /// </summary>
    public Dictionary<EffectRecyclerType, GameObject> EffectPrefabs
    { get; set; }

    /// <summary>
    /// Current active effects in the scene
    /// </summary>
    Dictionary<EffectRecyclerType, List<RecycledEffectScript>> ActiveEffects
    { get; set; }

    /// <summary>
    /// Effects that are currently inactive in the scene
    /// </summary>
    Dictionary<EffectRecyclerType, List<RecycledEffectScript>> InactiveEffects
    { get; set; }

    #endregion

    #region public methods

    /// <summary>
    /// Updates the Effect manager
    /// </summary>
    public void Update ()
    {

	}

    /// <summary>
    /// Adds a new effect to the scene
    /// </summary>
    /// <param name="type">The type of effect this is (used for calling methods in EffectManager)</param>
    /// <param name="startPosition">The starting position of the object</param>
    /// <param name="startRotation">The starting rotation of the object</param>
    /// <param name="velocity">The initial velocity of the effect (private update methods will handle any weird changes)</param>
    /// <param name="rotationSpeed">A rotation speed if one is required. 0 otherwise</param>
    /// <param name="timeUntilDeletion">The time that this object should be alive. After this point, this object is hidden and added to the InactiveEffects list in EffectManager</param>
    /// <param name="timeUntilFadeStart">The time until this object starts to fade out, if this object has a fade to it.</param>
    /// <returns>Returns the added script just incase we needed it</returns>
    public RecycledEffectScript AddNewEffect(EffectRecyclerType type, Vector3 startPosition, float startRotation, Vector3 velocity, float rotationSpeed, float timeUntilDeletion, float timeUntilFadeStart = -1f)
    {
        if (InactiveEffects[type].Count == 0)
        {
            ActiveEffects[type].Add(Object.Instantiate(EffectPrefabs[type], RecyclerParent, false).GetComponent<RecycledEffectScript>().Initialize(type, startPosition, startRotation, velocity, rotationSpeed, timeUntilDeletion, timeUntilFadeStart));
        }
        else
        {
            InactiveEffects[type][0].gameObject.SetActive(true);
            ActiveEffects[type].Add(InactiveEffects[type][0].Initialize(type, startPosition, startRotation, velocity, rotationSpeed, timeUntilDeletion, timeUntilFadeStart));
            InactiveEffects[type].RemoveAt(0);
        }
        return ActiveEffects[type][ActiveEffects[type].Count - 1];
    }

    /// <summary>
    /// An effect has finished, so add it to the inactive list and remove it from the active list
    /// </summary>
    /// <param name="type">Type of script it is</param>
    /// <param name="script">The script itself</param>
    public void EffectFinished(EffectRecyclerType type, RecycledEffectScript script)
    {
        InactiveEffects[type].Add(script);
        ActiveEffects[type].Remove(script);
        InactiveEffects[type][InactiveEffects[type].Count - 1].gameObject.SetActive(false);
    }

    /// <summary>
    /// Creates an explosion
    /// </summary>
    /// <param name="position">The center point of the explosion</param>
    /// <param name="radius">The radius of the explosion. anything > 1 will spawn multiple fire objects, and anything less than 1 will reduce the scale and only spawn one</param>
    /// <param name="duration">Duration it should exist</param>
    /// <param name="fadeDuration">Time at which it should start fading out (hurtbox goes away when fading)</param>
    /// <param name="damageValue">Amount of damage to deal</param>
    public void CreateExplosion(Vector2 position, float radius, float duration, float fadeDuration, float damageValue = Constants.EXPLOSION_FIRE_DAMAGE)
    {
        for (int i = 0; i < radius * 5; i++)
        {
            ((ExplosionFireScript)(AddNewEffect(EffectRecyclerType.ExplosionFire, position + Random.insideUnitCircle * radius, 0, Vector3.zero, 0, duration, fadeDuration))).SecondInitialize(damageValue, radius > 1 ? 1 : radius);
        }
    }


    #endregion

    #region private methods

    #endregion
}
