using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBossCannonScript : MonoBehaviour
{
    [SerializeField]
    bool isLeftCannon;

    float timer = 0;
    float durationTimer = 0;

	/// <summary>
    /// Initializes the cannon
    /// </summary>
	void Start ()
    {
		if (isLeftCannon)
        {
            GetComponentInParent<ShipBossScript>().LeftCannon = this;
        }
        else
        {
            GetComponentInParent<ShipBossScript>().RightCannon = this;
        }
	}

    /// <summary>
    /// Updates the cannon
    /// </summary>
    /// <param name="angleTarget">The angle we want to reach</param>
    /// <param name="isShooting">Should the cannon be shooting</param>
    /// <param name="shootDelay">The delay between shots, if shooting</param>
    public bool UpdateCannon(float angleTarget, bool isShooting, float shootDelay = 0)
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, angleTarget, Constants.SHIP_BOSS_CANNON_ROTATION_SPEED * Time.deltaTime));

        //set this timer here because it only matters in the other update
        durationTimer = 0;

        if (isShooting)
        {
            timer += Time.deltaTime;
            if (timer >= shootDelay)
            {
                timer = 0;
                ShootCannon();
            }
        }

        if (Mathf.Abs(transform.eulerAngles.z - angleTarget) < 3)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates the cannon by having it spin around
    /// </summary>
    /// <param name="duration">The duration to spin</param>
    /// <param name="shootDelay">The delay between shots</param>
    /// <returns>When it has finished, it returns true</returns>
    public bool UpdateCannonSpin(float duration, float shootDelay)
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(transform.eulerAngles.z, isLeftCannon ? transform.eulerAngles.z - 90 : transform.eulerAngles.z + 90, Constants.SHIP_BOSS_CANNON_ROTATION_SPEED * 2 * Time.deltaTime));

        timer += Time.deltaTime;
        durationTimer += Time.deltaTime;
        if (timer >= shootDelay)
        {
            timer = 0;
            ShootCannon();
        }

        if (durationTimer >= duration)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// shoots the cannon
    /// </summary>
    void ShootCannon()
    {
        AudioManager.Instance.PlayOneShot(SoundEffect.BossShoot, .5f);
        EffectManager.Instance.AddNewEffect(EffectRecyclerType.CannonProjectile, transform.GetChild(0).position, transform.eulerAngles.z, Utilities.LookAtDirection(transform.eulerAngles.z).normalized, 0, 5);
    }
}
