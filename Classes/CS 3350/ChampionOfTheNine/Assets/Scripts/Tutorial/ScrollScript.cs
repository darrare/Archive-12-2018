using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls ScrollScript
/// </summary>
public class ScrollScript : MonoBehaviour
{
    #region Fields

    [SerializeField]Text content;
    RectTransform rect;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the content text in the scroll, setting the scroll height appropriately
    /// </summary>
    public string Text
    {
        get { return content.text; }
        set
        {
            content.text = value;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, LayoutUtility.GetPreferredHeight(content.rectTransform) + 
                (-content.rectTransform.anchoredPosition.y * 2) + 30);
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Awake()
    {
        rect = (RectTransform)transform;
    }

    #endregion
}
