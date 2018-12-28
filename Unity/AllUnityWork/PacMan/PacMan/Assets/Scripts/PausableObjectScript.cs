using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PausableObjectScript : MonoBehaviour
{
    /// <summary>
    /// Runs even if the game is paused
    /// </summary>
    void Update()
    {
        if (!GameManager.Instance.IsPaused)
        { NotPausedUpdate(); }
        else
        {
            GetComponent<Animator>().speed = 0;
        }
    }

    /// <summary>
    /// Only runs if the game is not paused
    /// </summary>
    protected virtual void NotPausedUpdate()
    {
        GetComponent<Animator>().speed = 1;
    }
}
