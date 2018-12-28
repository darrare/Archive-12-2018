using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a remap controls button
/// </summary>
public class RemapButtonScript : MonoBehaviour
{
    #region Fields

    [SerializeField]InputType inputType;

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the button being pressed
    /// </summary>
    public void Pressed()
    {
        ControlSelectScript.Instance.RemapAxisPressed(inputType);
    }

    #endregion
}
