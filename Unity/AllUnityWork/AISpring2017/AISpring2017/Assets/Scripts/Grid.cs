using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;

/// <summary>
/// A grid data structure that holds the data.
/// </summary>
public class Grid
{
    #region properties

    /// <summary>
    /// 2D array of gridcells to represent the graph
    /// </summary>
    public GridCell[,] Graph
    { get; private set; }

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor for the grid class
    /// </summary>
    public Grid()
    {
        Graph = new GridCell[Constants.GRID_SIZE, Constants.GRID_SIZE];

        //Create the initial graph
        GameObject newObj;
        for (int x = 0; x < Constants.GRID_SIZE; x++)
        {
            for (int z = 0; z < Constants.GRID_SIZE; z++)
            {
                newObj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/GridCell"));
                newObj.transform.position = new Vector3(x, 0, z);
                newObj.name = "GridCell: " + x + ", " + z;
                Graph[x, z] = new GridCell(newObj);
            }
        }

        //Set up cells with 4 neighbors
        for (int x = 1; x < Constants.GRID_SIZE - 1; x++)
        {
            for (int z = 1; z < Constants.GRID_SIZE - 1; z++)
            {
                Graph[x, z].AddNeighbor(NeighborDirection.Up, Graph[x, z + 1]);
                Graph[x, z].AddNeighbor(NeighborDirection.Down, Graph[x, z - 1]);
                Graph[x, z].AddNeighbor(NeighborDirection.Left, Graph[x - 1, z]);
                Graph[x, z].AddNeighbor(NeighborDirection.Right, Graph[x + 1, z]);
            }
        }

        //Set up cells with 2 neighbors
        //Bottom left
        Graph[0, 0].AddNeighbor(NeighborDirection.Up, Graph[0, 1]);
        Graph[0, 0].AddNeighbor(NeighborDirection.Right, Graph[1, 0]);

        //Top left
        Graph[0, Constants.GRID_SIZE - 1].AddNeighbor(NeighborDirection.Down, Graph[0, Constants.GRID_SIZE - 2]);
        Graph[0, Constants.GRID_SIZE - 1].AddNeighbor(NeighborDirection.Right, Graph[1, Constants.GRID_SIZE - 1]);

        //Bottom right
        Graph[Constants.GRID_SIZE - 1, 0].AddNeighbor(NeighborDirection.Up, Graph[Constants.GRID_SIZE - 1, 1]);
        Graph[Constants.GRID_SIZE - 1, 0].AddNeighbor(NeighborDirection.Left, Graph[Constants.GRID_SIZE - 2, 0]);

        //Top right
        Graph[Constants.GRID_SIZE - 1, Constants.GRID_SIZE - 1].AddNeighbor(NeighborDirection.Down, Graph[Constants.GRID_SIZE - 1, Constants.GRID_SIZE - 2]);
        Graph[Constants.GRID_SIZE - 1, Constants.GRID_SIZE - 1].AddNeighbor(NeighborDirection.Left, Graph[Constants.GRID_SIZE - 2, Constants.GRID_SIZE - 1]);

        //set up cells with 3 neighbors
        //Top side
        for (int i = 1; i < Constants.GRID_SIZE - 1; i++)
        {
            Graph[i, Constants.GRID_SIZE - 1].AddNeighbor(NeighborDirection.Right, Graph[i + 1, Constants.GRID_SIZE - 1]);
            Graph[i, Constants.GRID_SIZE - 1].AddNeighbor(NeighborDirection.Down, Graph[i, Constants.GRID_SIZE - 2]);
            Graph[i, Constants.GRID_SIZE - 1].AddNeighbor(NeighborDirection.Left, Graph[i - 1, Constants.GRID_SIZE - 1]);
        }

        //bottom side
        for (int i = 1; i < Constants.GRID_SIZE - 1; i++)
        {
            Graph[i, 0].AddNeighbor(NeighborDirection.Right, Graph[i + 1, 0]);
            Graph[i, 0].AddNeighbor(NeighborDirection.Up, Graph[i, 1]);
            Graph[i, 0].AddNeighbor(NeighborDirection.Left, Graph[i - 1, 0]);
        }

        //Left side
        for (int i = 1; i < Constants.GRID_SIZE - 1; i++)
        {
            Graph[0, i].AddNeighbor(NeighborDirection.Right, Graph[1, i]);
            Graph[0, i].AddNeighbor(NeighborDirection.Up, Graph[0, i + 1]);
            Graph[0, i].AddNeighbor(NeighborDirection.Down, Graph[0, i - 1]);
        }

        //Right side
        for (int i = 1; i < Constants.GRID_SIZE - 1; i++)
        {
            Graph[Constants.GRID_SIZE - 1, i].AddNeighbor(NeighborDirection.Left, Graph[Constants.GRID_SIZE - 2, i]);
            Graph[Constants.GRID_SIZE - 1, i].AddNeighbor(NeighborDirection.Up, Graph[Constants.GRID_SIZE - 1, i + 1]);
            Graph[Constants.GRID_SIZE - 1, i].AddNeighbor(NeighborDirection.Down, Graph[Constants.GRID_SIZE - 1, i - 1]);
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// Destroys the graph in the scene so it can be recreated
    /// </summary>
    public void DestroyGraph()
    {
        foreach (GridCell g in Graph)
        {
            Object.Destroy(g.RepresentedCube);
        }
        Graph = null;
    }

    /// <summary>
    /// Gets a random gridcell from the graph
    /// </summary>
    /// <returns>The random GridCell</returns>
    public GridCell GetRandomAvailableCell()
    {
        int x, z;
        
        for (int i = 0; i < 10000; i++) //Check up to 1000 times
        {
            x = Random.Range(0, Constants.GRID_SIZE - 1);
            z = Random.Range(0, Constants.GRID_SIZE - 1);
            if (!Graph[x, z].IsOccupied && !Graph[x, z].IsTargeted)
            {
                return Graph[x, z];
            }
        }
        //If we didn't find a spot in 10000 searches, it probably doesnt exist
        return null;
    }

    #endregion

    #region private methods

    #endregion

    #region Indexer

    /// <summary>
    /// Indexer for the Graph
    /// </summary>
    /// <param name="x">x parameter</param>
    /// <param name="y">y parameter</param>
    /// <returns>The gridcell at that index</returns>
    public GridCell this[int x, int y]
    {
        get { return Graph[x, y]; }
    }

    #endregion
}
