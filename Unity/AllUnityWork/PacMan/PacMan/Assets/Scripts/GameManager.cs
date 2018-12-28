using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager
{
    #region fields

    static GameManager instance;

    #endregion

    #region Singleton and constructor

    /// <summary>
    /// Singleton for the game manager
    /// </summary>
    public static GameManager Instance
    {
        get { return instance ?? (instance = new GameManager()); }
    }

    /// <summary>
    /// GameManager constructor
    /// </summary>
    protected GameManager()
    {
        Object.DontDestroyOnLoad(new GameObject("gmUpdater", typeof(Updater)));

        IsPaused = true;
        DataObject = GameObject.Find("SpriteToEdit");

        //Only run this once
        if (RuntimePrefabs == null)
        {
            RuntimePrefabs = Resources.LoadAll<GameObject>(Constants.RUNTIME_PREFABS_LOCATION).ToDictionary(t => t.name);
        }

    }

    #endregion

    #region properties

    /// <summary>
    /// Accessor for whether or not the game is paused
    /// </summary>
    public bool IsPaused
    { get; set; }

    /// <summary>
    /// The object in the game that holds all of the data for the level to build off of. Originally named "spritetoedit"
    /// </summary>
    public GameObject DataObject
    { get; set; }

    /// <summary>
    /// Accessor for pacman
    /// </summary>
    public PacManScript PacMan
    { get; set; }
    
    /// <summary>
    /// Collection of all of the prefabs we will need to generate at runtime.
    /// </summary>
    public Dictionary<string, GameObject> RuntimePrefabs
    { get; private set; }

    #endregion

    #region public methods

    /// <summary>
    /// Call to put the game at the very start where the intro music plays and such.
    /// </summary>
    public void StartGame()
    {
        TimerAndEventManager.Instance.FireEvent(EventType.InitStart);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Updates the game loop
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }
        LevelManager.Instance.Update();
        AudioManager.Instance.Update();
        TimerAndEventManager.Instance.Update();
        UIManager.Instance.Update();
    }

    #endregion


    #region Updater class

    class Updater : MonoBehaviour
    {
        /// <summary>
        /// Updates the gameManager
        /// </summary>
        private void Update()
        {
            Instance.Update();
        }
    }

    #endregion

    #region debugging tools

    /// <summary>
    /// Draws a cross on the map for debugging purposes.
    /// </summary>
    /// <param name="centerpos">Center position of the cross</param>
    /// <param name="length">Length of the cross, generally do a 3rd of the total unit</param>
    /// <param name="color">Color of the cross</param>
    /// <param name="duration">Duration. If in update do Time.deltaTime / 3</param>
    public void DrawCross(Vector2 centerpos, float length, Color color, float duration)
    {
        Debug.DrawLine(centerpos - new Vector2(-1, 1) * length, centerpos + new Vector2(-1, 1) * length, color, duration);
        Debug.DrawLine(centerpos - Vector2.one * length, centerpos + Vector2.one * length, color, duration);
    }

    #endregion
}
