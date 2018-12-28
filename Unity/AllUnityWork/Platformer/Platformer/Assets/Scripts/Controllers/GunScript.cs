using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GunScript : MonoBehaviour
{
    Transform bulletSpawnLocation;
    Vector2 direction;
    float delayBetweenShots = Constants.GUN_DELAY_BETWEEN_SHOTS;
    SpriteRenderer flashSpriteRenderer;
    float angle;

    GameObject bulletPrefab;

	// Use this for initialization
	void Start ()
    {
        direction = Vector2.right;
        bulletSpawnLocation = transform.GetChild(0);
        flashSpriteRenderer = bulletSpawnLocation.GetComponent<SpriteRenderer>();
        flashSpriteRenderer.enabled = false;

        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.Instance.Player.IsReactingToDamage)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.Normalize();
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
            if (Mathf.Abs(angle) < 90)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, -1, 1);
            }

            delayBetweenShots -= Time.deltaTime;
            if (Input.GetMouseButton(0) && delayBetweenShots <= 0)
            {
                ShootBullet();
                flashSpriteRenderer.enabled = true;
                delayBetweenShots = Constants.GUN_DELAY_BETWEEN_SHOTS;
            }
            else if (delayBetweenShots <= Constants.GUN_DELAY_BETWEEN_SHOTS / 2)
            {
                flashSpriteRenderer.enabled = false;
            }
        }
	}

    void ShootBullet()
    {
        //Instantiate the bullet
        EffectManager.Instance.AddNewEffect(EffectRecyclerType.Bullet, bulletSpawnLocation.position, transform.eulerAngles.z, Utilities.LookAtDirection(transform.eulerAngles.z).normalized, 0, .5f, .5f);
        //Instantiate the bullet casing
        EffectManager.Instance.AddNewEffect(EffectRecyclerType.BulletCasing, transform.position, transform.eulerAngles.z, Utilities.RotateDegrees(new Vector2(-direction.y, direction.x) * transform.localScale.y * Constants.CASING_INIT_VELOCITY_MULTIPLIER, Random.Range(-10f, 10f)), Random.Range(0f, Constants.CASING_MAX_ROTATIONAL_VELOCITY), Constants.CASING_TIME_UNTIL_DELETION, Constants.CASING_TIME_UNTIL_FADE);

        AudioManager.Instance.PlayOneShot(SoundEffect.GunShot);

        GameManager.Instance.Camera.ApplyCameraShake(Constants.GUN_CAMERA_SHAKE_INTENSITY);
    }
}
