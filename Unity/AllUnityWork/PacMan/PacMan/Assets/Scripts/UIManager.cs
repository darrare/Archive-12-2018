using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    #region fields

    static UIManager instance;

    UIReferences uiReference;

    float oneUpValue = 0;
    float curHighScore = 1000;

    #endregion

    #region Singleton and constructor

    /// <summary>
    /// Singleton for the UI manager
    /// </summary>
    public static UIManager Instance
    {
        get { return instance ?? (instance = new UIManager()); }
    }

    /// <summary>
    /// UIManager constructor
    /// </summary>
    private UIManager()
    {
        uiReference = Object.Instantiate(GameManager.Instance.RuntimePrefabs["UserInterface"].GetComponent<UIReferences>());
        TimerAndEventManager.Instance.RegisterToEvent(EventType.GameBegin, DisableReadyText);
    }

    /// <summary>
    /// Disables the ready text when the game is started
    /// </summary>
    /// <param name="type"></param>
    void DisableReadyText(EventType type)
    {
        uiReference.Ready.enabled = false;
    }

    #endregion

    #region public methods

    /// <summary>
    /// Updates the UI Manager
    /// </summary>
    public void Update()
    {
        if (oneUpValue > curHighScore)
        {
            curHighScore = oneUpValue;
        }
    }

    /// <summary>
    /// Updates the high score to represent any changes this game.
    /// </summary>
    public void UpdateHighScore()
    {
        uiReference.HighScore.text = ((int)curHighScore).ToString();
    }

    /// <summary>
    /// Adds a value to the current score
    /// </summary>
    /// <param name="value">The value we want to add.</param>
    public void AddScore(float value)
    {
        oneUpValue += value;
        uiReference.OneUp.text = ((int)oneUpValue).ToString();
        //TODO: Handle extra life here. one for every 10k points
    }

    #endregion
}
