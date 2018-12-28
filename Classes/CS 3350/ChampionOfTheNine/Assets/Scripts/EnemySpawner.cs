using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the enemy spawner
/// </summary>
public class EnemySpawner : PauseableObjectScript
{
    #region Fields

    [SerializeField]Transform spawnLocation;
    Timer spawnTimer;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the spawner
    /// </summary>
    public void Initialize(bool active)
    {
        Initialize();
        if (active)
        {
            spawnTimer = new Timer(2);
            spawnTimer.Register(SpawnEnemy);
            spawnTimer.Start();
            GetComponent<SpriteRenderer>().color = Constants.ENEMY_COLOR;
        }
        else
        { this.enabled = false; }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Updates the object when it isn't paused
    /// </summary>
    protected override void NotPausedUpdate()
    {
        spawnTimer.Update();
    }

    /// <summary>
    /// Spawns an enemy
    /// </summary>
    protected void SpawnEnemy()
    {
        Instantiate(GameManager.Instance.EnemyPrefabs[(CharacterType)Random.Range(0, 3)], spawnLocation.position, transform.rotation);

        // Resets the spawn timer
        float level = GameManager.Instance.CurrentSave.Kingdoms.IndexOf(GameManager.Instance.CurrentLoadedKingdom);
        spawnTimer.TotalSeconds = Random.Range(Constants.AI_MIN_SPAWN_TIME - (level / 2), Constants.AI_MAX_SPAWN_TIME - level);
        spawnTimer.Start();
    }

    #endregion
}
