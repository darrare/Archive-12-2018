using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This class controls the behavior of the Tool Selection and Options panel such that it will roll in and roll out decently.
/// </summary>
public class ToolScenePanelControl : MonoBehaviour
{
    bool isHidden = true;
    float transitionSpeed = 1500f;
    RectTransform rTrans;
    float width;
    Vector2 nonHiddenPosition, hiddenPosition;

    /// <summary>
    /// Accessor for the isHidden variable
    /// </summary>
    public bool IsHidden
    { get { return isHidden; } }

	// Use this for initialization
	void Start ()
    {
        rTrans = GetComponent<RectTransform>();
        nonHiddenPosition = rTrans.anchoredPosition;
        width = rTrans.rect.width;
        hiddenPosition = new Vector2(rTrans.anchoredPosition.x + width * .9f, rTrans.anchoredPosition.y);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (isHidden)
        {
            if (rTrans.anchoredPosition.x >= hiddenPosition.x)
            {
                return;
            }

            //If the next step would move past the position we want to be
            if (rTrans.anchoredPosition.x + transitionSpeed * Time.deltaTime > hiddenPosition.x)
            {
                rTrans.anchoredPosition = new Vector2(hiddenPosition.x, rTrans.anchoredPosition.y);
            }
            else
            {
                rTrans.anchoredPosition = new Vector2(rTrans.anchoredPosition.x + transitionSpeed * Time.deltaTime, rTrans.anchoredPosition.y);
            }
        }
        else
        {
            if (rTrans.anchoredPosition.x <= nonHiddenPosition.x)
            {
                return;
            }

            //If the next step would move past the position we want to be
            if (rTrans.anchoredPosition.x - transitionSpeed * Time.deltaTime < nonHiddenPosition.x)
            {
                rTrans.anchoredPosition = new Vector2(nonHiddenPosition.x, rTrans.anchoredPosition.y);
            }
            else
            {
                rTrans.anchoredPosition = new Vector2(rTrans.anchoredPosition.x - transitionSpeed * Time.deltaTime, rTrans.anchoredPosition.y);
            }
        }
	}

    /// <summary>
    /// Called when the mouse either enters or exits the panel.
    /// </summary>
    /// <param name="var">True if exit, false if enter</param>
    public void ChangeIsHidden(bool var)
    {
        isHidden = var;
    }
}
