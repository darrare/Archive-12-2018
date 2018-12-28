using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemyScript : EnemyScript
{

    [SerializeField]
    bool isDashingToEstimatedPosition = false;
    float chargeTimer = 0;
    Vector2 refVelocity;
    float personalDecelerationTime;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
        aggroDistance = Constants.DASHING_ENEMY_AGGRO_DISTANCE;
        personalDecelerationTime = Constants.DASHING_ENEMY_DECELERATION_TIME + Random.Range(-Constants.DASHING_ENEMY_DECELERATION_TIME_VARIANCE, Constants.DASHING_ENEMY_DECELERATION_TIME_VARIANCE);
        if (isDashingToEstimatedPosition)
        {
            behaviour = DashingProjectedLocationBehaviour;
        }
        else
        {
            behaviour = DashingBehaviour;
        }
        health = Constants.DASHING_ENEMY_HEALTH;
	}

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}

    /// <summary>
    /// Called whenever this enemy starts attacking the player
    /// </summary>
    protected override void OnAggro()
    {
        base.OnAggro();
        GameManager.Instance.Camera.AddTransformToTargets(transform, Constants.DASHING_ENEMY_CAMERA_WEIGHT);
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
        GameManager.Instance.Camera.ApplyCameraShake(Constants.DASHING_ENEMY_DEATH_CAMERA_SHAKE_INTENSITY, Constants.DASHING_ENEMY_DEATH_CAMERA_SHAKE_DURATION);
        Destroy(gameObject);
    }

    /// <summary>
    /// The enemy will dash towards wherever the player currently is
    /// </summary>
    void DashingBehaviour()
    {
        if (rBody.velocity.magnitude < Constants.DASHING_ENEMY_MAGNITUDE_TO_START_CHARGING)
        {
            chargeTimer += Time.deltaTime;
            rBody.velocity = Vector2.zero;
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
            if (chargeTimer >= Constants.DASHING_ENEMY_CHARGE_TIME)
            {
                rBody.velocity = (GameManager.Instance.Player.transform.position - transform.position).normalized * Constants.DASHING_ENEMY_INIT_SPEED;
                chargeTimer = 0;
            }
        }
        else
        {
            rBody.velocity = Vector2.SmoothDamp(rBody.velocity, Vector2.zero, ref refVelocity, personalDecelerationTime, Mathf.Infinity, Time.deltaTime);
        }
    }

    /// <summary>
    /// The enemy will dash to where it predicts the player will be in a few frames
    /// </summary>
    void DashingProjectedLocationBehaviour()
    {
        if (rBody.velocity.magnitude < Constants.DASHING_ENEMY_MAGNITUDE_TO_START_CHARGING)
        {
            chargeTimer += Time.deltaTime;
            rBody.velocity = Vector2.zero;
            transform.eulerAngles = new Vector3(0, 0, Utilities.EulerAnglesLookAt(transform.position, GameManager.Instance.Player.transform.position));
            if (chargeTimer >= Constants.DASHING_ENEMY_CHARGE_TIME)
            {
                rBody.velocity = ((GameManager.Instance.Player.transform.position + GameManager.Instance.Player.Velocity * Time.deltaTime * Constants.DASHING_ENEMY_FRAMES_TO_PREDICT_LOCATION) - transform.position).normalized * Constants.DASHING_ENEMY_INIT_SPEED;
                chargeTimer = 0;
            }
        }
        else
        {
            rBody.velocity = Vector2.SmoothDamp(rBody.velocity, Vector2.zero, ref refVelocity, personalDecelerationTime, Mathf.Infinity, Time.deltaTime);
        }
    }

    protected override void OnTriggerStay2D(Collider2D col)
    {
        base.OnTriggerStay2D(col);
        if (col.tag == "Player")
        {
            GameManager.Instance.Player.DamageTaken(Constants.DASHING_ENEMY_DAMAGE, transform.position);
        }
    }
}
