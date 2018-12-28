using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;

/// <summary>
/// An enum that represents the different types of Nodes
/// </summary>
public enum NodeType { Teleporter, Available, Wall, GhostDoor };

/// <summary>
/// An enum that represents the possible directions for neighbors, None included for player initial state
/// </summary>
public enum Direction { Up, Left, Down, Right, None };

/// <summary>
/// A class that represents a graph that serves as the mapping agent for Pac-Man
/// </summary>
[Serializable]
public class GraphData
{
    /// <summary>
    /// Property for the graph.
    /// </summary>
    public Node[,] Graph { get; set; }

    /// <summary>
    /// Width of the graph
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Height of the graph
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Constructor for graph data.
    /// </summary>
    /// <param name="width">Width of the graph</param>
    /// <param name="height">Height of the graph</param>
    public GraphData(int width, int height)
    {
        Graph = new Node[width, height];
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Adds a node to the graph
    /// </summary>
    /// <param name="x">The X position of the node</param>
    /// <param name="y">The Y position of the node</param>
    /// <param name="type">The type of node</param>
    /// <param name="teleporterIdentifier">The unique identifier that links two teleporters together</param>
    public void AddNode(int x, int y, Vector2 worldPosition, NodeType type, int teleporterIdentifier = 0)
    {
        Graph[x, y] = new Node(type, worldPosition, teleporterIdentifier);
    }

    /// <summary>
    /// Sets the neighbors of a specific node.
    /// </summary>
    /// <param name="x">The X position of the node</param>
    /// <param name="y">The Y position of the node</param>
    /// <param name="neighbors">The dictionary of neighbors</param>
    public void SetNodeNeighbors(int x, int y, Dictionary<Direction, Node> neighbors)
    {
        Graph[x, y].Neighbors = neighbors;
    }

    /// <summary>
    /// Gets the nearest node to the sent location so that we can "snap" our objects to the grid
    /// </summary>
    /// <param name="location">The location of the object</param>
    /// <returns>The nearest node</returns>
    public Node FindNearestNodeToPoint(Vector2 location)
    {
        float distBetweenTwoPoints = Vector2.Distance(Graph[0, 0].GetWorldPosition(), Graph[0, 1].GetWorldPosition());
        float minDist = Mathf.Infinity;
        Node curNode = Graph[0, 0];

        foreach (Node n in Graph)
        {
            if (n.Type == NodeType.Available || n.Type == NodeType.Teleporter || n.Type == NodeType.GhostDoor)
            {
                if (Vector2.Distance(location, n.GetWorldPosition()) < minDist)
                {
                    curNode = n;
                    minDist = Vector2.Distance(location, n.GetWorldPosition());

                    //If we have found a distance shorter than the distance between two points then we know we have our closest node.
                    if (minDist < distBetweenTwoPoints)
                    {
                        break;
                    }
                }
            }
        }
        return curNode;
    }

    /// <summary>
    /// Gets a random location enarby the ghost so that it can correctly do the frightened mode
    /// </summary>
    /// <param name="location">The location of the ghost</param>
    /// <returns>The node the ghost will pathfind to</returns>
    public Node GetPseudoRandomNodeNearLocation(Vector2 location)
    {
        System.Random rand = new System.Random();
        List<Node> availableNodes = new List<Node>();
        foreach (Node n in Graph)
        {
            if (n.Type != NodeType.Wall && Vector2.Distance(location, n.GetWorldPosition()) < 10)
            {
                availableNodes.Add(n);
            }
        }
        return availableNodes[rand.Next(0, availableNodes.Count)];
    }

    #region Serializations stuff

    /// <summary>
    /// Serialize this graph into a binary file that we can open up later.
    /// </summary>
    /// <param name="fileName">Name of the file you want to save (include extension)</param>
    public void SerializeIntoFile(string fileName)
    {
        using (Stream stream = File.Open(fileName, FileMode.Create))
        {
            BinaryFormatter bin = new BinaryFormatter();
            bin.Serialize(stream, this);
        }
    }

    /// <summary>
    /// Deserialize from a binary file into this data structure
    /// </summary>
    /// <param name="fileName">Name of the file you want to load in (include extension)</param>
    public void DeserializeFromFile(string fileName)
    {
        using (Stream stream = File.Open(fileName, FileMode.Open))
        {
            BinaryFormatter bin = new BinaryFormatter();

            GraphData temp = (GraphData)bin.Deserialize(stream);
            Graph = temp.Graph;
            Width = temp.Width;
            Height = temp.Height;
        }
    }

    #endregion
}

/// <summary>
/// A class that represents a node in a graph that represents Pac-Man
/// </summary>
[Serializable]
public class Node
{
    /// <summary>
    /// The type of node this is
    /// </summary>
    public NodeType Type { get; private set; }

    /// <summary>
    /// The world position this node is referenced to
    /// </summary>
    public Vec2 WorldPosition { get; set; }

    /// <summary>
    /// Converts the Vec2 back into a Vector2
    /// </summary>
    /// <returns>The converted Vec2</returns>
    public Vector2 GetWorldPosition()
    {
        return new Vector2(WorldPosition.X, WorldPosition.Y);
    }

    /// <summary>
    /// The Neighbors of this current node
    /// </summary>
    public Dictionary<Direction, Node> Neighbors { get; set; }

    /// <summary>
    /// An identifier to connect two teleporters together. All other nodes get a default of 0.
    /// </summary>
    public int TeleporterIdentifier { get; set; }

    /// <summary>
    /// Constructor for a Node
    /// </summary>
    /// <param name="type">The type of node this is.</param>
    /// <param name="teleporterIdentifer">The unique identifier that links two teleporters</param>
    public Node(NodeType type, Vector2 worldPosition, int teleporterIdentifer)
    {
        Type = type;
        Neighbors = new Dictionary<Direction, Node>();
        WorldPosition = new Vec2(worldPosition.x, worldPosition.y);
        TeleporterIdentifier = teleporterIdentifer;
    }
}

/// <summary>
/// Custom vec2 class because Vector2 isnt serializable.
/// </summary>
[Serializable]
public class Vec2
{
    public float X { get; set; }
    public float Y { get; set; }

    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }
}