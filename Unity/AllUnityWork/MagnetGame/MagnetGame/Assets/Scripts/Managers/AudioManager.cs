using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    #region fields

    static AudioManager instance;

    #endregion

    #region singleton, constructor, and initializer

    /// <summary>
    /// Singleton for the AudioManager class
    /// </summary>
    public static AudioManager Instance
    { get { return (instance ?? (instance = new AudioManager())); } }

    /// <summary>
    /// Private constructor for the Audio manager
    /// </summary>
    private AudioManager()
    {

    }

    #endregion

    #region properties

    #endregion

    #region public methods

    /// <summary>
    /// Update for the Audio Manager
    /// </summary>
    public void Update()
    {

    }

    #endregion private methods
}
