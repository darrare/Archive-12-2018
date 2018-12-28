using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a kingdom button
/// </summary>
public class KingdomButtonScript : MonoBehaviour
{
    #region Fields

    KingdomName kingdom;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the button
    /// </summary>
    public void Initialize(int kingdomNum)
    {
        kingdom = GameManager.Instance.CurrentSave.Kingdoms[kingdomNum];
        Button buttonScript = GetComponent<Button>();
        buttonScript.interactable = kingdomNum <= GameManager.Instance.CurrentSave.CurrentKingdom;
        buttonScript.onClick.AddListener(Pressed);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Handles the kingdom button being pressed
    /// </summary>
    private void Pressed()
    {
        GameManager.Instance.LoadGameLevel(kingdom);
    }

    #endregion
}
