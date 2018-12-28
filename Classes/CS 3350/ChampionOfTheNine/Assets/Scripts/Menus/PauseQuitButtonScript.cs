using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the pause quit button
/// </summary>
public class PauseQuitButtonScript : MonoBehaviour
{
    #region Fields

    [SerializeField]bool tutorial;

    #endregion

    #region Private Methods

    /// <summary>
    /// Activates when the mouse clicks on the collider
    /// </summary>
    private void OnMouseDown()
    {
        if (tutorial)
        {
            if (GameManager.Instance.CurrentSaveName != Constants.TUT_SAVE)
            { Application.LoadLevel(Constants.CHAR_CREATE_SCENE); }
            else
            { Application.LoadLevel(Constants.MAIN_MENU_SCENE); }
        }
        else
        { Application.LoadLevel(Constants.MAP_SCENE); }
        
    }

    #endregion
}
