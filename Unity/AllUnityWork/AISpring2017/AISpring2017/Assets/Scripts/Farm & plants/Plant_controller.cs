using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_controller : MonoBehaviour
{
    #region Variables

    public Sprite firePlantLv1;
    public Sprite firePlantLv2;
    public Sprite firePlantLv3;
    public Sprite icePlantLv1;
    public Sprite icePlantLv2;
    public Sprite icePlantLv3;
    public Sprite voidPlantLv1;
    public Sprite voidPlantLv2;
    public Sprite voidPlantLv3;
    public Sprite fireBullet;
    public Sprite iceBullet;
    public Sprite voidBullet;
    public Game_Manager.PlantType thisPlant;
    public GameObject bullet;
    public GameObject currentTarget;
    GameObject createdBullet;
    GameObject testEnemyExist;
    public GameObject thisTile;
    int shootTimer = 0;
    int growthTimer = 1800;
    int currentLevel = 1;
    float currentAlpha = 255;
    public bool isFertilized = false;
    bool canShoot = true;
    public float range = 1f;
    Vector3 startScale;
    SpriteRenderer spriteRender;

    #endregion

    #region Start

    // Use this for initialization
    void Start ()
    {
        switch (thisPlant)
        {
            case Game_Manager.PlantType.FIRE:
                GetComponent<SpriteRenderer>().sprite = firePlantLv3;
                break;
            case Game_Manager.PlantType.ICE:
                GetComponent<SpriteRenderer>().sprite = icePlantLv3;
                break;
            case Game_Manager.PlantType.VOID:
                GetComponent<SpriteRenderer>().sprite = voidPlantLv3;
                break;
            default:
                GetComponent<SpriteRenderer>().sprite = firePlantLv3;
                break;
        }

        spriteRender = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
    }

    #endregion

    #region Update

    // Update is called once per frame
    void Update ()
    {
        #region Scale Sprite

        switch (currentLevel)
        {
            case 1:
                transform.localScale = startScale * ((2250f - (float)growthTimer) / 1800f);

                if (transform.localScale.x > .5f)
                {
                    transform.localScale = new Vector3(.5f, .5f, .5f);
                }
                break;
            case 2:
                transform.localScale = startScale * ((5400f - (float)growthTimer) / 3600f);

                if (transform.localScale.x > .75f)
                {
                    transform.localScale = new Vector3(.75f, .75f, .75f);
                }
                break;
            case 3:
                transform.localScale = startScale;
                break;
        }

        #endregion

        if (!Game_Manager.Instance.pauseGame && Game_Manager.Instance.gameStarted)
        {
            currentTarget = FindClosestEnemy();
            testEnemyExist = GameObject.FindGameObjectWithTag("Enemy");
            shootTimer--;

            if ((currentLevel < Game_Manager.maxPlantLevel) && (thisTile.GetComponent<Farm_Controller>().waterLevel > 0) && (Game_Manager.Instance.currentPhase == Game_Manager.Phase.DAY))
            {
                if (isFertilized)
                {
                    growthTimer -= (1 * (currentLevel)) * 2;
                }
                else
                {
                    growthTimer -= 1 * (currentLevel);
                }
            }

            #region Flash Transparency

            if (currentLevel == 3)
            {
                currentAlpha = 255f;
                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, currentAlpha / 255f);
            }
            else if (((currentAlpha >= 255) || (thisTile.GetComponent<Farm_Controller>().waterLevel <= 0)) && (Game_Manager.Instance.currentPhase == Game_Manager.Phase.NIGHT))
            {
                currentAlpha = 125f;
                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, currentAlpha / 255f);
            }
            else
            {
                if (currentAlpha < 255)
                {
                    currentAlpha++;
                }

                spriteRender.color = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, currentAlpha / 255f);
            }

            #endregion

            #region LevelUp Plant

            if (growthTimer <= 0)
            {
                if (currentLevel == 1)
                {
                    currentLevel++;
                    range++;
                    growthTimer = 1800;
                }
                else
                {
                    currentLevel++;
                    range++;
                    growthTimer = 3600;

                    switch (thisPlant)
                    {
                        case Game_Manager.PlantType.FIRE:
                            Game_Manager.Instance.firePlantsGrown++;
                            break;
                        case Game_Manager.PlantType.ICE:
                            Game_Manager.Instance.icePlantsGrown++;
                            break;
                        case Game_Manager.PlantType.VOID:
                            Game_Manager.Instance.voidPlantsGrown++;
                            break;
                    }
                }
            }

            #endregion

            if (shootTimer == 0)
            {
                canShoot = true;
            }

            #region Shoot

            if (canShoot && (testEnemyExist != null) && (currentTarget != null))
            {
                if (thisTile.GetComponent<Farm_Controller>().waterLevel > 0)
                {
                    if (Vector2.Distance(currentTarget.transform.position, this.gameObject.transform.position) <= range)
                    {
                        createdBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
                        createdBullet.GetComponent<Bullet>().move = true;
                        createdBullet.GetComponent<Bullet>().target = currentTarget;
                        createdBullet.GetComponent<Bullet>().damage = createdBullet.GetComponent<Bullet>().damage * (currentLevel + 1);
                        createdBullet.GetComponent<Bullet>().type = thisPlant;
                        canShoot = false;
                        shootTimer = 60 - ((currentLevel - 1) * 20);

                        if (currentLevel < 3)
                        {
                            if (Game_Manager.Instance.purchasedWaterEfficiency)
                            {
                                thisTile.GetComponent<Farm_Controller>().waterLevel -= .5f;
                            }
                            else
                            {
                                thisTile.GetComponent<Farm_Controller>().waterLevel -= 1;
                            }
                        }

                        if (thisPlant == Game_Manager.PlantType.VOID)
                        {
                            shootTimer = shootTimer / 2;
                        }

                        switch (thisPlant)
                        {
                            case Game_Manager.PlantType.FIRE:
                                createdBullet.GetComponent<Bullet>().thisSprite = fireBullet;
                                break;
                            case Game_Manager.PlantType.ICE:
                                createdBullet.GetComponent<Bullet>().thisSprite = iceBullet;
                                break;
                            case Game_Manager.PlantType.VOID:
                                createdBullet.GetComponent<Bullet>().thisSprite = voidBullet;
                                break;
                            default:
                                createdBullet.GetComponent<Bullet>().thisSprite = fireBullet;
                                break;
                        }
                    }
                }
            }

            #endregion
        }
    }

    #endregion

    #region Custom Methods

    public GameObject FindClosestEnemy()
    {
        GameObject[] allEnemies;
        allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in allEnemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }

    #endregion
}
