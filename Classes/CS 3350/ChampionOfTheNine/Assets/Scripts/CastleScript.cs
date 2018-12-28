using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a castle
/// </summary>
public class CastleScript : DamagableObjectScript
{
    #region Fields

    [SerializeField]Sprite destroyedSprite;
    [SerializeField]GameObject leftWall;
    [SerializeField]GameObject rightWall;
    SpriteRenderer spriteRenderer;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the castle
    /// </summary>
    /// <param name="healthBar">the castle's health bar</param>
    /// <param name="tag">the castle's tag</param>
    public virtual void Initialize(Image healthBar, string tag, Vector2 location)
    {
        bool isEnemy = tag == Constants.ENEMY_TAG;
        this.healthBar = healthBar;
        gameObject.tag = tag;
        transform.position = location;
        GetComponent<EnemySpawner>().Initialize(isEnemy);
        rightWall.SetActive(isEnemy);
        leftWall.SetActive(!isEnemy);
        maxHealth = Constants.CASTLE_HEALTH;
        hitSound = GameManager.Instance.GameSounds[Constants.CASTLE_HIT_SND];
        deathSound = GameManager.Instance.GameSounds[Constants.CASTLE_DEATH_SND];
        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Initialize();
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles the castle dying
    /// </summary>
    protected override void Death()
    {
        GameManager.Instance.PlaySoundPitched(audioSource, deathSound);
        spriteRenderer.sprite = destroyedSprite;
        WorldScript.Instance.Defeat(gameObject.tag);

        try
        { gameObject.GetComponent<EnemySpawner>().enabled = false; }
        catch { }
    }

    #endregion
}
