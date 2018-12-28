using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInteractionsScript : MonoBehaviour
{
    [SerializeField]
    Slider percentage, speed;

    [SerializeField]
    InputField widthInput, heightInput;

    [SerializeField]
    Text errorText;

    public static CanvasInteractionsScript Instance;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// call whenever an error occurs and you want to tell the player
    /// </summary>
    /// <param name="text">The string to send to the player</param>
    public void EditErrorText(string text)
    {
        errorText.text = text;
    }

    /// <summary>
    /// Whenever the speed slider is modified
    /// </summary>
    public void OnSpeedSliderChange()
    {
        GameManager.Instance.timeForUpdate = speed.value / 4;
    }


    /// <summary>
    /// The following methods react to the respective buttons on the screen.
    /// </summary>
    #region BUTTON CLICKS

    public void RandomizeObstacles()
    {
        EditErrorText("");
        GameManager.Instance.RandomizeGraphObstacles(percentage.value);
    }

    public void RebuildGraph()
    {
        int temp;
        if (int.TryParse(widthInput.text, out temp) && int.TryParse(heightInput.text, out temp))
        {
            GameManager.Instance.RebuildGraph(int.Parse(widthInput.text), int.Parse(heightInput.text));
            EditErrorText("");
        }
        else
        {
            EditErrorText("Your input fields have an error in them.");
        }
    }

    public void AStarSearch()
    {
        GameManager.Instance.ResetGraph();
        EditErrorText("");
        GameManager.Instance.AStar();
    }

    public void DjikstrasSearch()
    {
        GameManager.Instance.ResetGraph();
        EditErrorText("");
        GameManager.Instance.Djikstras();
    }

    public void BestFirstSearch()
    {
        GameManager.Instance.ResetGraph();
        EditErrorText("");
        GameManager.Instance.BestFirst();
    }

    public void BreadthFirstSearch()
    {
        //GameManager.Instance.ResetGraph();
        EditErrorText("Breadth first not yet implemented");
        //GameManager.Instance.BreadthFirst();
    }

    #endregion
}
