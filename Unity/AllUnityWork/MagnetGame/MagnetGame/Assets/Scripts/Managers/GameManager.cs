using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region fields

    static GameManager instance;

    #endregion

    #region singleton, constructor, and initializer

    /// <summary>
    /// Singleton for the GameManager class
    /// </summary>
    public static GameManager Instance
    { get { return (instance ?? (instance = new GameManager())); } }

    /// <summary>
    /// Private constructor for the game manager
    /// </summary>
    private GameManager()
    {
        Object.Instantiate(new GameObject("GameManager", typeof(Updater)));
    }

    #endregion

    #region properties

    #endregion

    #region public methods

    void Update()
    {
        AudioManager.Instance.Update();
        UIManager.Instance.Update();
    }

    #endregion private methods

    #region Updater class

    class Updater : MonoBehaviour
    {
        void Update()
        {
            instance.Update();
        }
    }

    #endregion
}
