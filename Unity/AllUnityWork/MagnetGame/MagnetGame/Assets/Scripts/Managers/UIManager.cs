using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    #region fields

    static UIManager instance;

    #endregion

    #region singleton, constructor, and initializer

    /// <summary>
    /// Singleton for the UIManager class
    /// </summary>
    public static UIManager Instance
    { get { return (instance ?? (instance = new UIManager())); } }

    /// <summary>
    /// Private constructor for the UI manager
    /// </summary>
    private UIManager()
    {

    }

    #endregion

    #region properties

    /// <summary>
    /// The levelbuilderUI
    /// </summary>
    public LevelBuilderUIScript LevelBuilderUI
    { get; set; }

    #endregion

    #region public methods

    /// <summary>
    /// Update for the UI manager
    /// </summary>
    public void Update()
    {

    }

    #endregion private methods
}
