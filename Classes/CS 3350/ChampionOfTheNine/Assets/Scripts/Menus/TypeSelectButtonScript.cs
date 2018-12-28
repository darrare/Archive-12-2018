using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls TypeSelectButtonScript
/// </summary>
public class TypeSelectButtonScript : MonoBehaviour
{
    #region Fields

    [SerializeField]CharacterType type;

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the button being pressed
    /// </summary>
    public void Pressed()
    {
        CharacterCreationScript.Instance.SetCharacterType(type);
    }

    #endregion
}
