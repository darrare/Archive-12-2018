using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph
{
    Node[,] graph;
    public int width, height;

    /// <summary>
    /// Constructor for the Graph class
    /// </summary>
    /// <param name="width">The width of the graph</param>
    /// <param name="height">The height of the graph</param>
    public Graph(int width, int height)
    {
        this.width = width;
        this.height = height;
        graph = new Node[width, height]; //Create the new node grid set at the required size

        //Create each individual node and the tile that represents it
        GameObject h;
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < height; y++)
            {
                h = Object.Instantiate(GameManager.Instance.TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                h.name = "GridCell: " + x + ", " + y;
                graph[x, y] = new Node(h, x, y);
            }
        }

        //Time to set up all of the neighbors within our graph

        //Set up all nodes with 8 neighbors. This is basically every node that is not on the edge
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                graph[x, y].AddNeighbor(Direction.Up, graph[x, y + 1]);
                graph[x, y].AddNeighbor(Direction.Down, graph[x, y - 1]);
                graph[x, y].AddNeighbor(Direction.Left, graph[x - 1, y]);
                graph[x, y].AddNeighbor(Direction.Right, graph[x + 1, y]);
                graph[x, y].AddNeighbor(Direction.TopLeft, graph[x - 1, y + 1]);
                graph[x, y].AddNeighbor(Direction.TopRight, graph[x + 1, y + 1]);
                graph[x, y].AddNeighbor(Direction.BottomLeft, graph[x - 1, y - 1]);
                graph[x, y].AddNeighbor(Direction.BottomRight, graph[x + 1, y - 1]);
            }
        }


        //Set up all nodes with 3 neighbors. These are the 4 corner nodes
        graph[0, 0].AddNeighbor(Direction.Up, graph[0, 1]);
        graph[0, 0].AddNeighbor(Direction.Right, graph[1, 0]);
        graph[0, 0].AddNeighbor(Direction.TopRight, graph[1, 1]);

        graph[0, height - 1].AddNeighbor(Direction.Right, graph[1, height - 1]);
        graph[0, height - 1].AddNeighbor(Direction.Down, graph[0, height - 2]);
        graph[0, height - 1].AddNeighbor(Direction.BottomRight, graph[1, height - 2]);

        graph[width - 1, 0].AddNeighbor(Direction.Up, graph[width - 1, 1]);
        graph[width - 1, 0].AddNeighbor(Direction.Left, graph[width - 2, 0]);
        graph[width - 1, 0].AddNeighbor(Direction.TopLeft, graph[width - 2, 1]);

        graph[width - 1, height - 1].AddNeighbor(Direction.Left, graph[width - 2, height - 1]);
        graph[width - 1, height - 1].AddNeighbor(Direction.Down, graph[width - 1, height - 2]);
        graph[width - 1, height - 1].AddNeighbor(Direction.BottomLeft, graph[width - 2, height - 2]);


        //Set up all nodes with 5 neighbors. Edge nodes that aren't a corner piece
        //Top and bottom edge
        for (int i = 1; i < width - 1; i++)
        {
            //Top edge
            graph[i, height - 1].AddNeighbor(Direction.Down, graph[i, height - 2]);
            graph[i, height - 1].AddNeighbor(Direction.Left, graph[i - 1, height - 1]);
            graph[i, height - 1].AddNeighbor(Direction.Right, graph[i + 1, height - 1]);
            graph[i, height - 1].AddNeighbor(Direction.BottomLeft, graph[i - 1, height - 2]);
            graph[i, height - 1].AddNeighbor(Direction.BottomRight, graph[i + 1, height - 2]);

            //bottom edge
            graph[i, 0].AddNeighbor(Direction.Up, graph[i, 1]);
            graph[i, 0].AddNeighbor(Direction.Left, graph[i - 1, 0]);
            graph[i, 0].AddNeighbor(Direction.Right, graph[i + 1, 0]);
            graph[i, 0].AddNeighbor(Direction.TopLeft, graph[i - 1, 1]);
            graph[i, 0].AddNeighbor(Direction.TopRight, graph[i + 1, 1]);
        }

        //left and right edge
        for (int i = 1; i < height - 1; i++)
        {
            //right side
            graph[width - 1, i].AddNeighbor(Direction.Left, graph[width - 2, i]);
            graph[width - 1, i].AddNeighbor(Direction.Up, graph[width - 1, i + 1]);
            graph[width - 1, i].AddNeighbor(Direction.Down, graph[width - 1, i - 1]);
            graph[width - 1, i].AddNeighbor(Direction.BottomLeft, graph[width - 2, i - 1]);
            graph[width - 1, i].AddNeighbor(Direction.TopLeft, graph[width - 2, i + 1]);

            //left side
            graph[0, i].AddNeighbor(Direction.Right, graph[1, i]);
            graph[0, i].AddNeighbor(Direction.Up, graph[0, i + 1]);
            graph[0, i].AddNeighbor(Direction.Down, graph[0, i - 1]);
            graph[0, i].AddNeighbor(Direction.TopRight, graph[1, i + 1]);
            graph[0, i].AddNeighbor(Direction.BottomRight, graph[1, i - 1]);
        }
    }

    /// <summary>
    /// Whenever we create a new graph, we need to delete all of the game objects of the old one
    /// </summary>
    public void Destructor()
    {
        foreach (Node g in graph)
        {
            Object.Destroy(g.Tile);
        }
        graph = null;
    }

    /// <summary>
    /// Resets the graph to a default clean state
    /// </summary>
    public void ResetGraphToDefault()
    {
        foreach (Node g in graph)
        {
            g.Status = TileStatus.None;
            g.backNode = null;
            g.lengthFromStart = 0;
        }
    }

    /// <summary>
    /// Sets the graph to before it was searched
    /// </summary>
    public void ResetGraphBeforeSearch()
    {
        foreach (Node g in graph)
        {
            if (g.Status == TileStatus.Dequeued || g.Status == TileStatus.Enqueued || g.Status == TileStatus.PartOfPath)
            {
                g.Status = TileStatus.None;
                g.backNode = null;
                g.lengthFromStart = 0;
            }
        }
    }

    /// <summary>
    /// Generates obstacles throughout the grid randomly
    /// </summary>
    /// <param name="percentage">The percentage of the map we want to cover</param>
    public void GenerateObstacles(float percentage)
    {
        ResetGraphToDefault();
        foreach (Node n in ToList().OrderBy(t => Random.value).Take((int)((width * height) * percentage)))
        {
            n.Status = TileStatus.Occupied;
        }
    }

    /// <summary>
    /// uses the 2D array to get a list
    /// </summary>
    List<Node> ToList()
    {
        List<Node> temp = new List<Node>();
        foreach (Node n in graph)
        {
            temp.Add(n);
        }
        return temp;
    }

    #region Indexer

    /// <summary>
    /// Indexer for the Graph
    /// </summary>
    /// <param name="x">x parameter</param>
    /// <param name="y">y parameter</param>
    /// <returns>The node at that index</returns>
    public Node this[int x, int y]
    {
        get
        {
            return graph[x, y];
        }
    }

    #endregion
}
