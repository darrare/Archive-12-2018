using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gamemanager class
/// </summary>
public class GameManager
{
    static GameManager instance;

    #region singleton and constructor

    /// <summary>
    /// singleton for the game manager class
    /// </summary>
    public static GameManager Instance
    { get { return instance ?? (instance = new GameManager()); } }

    /// <summary>
    /// Constructor for the gamemanager class
    /// </summary>
    private GameManager()
    {
        Object.DontDestroyOnLoad(new GameObject("Updater", typeof(Updater)));
    }

    #endregion

    #region properties

    /// <summary>
    /// Accessor for the player character
    /// </summary>
    public PlayerController Player
    { get; set; }

    /// <summary>
    /// The main camera script
    /// </summary>
    public CameraControllerScript Camera
    { get; set; }

    #endregion

    #region public methods

    #endregion

    #region private methods

    /// <summary>
    /// Updates the gamemanager
    /// </summary>
    void Update()
    {
        AudioManager.Instance.Update();
        EffectManager.Instance.Update();
    }

    #endregion

    #region internal updater class

    /// <summary>
    /// Updates the game manager class
    /// </summary>
    class Updater : MonoBehaviour
    {
        void Update()
        {
            instance.Update();
        }
    }

    #endregion
}
