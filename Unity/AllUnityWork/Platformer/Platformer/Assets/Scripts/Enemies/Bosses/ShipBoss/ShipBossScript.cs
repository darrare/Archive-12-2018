using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBossScript : EnemyScript
{
    enum ShipBossState { None, Intermission, ArcingCannonsPrep, ArcingCannonsFire, MiniNukesPrep, MiniNukesRelease, MiniNukesReturnToCenter, BigNuke }

    Dictionary<ShipBossState, Behaviour> behaviours;

    float stateTimer = 0;
    int prevIndex = 0;
    int index = 0;

    [SerializeField]
    Vector2 leftPos, centerPos, rightPos;

    /// <summary>
    /// prefab for the cannon projectiles
    /// </summary>
    public GameObject CannonProjectilePrefab
    { get; private set; }

    /// <summary>
    /// The ships left cannon
    /// </summary>
    public ShipBossCannonScript LeftCannon
    { get; set; }

    /// <summary>
    /// the ships left cannon
    /// </summary>
    public ShipBossCannonScript RightCannon
    { get; set; }


    /// <summary>
    /// Initializes the ship
    /// </summary>
    protected override void Start()
    {
        base.Start();
        behaviours = new Dictionary<ShipBossState, Behaviour>()
        {
            { ShipBossState.None, None },
            { ShipBossState.ArcingCannonsPrep, ArcingCannonsPrep },
            { ShipBossState.ArcingCannonsFire, ArcingCannonsFire },
            { ShipBossState.MiniNukesPrep, MiniNukesPrep },
            { ShipBossState.MiniNukesRelease, MiniNukesRelease },
            { ShipBossState.MiniNukesReturnToCenter, MiniNukesReturnToCenter },
            { ShipBossState.BigNuke, BigNuke },
            { ShipBossState.Intermission, Intermission },
        };
        aggroDistance = Constants.SHIP_BOSS_AGGRO_DISTANCE;
        behaviour = behaviours[ShipBossState.None];
        health = Constants.SHIP_BOSS_HEALTH;

        CannonProjectilePrefab = Resources.Load<GameObject>("Prefabs/CannonProjectile");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Called whenever this enemy starts attacking the player
    /// </summary>
    protected override void OnAggro()
    {
        base.OnAggro();
        GameManager.Instance.Camera.AddTransformToTargets(transform, Constants.SHIP_BOSS_CAMERA_WEIGHT);
    }

    /// <summary>
    /// Called whenever this enemy dies
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();
        for (int i = 0; i < 75; i++)
        {
            EffectManager.Instance.AddNewEffect(EffectRecyclerType.BloodSplatter, transform.position, 0, Utilities.RotateDegrees(Vector2.right, Random.Range(0, 360)) * Random.Range(Constants.BLOOD_SPLATTER_INIT_SPEED / 2, Constants.BLOOD_SPLATTER_INIT_SPEED), 0, 0, 0);
        }
        GameManager.Instance.Camera.ApplyCameraShake(Constants.SHIP_BOSS_DEATH_CAMERA_SHAKE_INTENSITY, Constants.SHIP_BOSS_DEATH_CAMERA_SHAKE_DURATION);
        Destroy(gameObject);
    }

    public void WhenThisBossDies()
    {
        OnDeath();
    }

    #region Behaviours

    /// <summary>
    /// The boss is literally idle
    /// </summary>
    void None()
    {
        if (Vector2.Distance(GameManager.Instance.Player.transform.position, transform.position) < aggroDistance)
        {
            OnAggro();
            behaviour = behaviours[ShipBossState.Intermission];
        }
    }

    /// <summary>
    /// The boss is moving around relative to the player and shooting cannons at them
    /// </summary>
    void Intermission()
    {
        stateTimer += Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, Constants.SHIP_BOSS_SPEED * Time.deltaTime / 4);
        LeftCannon.UpdateCannon(Utilities.EulerAnglesLookAt(LeftCannon.transform.position, GameManager.Instance.Player.transform.position), true, Constants.SHIP_BOSS_CANNON_FREE_SHOOT_DELAY);
        RightCannon.UpdateCannon(Utilities.EulerAnglesLookAt(RightCannon.transform.position, GameManager.Instance.Player.transform.position), true, Constants.SHIP_BOSS_CANNON_FREE_SHOOT_DELAY);

        if (stateTimer >= Constants.SHIP_BOSS_TIME_BETWEEN_STATES)
        {
            stateTimer = 0;
            behaviour = PickRandomAvailableBehaviour();
        }
    }

    /// <summary>
    /// The arcing cannons are preparing to fire.
    /// The boss will move to the center of the screen and aim both cannons directly downwards
    /// </summary>
    void ArcingCannonsPrep()
    {
        transform.position = Vector2.MoveTowards(transform.position, centerPos, Constants.SHIP_BOSS_SPEED * Time.deltaTime);

        if (Vector2.Distance(transform.position, centerPos) < Constants.SHIP_BOSS_MIN_DISTANCE_TO_SWITCH_STAGE
            & LeftCannon.UpdateCannon(270, false)
            & RightCannon.UpdateCannon(270, false))
        {
            behaviour = behaviours[ShipBossState.ArcingCannonsFire];
        }
    }

    /// <summary>
    /// The arcing cannons are rotating upwards, firing as they go
    /// </summary>
    void ArcingCannonsFire()
    {
        if (LeftCannon.UpdateCannonSpin(Constants.SHIP_BOSS_CANNON_SPIN_DURATION, Constants.SHIP_BOSS_CANNON_SPIN_SHOOT_DELAY)
            & RightCannon.UpdateCannonSpin(Constants.SHIP_BOSS_CANNON_SPIN_DURATION, Constants.SHIP_BOSS_CANNON_SPIN_SHOOT_DELAY))
        {
            behaviour = behaviours[ShipBossState.Intermission];
        }
    }

    /// <summary>
    /// The mini nukes phase is preparing.
    /// The ship will fly to the left side of the screen
    /// </summary>
    void MiniNukesPrep()
    {
        transform.position = Vector2.MoveTowards(transform.position, leftPos, Constants.SHIP_BOSS_SPEED * Time.deltaTime);
        LeftCannon.UpdateCannon(270, false);
        RightCannon.UpdateCannon(270, false);

        if (Vector2.Distance(transform.position, leftPos) < Constants.SHIP_BOSS_MIN_DISTANCE_TO_SWITCH_STAGE)
        {
            behaviour = behaviours[ShipBossState.MiniNukesRelease];
        }
    }

    /// <summary>
    /// The boss will fly accross the screen dropping mini nukes
    /// </summary>
    void MiniNukesRelease()
    {
        transform.position = Vector2.MoveTowards(transform.position,rightPos, Constants.SHIP_BOSS_SPEED * Time.deltaTime);

        stateTimer += Time.deltaTime;

        if (stateTimer >= Constants.SHIP_BOSS_MINI_NUKE_DROP_DELAY)
        {
            stateTimer = 0;
            EffectManager.Instance.AddNewEffect(EffectRecyclerType.MiniNuke, transform.position, 0, Vector3.zero, 0, 0);
        }

        if (Vector2.Distance(transform.position, rightPos) < Constants.SHIP_BOSS_MIN_DISTANCE_TO_SWITCH_STAGE)
        {
            behaviour = behaviours[ShipBossState.MiniNukesReturnToCenter];
        }
    }

    /// <summary>
    /// The ship will return to the center of the screen and wait until all explosions have ceased
    /// </summary>
    void MiniNukesReturnToCenter()
    {
        transform.position = Vector2.MoveTowards(transform.position, centerPos, Constants.SHIP_BOSS_SPEED * Time.deltaTime);

        if (Vector2.Distance(transform.position, centerPos) < Constants.SHIP_BOSS_MIN_DISTANCE_TO_SWITCH_STAGE) //&& all explosions have ceased)
        {
            behaviour = behaviours[ShipBossState.Intermission];
        }
    }

    /// <summary>
    /// The ship will drop the big nuke straight down from the center of the area and wait until all explosions have ceased
    /// </summary>
    void BigNuke()
    {
        transform.position = Vector2.MoveTowards(transform.position, centerPos, Constants.SHIP_BOSS_SPEED * Time.deltaTime);
        LeftCannon.UpdateCannon(270, false);
        RightCannon.UpdateCannon(270, false);

        if (Vector2.Distance(transform.position, centerPos) < Constants.SHIP_BOSS_MIN_DISTANCE_TO_SWITCH_STAGE)
        {
            EffectManager.Instance.AddNewEffect(EffectRecyclerType.BigNuke, transform.position, 0, Vector3.zero, 0, 0);
            behaviour = behaviours[ShipBossState.Intermission];
        }
    }

    #endregion

    /// <summary>
    /// Picks a random init stage
    /// TODO: we might want to make it where it can't pick the same thing twice in a row
    /// </summary>
    Behaviour PickRandomAvailableBehaviour()
    {
        while (index == prevIndex)
        {
            index = Random.Range(0, 3);
        }
        prevIndex = index;

        switch(index)
        {
            case 0:
                return behaviours[ShipBossState.ArcingCannonsPrep];
            case 1:
                return behaviours[ShipBossState.MiniNukesPrep];
            case 2:
                return behaviours[ShipBossState.BigNuke];
            default:
                return behaviours[ShipBossState.Intermission];
        }
    }

    /// <summary>
    /// Something has collided with the boss
    /// </summary>
    /// <param name="col">The collider of the object that collided</param>
    protected override void OnTriggerStay2D(Collider2D col)
    {
        base.OnTriggerStay2D(col);
        if (col.tag == "Player")
        {
            GameManager.Instance.Player.DamageTaken(Constants.DASHING_ENEMY_DAMAGE, transform.position);
        }
    }
}
