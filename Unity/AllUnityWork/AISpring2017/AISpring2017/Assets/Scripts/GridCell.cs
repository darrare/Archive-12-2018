using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum NeighborDirection { Up, Down, Left, Right , UL, UR, DL, DR};

/// <summary>
/// A gridcell that is part of a 2D array
/// </summary>
public class GridCell
{
    #region fields

    bool isOccupied;
    bool isTargeted;

    #endregion

    #region Properties

    /// <summary>
    /// A dictionary of neighboring gridcells
    /// </summary>
    public Dictionary<NeighborDirection, GridCell> Neighbors
    { get; private set; }

    /// <summary>
    /// The position of the gridcell object
    /// </summary>
    public Vector3 Position
    {
        get { return RepresentedCube.transform.position; }
    }

    /// <summary>
    /// Whether or not this cell is currently occupied
    /// </summary>
    public bool IsOccupied
    {
        get { return isOccupied; }
        set
        {
            isOccupied = value;
            if (value)
            {
                RepresentedCube.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (RepresentedCube.GetComponent<Renderer>().material.color != Color.blue)
            {
                RepresentedCube.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    /// <summary>
    /// Whether or not this cell is currently targeted
    /// </summary>
    public bool IsTargeted
    {
        get { return isTargeted; }
        set
        {
            isTargeted = value;
            if (value)
            {
                RepresentedCube.GetComponent<Renderer>().material.color = Color.blue;
            }
            else if (RepresentedCube.GetComponent<Renderer>().material.color != Color.red)
            {
                RepresentedCube.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    /// <summary>
    /// The game object associated with this specific gridcell
    /// </summary>
    public GameObject RepresentedCube
    { get; private set; }


    #endregion

    #region Constructor

    /// <summary>
    /// Constructor for a grid cell
    /// </summary>
    public GridCell(GameObject representedCube)
    {
        RepresentedCube = representedCube;
        IsOccupied = false;
        IsTargeted = false;
        Neighbors = new Dictionary<NeighborDirection, GridCell>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Only called whenever a new grid is made.
    /// </summary>
    /// <param name="dir">The direction of the neighbor</param>
    /// <param name="cell">The neighbor</param>
    public void AddNeighbor(NeighborDirection dir, GridCell cell)
    {
        Neighbors.Add(dir, cell);
    }

    /// <summary>
    /// Used for the initial primitive pathfinding.
    /// </summary>
    /// <returns>A random neighbor</returns>
    public GridCell GetRandomAvailableNeighbor()
    {
        List<GridCell> availNeighbors = Neighbors.Values.Where(t => !t.IsOccupied && !t.isTargeted).ToList();
        return availNeighbors.Count != 0 ? availNeighbors.ElementAt(Random.Range(0, availNeighbors.Count - 1)) : null;
    }

    #endregion
}
