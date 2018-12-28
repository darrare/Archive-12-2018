using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Down, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };
public enum TileStatus { None, Enqueued, Dequeued, Occupied, PartOfPath, TargetOfPath, StartOfPath };

/// <summary>
/// A node that is a part of the graph class
/// </summary>
public class Node
{
    TileStatus status;
    int x, y;
    public float lengthFromStart;
    public float priority;
    public Node backNode;

    /// <summary>
    /// Status of this node
    /// </summary>
    public TileStatus Status
    {
        get { return status; }
        set
        {
            status = value;
            switch (value)
            {
                case TileStatus.Enqueued:
                    Tile.GetComponent<SpriteRenderer>().color = new Color(1, .66f, 0); //orange
                    break;
                case TileStatus.Dequeued:
                    Tile.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case TileStatus.Occupied:
                    Tile.GetComponent<SpriteRenderer>().color = new Color(.25f, .25f, .25f); //dark grey
                    break;
                case TileStatus.PartOfPath:
                    Tile.GetComponent<SpriteRenderer>().color = Color.magenta;
                    break;
                case TileStatus.TargetOfPath:
                    Tile.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case TileStatus.StartOfPath:
                    Tile.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    Tile.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
            }
        }
    }

    /// <summary>
    /// The coordinates of this specific node
    /// </summary>
    public Vector2 Coordinates
    { get { return new Vector2(x, y); } }

    /// <summary>
    /// A dictionary of neighbors
    /// </summary>
    public Dictionary<Direction, Neighbor> Neighbors
    { get; private set; }

    /// <summary>
    /// The game object associated with this specific gridcell
    /// </summary>
    public GameObject Tile
    { get; private set; }

    /// <summary>
    /// Constructor for the node class
    /// </summary>
    /// <param name="tile">The gameobject tile that this node is tied to</param>
    /// <param name="x">The x coordinate of this node</param>
    /// <param name="y">The y coordinate of this node</param>
    public Node(GameObject tile, int x, int y)
    {
        Tile = tile;
        Status = TileStatus.None;
        Neighbors = new Dictionary<Direction, Neighbor>();
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Only called whenever a new grid is made.
    /// </summary>
    /// <param name="dir">The direction of the neighbor</param>
    /// <param name="cell">The neighbor</param>
    public void AddNeighbor(Direction dir, Node node)
    {
        Neighbors.Add(dir, new Neighbor(new Edge(Vector2.Distance(Coordinates, node.Coordinates), this, node), node)); //Edge weights based on pure distance. might want to change that later
    }
}

/// <summary>
/// Neighbor struct
/// </summary>
public struct Neighbor
{
    public Edge edge;
    public Node node;

    /// <summary>
    /// Constructor for the neighbor class
    /// </summary>
    /// <param name="edge">The edge that connects the two neighbors</param>
    /// <param name="node">The neighboring node</param>
    public Neighbor(Edge edge, Node node)
    {
        this.edge = edge;
        this.node = node;
    }
}
