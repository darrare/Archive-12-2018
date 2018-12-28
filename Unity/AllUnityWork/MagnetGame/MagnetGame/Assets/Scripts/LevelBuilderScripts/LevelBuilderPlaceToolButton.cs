using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilderPlaceToolButton : MonoBehaviour
{
    [SerializeField]
    LevelObjectType type;

    /// <summary>
    /// The button has been clicked
    /// </summary>
    public void ButtonClick()
    {
        UIManager.Instance.LevelBuilderUI.PlacingToolClicked(type);
    }
}
