using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A class that generates a graph that can be saved and used for pathfinding.
/// </summary>
public class GenerateGraph : MonoBehaviour
{
    [SerializeField]
    int width;

    [SerializeField]
    int height;

    [SerializeField]
    float units;

    float startingPosX, startingPosY;
    RaycastHit2D hitInfo;

    GraphData graph;

    //for testing
    int x = 2, y = 1;
    Node curNode;

	// Use this for initialization
	void Start ()
    {
        graph = new GraphData(width, height);
        startingPosX = transform.position.x;
        startingPosY = transform.position.y;

        //Generates an entirely new graph
        System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        GenerateGraphFunction();
        watch.Stop();
        Debug.Log((float)watch.ElapsedMilliseconds / 1000);

        //Saves the new graphs data into the binary file
        //graph.SerializeIntoFile("Level1.levelData");

        //Loads a file and stores it in graph.
        //LoadGraph("Level1.levelData");

        GameManager.Instance.IsPaused = true;
        LevelManager.Instance.Graph = graph;
        LevelManager.Instance.InitializeGame();

        curNode = graph.Graph[x, y];
	}

    void Update()
    {
        //for testing 
        //if (Input.GetKeyDown(KeyCode.W) && curNode.Neighbors.ContainsKey(Direction.Up) && (curNode.Neighbors[Direction.Up].Type == NodeType.Available || curNode.Neighbors[Direction.Up].Type == NodeType.Teleporter))
        //{
        //    curNode = curNode.Neighbors.First(t => t.Key == Direction.Up).Value;
        //    LevelManager.Instance.EatDot(curNode.GetWorldPosition());
        //}
        //if (Input.GetKeyDown(KeyCode.A) && curNode.Neighbors.ContainsKey(Direction.Left) && (curNode.Neighbors[Direction.Left].Type == NodeType.Available || curNode.Neighbors[Direction.Left].Type == NodeType.Teleporter))
        //{
        //    curNode = curNode.Neighbors.First(t => t.Key == Direction.Left).Value;
        //    LevelManager.Instance.EatDot(curNode.GetWorldPosition());
        //}
        //if (Input.GetKeyDown(KeyCode.S) && curNode.Neighbors.ContainsKey(Direction.Down) && (curNode.Neighbors[Direction.Down].Type == NodeType.Available || curNode.Neighbors[Direction.Down].Type == NodeType.Teleporter))
        //{
        //    curNode = curNode.Neighbors.First(t => t.Key == Direction.Down).Value;
        //    LevelManager.Instance.EatDot(curNode.GetWorldPosition());
        //}
        //if (Input.GetKeyDown(KeyCode.D) && curNode.Neighbors.ContainsKey(Direction.Right) && (curNode.Neighbors[Direction.Right].Type == NodeType.Available || curNode.Neighbors[Direction.Right].Type == NodeType.Teleporter))
        //{
        //    curNode = curNode.Neighbors.First(t => t.Key == Direction.Right).Value;
        //    LevelManager.Instance.EatDot(curNode.GetWorldPosition());
        //}

        //DrawCross(curNode.GetWorldPosition(), .3f, Color.red, Time.deltaTime / 3);
        //foreach (KeyValuePair<Direction, Node> n in curNode.Neighbors)
        //{
        //    if ((n.Value.Type == NodeType.Available || n.Value.Type == NodeType.Teleporter) && (curNode.Type == NodeType.Available || curNode.Type == NodeType.Teleporter))
        //        DrawCross(n.Value.GetWorldPosition(), .3f, Color.green, Time.deltaTime * 3);
        //}
    }

    /// <summary>
    /// Right click on this script in the inspector to generate the graph. (Maybe only needs to be done once)
    /// </summary>
    //[ContextMenu("Generate Graph")]
    private void GenerateGraphFunction()
    {
        //Start(); //Needs to call start because this script shouldn't ever run when the game is played

        for (float x = startingPosX; x < startingPosX + width * units; x += units)
        {
            for (float y = startingPosY; y < startingPosY + height * units; y += units)
            {
                hitInfo = Physics2D.Raycast(new Vector2(x, y), Vector2.up, units * .1f);
                if (hitInfo.collider && hitInfo.collider.tag == "Wall")
                {
                    //DrawCross(new Vector2(x, y), units * .3f, Color.green, 50000f);
                    graph.AddNode((int)(x - startingPosX), (int)(y - startingPosY), new Vector2(x, y), NodeType.Wall);
                }
                else if (hitInfo.collider && hitInfo.collider.tag == "Teleporter")
                {
                    //DrawCross(new Vector2(x, y), units * .3f, Color.red, 50000f);
                    int identifier = 0;
                    try { identifier = hitInfo.collider.GetComponent<TeleporterIdentifier>().Identifier; }
                    catch { throw new System.Exception("Teleporter doesn't have TeleporterIdentifierScript attatched."); }
                        
                    graph.AddNode((int)(x - startingPosX), (int)(y - startingPosY), new Vector2(x, y), NodeType.Teleporter, identifier);
                }
                else if (hitInfo.collider && hitInfo.collider.tag == "GhostDoor")
                {
                    //DrawCross(new Vector2(x, y), units * .3f, Color.magenta, 50000f);
                    graph.AddNode((int)(x - startingPosX), (int)(y - startingPosY), new Vector2(x, y), NodeType.GhostDoor);
                }
                else
                {
                    //DrawCross(new Vector2(x, y), units * .3f, Color.blue, 50000f);
                    graph.AddNode((int)(x - startingPosX), (int)(y - startingPosY), new Vector2(x, y), NodeType.Available);
                }
            }
        }

        //Build the neighbors
        for (float x = startingPosX; x < startingPosX + width * units; x += units)
        {
            for (float y = startingPosY; y < startingPosY + height * units; y += units)
            {
                if ((int)(y - startingPosY) + 1 <= height - 1)
                    graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Up, graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY) + 1]);
                if ((int)(x - startingPosX) - 1 >= 0)
                    graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Left, graph.Graph[(int)(x - startingPosX) - 1, (int)(y - startingPosY)]);
                if ((int)(x - startingPosX) + 1 <= width - 1)
                    graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Right, graph.Graph[(int)(x - startingPosX) + 1, (int)(y - startingPosY)]);
                if ((int)(y - startingPosY) - 1 >= 0)
                    graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Down, graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY) - 1]);

                //Handle a teleporter node
                if (graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Type == NodeType.Teleporter)
                {
                    if (!graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.ContainsKey(Direction.Up))
                    {
                        graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Up, 
                            graph.Graph.Cast<Node>().First(t => t.Type == NodeType.Teleporter 
                            && t.TeleporterIdentifier == graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].TeleporterIdentifier 
                            && t != graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)]));
                    }
                    if (!graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.ContainsKey(Direction.Left))
                    {
                        graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Left,
                            graph.Graph.Cast<Node>().First(t => t.Type == NodeType.Teleporter
                            && t.TeleporterIdentifier == graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].TeleporterIdentifier
                            && t != graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)]));
                    }
                    if (!graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.ContainsKey(Direction.Right))
                    {
                        graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Right,
                            graph.Graph.Cast<Node>().First(t => t.Type == NodeType.Teleporter
                            && t.TeleporterIdentifier == graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].TeleporterIdentifier
                            && t != graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)]));
                    }
                    if (!graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.ContainsKey(Direction.Down))
                    {
                        graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].Neighbors.Add(Direction.Down,
                            graph.Graph.Cast<Node>().First(t => t.Type == NodeType.Teleporter
                            && t.TeleporterIdentifier == graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)].TeleporterIdentifier
                            && t != graph.Graph[(int)(x - startingPosX), (int)(y - startingPosY)]));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Loads a graph from a pre built file.
    /// </summary>
    /// <param name="fileName">The name of the file you want to load (include the extension)</param>
    private void LoadGraph(string fileName)
    {
        graph.DeserializeFromFile(fileName);
    }

    /// <summary>
    /// Draws a cross on the map for debugging purposes.
    /// </summary>
    /// <param name="centerpos">Center position of the cross</param>
    /// <param name="length">Length of the cross, generally do a 3rd of the total unit</param>
    /// <param name="color">Color of the cross</param>
    /// <param name="duration">Duration. If in update do Time.deltaTime / 3</param>
    private void DrawCross(Vector2 centerpos, float length, Color color, float duration)
    {
        Debug.DrawLine(centerpos - new Vector2(-1, 1) * length, centerpos + new Vector2(-1, 1) * length, color, duration);
        Debug.DrawLine(centerpos - Vector2.one * length, centerpos + Vector2.one * length, color, duration);
    }
}
