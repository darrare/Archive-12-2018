using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    #region Variables

    public Sprite faceUp;
    public Sprite faceDown;
    public Sprite faceLeft;
    public Sprite faceRight;
    public float speed = 1f;
    const float constSpeed = 1f;
    float slowDownPercent = 1f;
    public GameObject[] fullPath;
    public int pathCount = 0;
    public int health = 70;
    public int moneyGivenOnDeath = 5;
    int DOT_effectTimer = 0;
    int slowEffectTimer = 0;
    int DOT = 0;
    int DOTMultiplier = 0;
    int DOTMultiplierMax = 4;
    int SlowMultiplier = 0;
    int SlowMultiplierMax= 2;
    bool startDOTEffect = false;

    #endregion

    #region Start

    // Use this for initialization
    void Start ()
    {

    }

    #endregion

    #region Update

    // Update is called once per frame
    void Update ()
    {
        if (!Game_Manager.Instance.pauseGame)
        {
            #region DOT Effect

            if (DOT_effectTimer > 0 && startDOTEffect)
            {
                DOT_effectTimer--;

                if (DOT_effectTimer % 6 == 0 && DOT > 0)
                {
                    health -= DOT * DOTMultiplier;
                }

                if (DOT_effectTimer <= 0)
                {
                    startDOTEffect = false;
                    DOT = 0;
                    DOTMultiplier = 0;
                }
            }

            #endregion

            #region Slow Effect

            if (slowEffectTimer > 0)
            {
                slowEffectTimer--;

                if (slowDownPercent > 0)
                {
                    speed = constSpeed - (constSpeed * (slowDownPercent * SlowMultiplier));
                }

                if (slowEffectTimer <= 0)
                {
                    slowDownPercent = 0;
                    SlowMultiplier = 0;
                }
            }

            #endregion

            #region Path

            if (pathCount <= fullPath.Length)
            {
                if (Vector2.Distance(this.gameObject.transform.position, fullPath[pathCount].gameObject.transform.position) < .07f)
                {
                    this.transform.position = fullPath[pathCount].transform.position;
                    pathCount++;
                }

                this.transform.position += Vector3.Normalize(fullPath[pathCount].transform.position - transform.position) * speed * Time.deltaTime;

                if (Vector3.Normalize(fullPath[pathCount].transform.position - transform.position) == Vector3.up)
                {
                    GetComponent<SpriteRenderer>().sprite = faceUp;
                }
                else if (Vector3.Normalize(fullPath[pathCount].transform.position - transform.position) == Vector3.down)
                {
                    GetComponent<SpriteRenderer>().sprite = faceDown;
                }
                else if (Vector3.Normalize(fullPath[pathCount].transform.position - transform.position) == Vector3.left)
                {
                    GetComponent<SpriteRenderer>().sprite = faceLeft;
                }
                else if (Vector3.Normalize(fullPath[pathCount].transform.position - transform.position) == Vector3.right)
                {
                    GetComponent<SpriteRenderer>().sprite = faceRight;
                }
            }

            #endregion

            #region Check Death

            if (health <= 0)
            {
                Game_Manager.Instance.money += moneyGivenOnDeath;
                Destroy(this.gameObject);
            }

            #endregion
        }
    }

    #endregion

    #region Collision Methods

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            health -= coll.gameObject.GetComponent<Bullet>().damage;
            slowDownPercent = coll.gameObject.GetComponent<Bullet>().slowDownPercent;
            DOT = coll.gameObject.GetComponent<Bullet>().damageOverTime;
            DOT_effectTimer = coll.gameObject.GetComponent<Bullet>().DOT_effectTimer;
            slowEffectTimer = coll.gameObject.GetComponent<Bullet>().slowEffectTimer;

            if (DOT_effectTimer > 0)
            {
                startDOTEffect = true;
                DOTMultiplier++;

                if(DOTMultiplier > DOTMultiplierMax)
                {
                    DOTMultiplier = DOTMultiplierMax;
                }
            }

            if(slowEffectTimer > 0)
            {
                SlowMultiplier++;

                if(SlowMultiplier > SlowMultiplierMax)
                {
                    SlowMultiplier = SlowMultiplierMax;
                }
            }

            Destroy(coll.gameObject);
        }
    }

    #endregion
}
