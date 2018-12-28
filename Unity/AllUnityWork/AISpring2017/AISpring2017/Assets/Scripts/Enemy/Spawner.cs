using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variables

    public GameObject bug1;
    GameObject bugCreated;
    public int cooldown = 250;
    public int cooldownConst = 250;
    public int spawnCount = 0;
    public int spawnRateIncrease = 10;
    int totalWaveEnemies = 5;
    int waveEnemyConstant = 15;

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
        if (!Game_Manager.Instance.pauseGame && Game_Manager.Instance.gameStarted)
        {
            if ((Game_Manager.Instance.currentPhase == Game_Manager.Phase.NIGHT) && (spawnCount < totalWaveEnemies) && (!Game_Manager.Instance.gameOver))
            {
                cooldown--;

                if (spawnCount == 0)
                {
                    totalWaveEnemies = waveEnemyConstant * Game_Manager.Instance.waveNumber;
                    Game_Manager.Instance.totalWaveEnemies = totalWaveEnemies;
                }

                if (cooldown <= 0)
                {
                    bugCreated = Instantiate(bug1, transform.position, transform.rotation);
                    bugCreated.GetComponent<Enemy_Controller>().health += 5 * (int)Mathf.Pow(2f, Game_Manager.Instance.waveNumber - 1);
                    cooldown = cooldownConst - (Game_Manager.Instance.waveNumber * 25);

                    if (cooldown < 50)
                    {
                        cooldown = 50;
                    }

                    spawnCount++;
                    Game_Manager.Instance.spawnCount = spawnCount;
                }
            }
            else
            {
                if ((GameObject.FindGameObjectWithTag("Enemy") == null) && (Game_Manager.Instance.currentPhase == Game_Manager.Phase.NIGHT))
                {
                    cooldown = cooldownConst;
                    spawnCount = 0;
                    Game_Manager.Instance.currentPhase = Game_Manager.Phase.DAY;
                    Game_Manager.Instance.dayTimer = Game_Manager.Instance.dayTimerConstant;
                    Game_Manager.Instance.waveNumber++;
                }
            }
        }
	}

    #endregion
}
