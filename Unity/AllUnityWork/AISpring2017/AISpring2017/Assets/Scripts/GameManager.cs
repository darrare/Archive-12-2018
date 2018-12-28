using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields

    static GameManager instance;

    #endregion

    #region properties

    /// <summary>
    /// singleton for the gamemanager class
    /// </summary>
    public static GameManager Instance
    { get { return instance; } }

    /// <summary>
    /// The grid
    /// </summary>
    public Grid Graph
    { get; set; }

    #endregion

    #region private methods

    /// <summary>
    /// Initializes the game manager
    /// </summary>
    void Start ()
    {
        instance = this;
        Graph = new Grid();
        Camera.main.transform.position = new Vector3((float)Constants.GRID_SIZE / 2 - .5f, 10, (float)Constants.GRID_SIZE / 2 - .5f);
        Camera.main.orthographicSize = (float)Constants.GRID_SIZE / 2 + .5f;

        for (int i = 0; i < Constants.NUM_AGENTS; i++)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Agent"));
        }
	}
	
	/// <summary>
    /// Updates the game manager
    /// </summary>
	void Update ()
    {
		
	}

    #endregion
}
