using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls MapScript
/// </summary>
public class MapScript : MenuUIScript
{
    #region Fields

    [SerializeField]KingdomButtonScript[] kingdomButtons;

    #endregion

    #region Public Methods

    public void ControlsButtonPressed()
    {
        GameManager.Instance.LoadLevel(Constants.CONTROLS_SCENE);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        for (int i = 0; i < kingdomButtons.Length; i++)
        { kingdomButtons[i].Initialize(i); }
        base.Start();
    }

    #endregion
}
