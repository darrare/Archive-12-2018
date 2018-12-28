using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public float length;
    public Node[] connectedNodes;

    /// <summary>
    /// Constructor for an edge
    /// </summary>
    /// <param name="length">Length of the edge</param>
    /// <param name="neighborOne">First node</param>
    /// <param name="neighborTwo">Second node</param>
    public Edge(float length, Node neighborOne, Node neighborTwo)
    {
        this.length = length;
        connectedNodes = new Node[] { neighborOne, neighborTwo };
    }
}
