using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    bool isSearching = false;

    delegate void SearchBehaviour();
    SearchBehaviour behaviour;

    float timer = 0;
    public float timeForUpdate;

    //Variables for the searching algorithm
    PriorityQueue queue;
    Node curNode;


    /// <summary>
    /// The prefab used for the tiles so we dont have to load it for every single tile
    /// </summary>
    public GameObject TilePrefab
    { get; private set; }

    /// <summary>
    /// The graph of the game
    /// </summary>
    public Graph Graph
    { get; private set; }

    /// <summary>
    /// The starting node
    /// </summary>
    public Node StartNode
    { get; set; }

    /// <summary>
    /// The targeted node
    /// </summary>
    public Node TargetNode
    { get; set; }

	/// <summary>
    /// First things first, set this game manager as the only game manager
    /// </summary>
	void Awake ()
    {
        Instance = this;
	}

    /// <summary>
    /// Instantiate everything else in the game manager
    /// </summary>
    void Start()
    {
        TilePrefab = Resources.Load<GameObject>("Prefabs/TilePrefab");
        Graph = new Graph(30, 30);
        Camera.main.transform.position = new Vector3(15f, 15f, -10);
        Camera.main.orthographicSize = 30 * .66f;
    }
	
	/// <summary>
    /// Updates the game manager
    /// </summary>
	void Update ()
    {
        //only allow modifications to the graph if the player is not searching
        if (!isSearching)
        {
            HandleUserInput();
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= timeForUpdate)
            {
                behaviour();
                timer = 0;
            }
        }
	}

    void HandleUserInput()
    {
        //Left click to add obstacles
        if (Input.GetMouseButton(0))
        {
            Vector2 selectedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPos = new Vector2(selectedPos.x + .5f, selectedPos.y + .5f);
            if (selectedPos.x > Graph.width || selectedPos.x < 0 || selectedPos.y > Graph.height || selectedPos.y < 0)
            {
                return;
            }
            Graph[(int)selectedPos.x, (int)selectedPos.y].Status = TileStatus.Occupied;
        }
        //right click to remove obstacles
        else if (Input.GetMouseButton(1))
        {
            Vector2 selectedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPos = new Vector2(selectedPos.x + .5f, selectedPos.y + .5f);
            if (selectedPos.x > Graph.width || selectedPos.x < 0 || selectedPos.y > Graph.height || selectedPos.y < 0)
            {
                return;
            }
            Graph[(int)selectedPos.x, (int)selectedPos.y].Status = TileStatus.None;
        }
        //middle mouse click to set target
        else if (Input.GetMouseButton(2))
        {
            if (TargetNode != null)
            {
                TargetNode.Status = TileStatus.None;
                Vector2 selectedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                selectedPos = new Vector2(selectedPos.x + .5f, selectedPos.y + .5f);
                if (selectedPos.x > Graph.width || selectedPos.x < 0 || selectedPos.y > Graph.height || selectedPos.y < 0)
                {
                    return;
                }
                Graph[(int)selectedPos.x, (int)selectedPos.y].Status = TileStatus.TargetOfPath;
                TargetNode = Graph[(int)selectedPos.x, (int)selectedPos.y];
            }
            else
            {
                Vector2 selectedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                selectedPos = new Vector2(selectedPos.x + .5f, selectedPos.y + .5f);
                if (selectedPos.x > Graph.width || selectedPos.x < 0 || selectedPos.y > Graph.height || selectedPos.y < 0)
                {
                    return;
                }
                Graph[(int)selectedPos.x, (int)selectedPos.y].Status = TileStatus.TargetOfPath;
                TargetNode = Graph[(int)selectedPos.x, (int)selectedPos.y];
            }
        }
        //space bar to set start node
        else if (Input.GetKey(KeyCode.Space))
        {
            if (StartNode != null)
            {
                StartNode.Status = TileStatus.None;
                Vector2 selectedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                selectedPos = new Vector2(selectedPos.x + .5f, selectedPos.y + .5f);
                if (selectedPos.x > Graph.width || selectedPos.x < 0 || selectedPos.y > Graph.height || selectedPos.y < 0)
                {
                    return;
                }
                Graph[(int)selectedPos.x, (int)selectedPos.y].Status = TileStatus.StartOfPath;
                StartNode = Graph[(int)selectedPos.x, (int)selectedPos.y];
            }
            else
            {
                Vector2 selectedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                selectedPos = new Vector2(selectedPos.x + .5f, selectedPos.y + .5f);
                if (selectedPos.x > Graph.width || selectedPos.x < 0 || selectedPos.y > Graph.height || selectedPos.y < 0)
                {
                    return;
                }
                Graph[(int)selectedPos.x, (int)selectedPos.y].Status = TileStatus.StartOfPath;
                StartNode = Graph[(int)selectedPos.x, (int)selectedPos.y];
            }
        }
    }

    /// <summary>
    /// Generates obstacles throughout the grid randomly
    /// </summary>
    /// <param name="percentage">The percentage of the map we want to cover</param>
    public void RandomizeGraphObstacles(float percentage)
    {
        isSearching = false;
        Graph.GenerateObstacles(percentage);
    }

    /// <summary>
    /// Builds a new graph
    /// </summary>
    /// <param name="width">width of the new graph</param>
    /// <param name="height">height of the new graph</param>
    public void RebuildGraph(int width, int height)
    {
        isSearching = false;
        Graph.Destructor();
        StartNode = null;
        TargetNode = null;
        Graph = new Graph(width, height);
        Camera.main.transform.position = new Vector3(width / 2, height / 2, -10);
        Camera.main.orthographicSize = width > height ? width * .66f : height * .66f;
    }

    /// <summary>
    /// Sets all the queued/dequeued nodes to none
    /// </summary>
    public void ResetGraph()
    {
        Graph.ResetGraphBeforeSearch();
    }

    /// <summary>
    /// Performs an A* search from StartNode to TargetNode
    /// </summary>
    public void AStar()
    {
        if (StartNode != null && TargetNode != null)
        {
            isSearching = true;
            behaviour = AStarSearch;
            queue = new PriorityQueue();
            queue.Enqueue(StartNode, 0);
        }
        else
        {
            CanvasInteractionsScript.Instance.EditErrorText("Either a start node or target node has not been set.");
        }
    }

    /// <summary>
    /// Performs djikstras search from StartNode to TargetNode
    /// </summary>
    public void Djikstras()
    {
        if (StartNode != null && TargetNode != null)
        {
            isSearching = true;
            behaviour = DjikstrasSearch;
            queue = new PriorityQueue();
            queue.Enqueue(StartNode, 0);
        }
        else
        {
            CanvasInteractionsScript.Instance.EditErrorText("Either a start node or target node has not been set.");
        }
    }

    /// <summary>
    /// performs a best first search from StartNode to TargetNode
    /// </summary>
    public void BestFirst()
    {
        if (StartNode != null && TargetNode != null)
        {
            isSearching = true;
            behaviour = BestFirstSearch;
            queue = new PriorityQueue();
            queue.Enqueue(StartNode, 0);
        }
        else
        {
            CanvasInteractionsScript.Instance.EditErrorText("Either a start node or target node has not been set.");
        }
    }

    /// <summary>
    /// performs a breadth first search from StartNode to TargetNode
    /// </summary>
    public void BreadthFirst()
    {
        isSearching = true;
        behaviour = BreadthFirstSearch;
    }

    /// <summary>
    /// The actual A* searching algorithm. Called every frame
    /// </summary>
    void AStarSearch()
    {
        if (queue.Count > 0)
        {
            curNode = queue.Dequeue();
            if (curNode.Status != TileStatus.StartOfPath && curNode.Status != TileStatus.TargetOfPath)
                curNode.Status = TileStatus.Dequeued;

            //If we have found the target node
            if (curNode == TargetNode)
            {
                while(curNode.backNode != null)
                {
                    if (curNode.Status != TileStatus.StartOfPath && curNode.Status != TileStatus.TargetOfPath)
                    {
                        curNode.Status = TileStatus.PartOfPath;
                    }
                    curNode = curNode.backNode;
                }

                isSearching = false;
                return;
            }

            foreach (KeyValuePair<Direction, Neighbor> n in curNode.Neighbors.Where(t => t.Value.node.Status != TileStatus.Dequeued && t.Value.node.Status != TileStatus.Occupied && t.Value.node.Status != TileStatus.StartOfPath))
            {
                float priority = curNode.lengthFromStart + n.Value.edge.length + Vector2.Distance(n.Value.node.Coordinates, TargetNode.Coordinates);
                if (n.Value.node.Status != TileStatus.Enqueued)
                {
                    queue.Enqueue(n.Value.node, priority);
                    n.Value.node.priority = priority;
                    if (n.Value.node.Status != TileStatus.TargetOfPath)
                        n.Value.node.Status = TileStatus.Enqueued;
                    n.Value.node.backNode = curNode;
                    n.Value.node.lengthFromStart = curNode.lengthFromStart + n.Value.edge.length;
                }
                else if (n.Value.node.priority > priority)
                {
                    queue.ModifyPriority(n.Value.node, priority);
                    n.Value.node.priority = priority;
                    if (n.Value.node.Status != TileStatus.TargetOfPath)
                        n.Value.node.Status = TileStatus.Enqueued;
                    n.Value.node.backNode = curNode;
                    n.Value.node.lengthFromStart = curNode.lengthFromStart + n.Value.edge.length;
                }
            }
        }
        else
        {
            CanvasInteractionsScript.Instance.EditErrorText("A path could not be found");
        }
    }

    /// <summary>
    /// The actual Djikstras searching algorithm. Called every frame
    /// </summary>
    void DjikstrasSearch()
    {
        if (queue.Count > 0)
        {
            curNode = queue.Dequeue();
            if (curNode.Status != TileStatus.StartOfPath && curNode.Status != TileStatus.TargetOfPath)
                curNode.Status = TileStatus.Dequeued;

            //If we have found the target node
            if (curNode == TargetNode)
            {
                while (curNode.backNode != null)
                {
                    if (curNode.Status != TileStatus.StartOfPath && curNode.Status != TileStatus.TargetOfPath)
                    {
                        curNode.Status = TileStatus.PartOfPath;
                    }
                    curNode = curNode.backNode;
                }

                isSearching = false;
                return;
            }

            foreach (KeyValuePair<Direction, Neighbor> n in curNode.Neighbors.Where(t => t.Value.node.Status != TileStatus.Dequeued && t.Value.node.Status != TileStatus.Occupied && t.Value.node.Status != TileStatus.StartOfPath))
            {
                float priority = curNode.lengthFromStart + n.Value.edge.length;
                if (n.Value.node.Status != TileStatus.Enqueued)
                {
                    queue.Enqueue(n.Value.node, priority);
                    n.Value.node.priority = priority;
                    if (n.Value.node.Status != TileStatus.TargetOfPath)
                        n.Value.node.Status = TileStatus.Enqueued;
                    n.Value.node.backNode = curNode;
                    n.Value.node.lengthFromStart = curNode.lengthFromStart + n.Value.edge.length;
                }
                else if (n.Value.node.priority > priority)
                {
                    queue.ModifyPriority(n.Value.node, priority);
                    n.Value.node.priority = priority;
                    if (n.Value.node.Status != TileStatus.TargetOfPath)
                        n.Value.node.Status = TileStatus.Enqueued;
                    n.Value.node.backNode = curNode;
                    n.Value.node.lengthFromStart = curNode.lengthFromStart + n.Value.edge.length;
                }
            }
        }
        else
        {
            CanvasInteractionsScript.Instance.EditErrorText("A path could not be found");
        }
    }

    /// <summary>
    /// The actual Best first searching algorithm. Called every frame
    /// </summary>
    void BestFirstSearch()
    {
        if (queue.Count > 0)
        {
            curNode = queue.Dequeue();
            if (curNode.Status != TileStatus.StartOfPath && curNode.Status != TileStatus.TargetOfPath)
                curNode.Status = TileStatus.Dequeued;

            //If we have found the target node
            if (curNode == TargetNode)
            {
                while (curNode.backNode != null)
                {
                    if (curNode.Status != TileStatus.StartOfPath && curNode.Status != TileStatus.TargetOfPath)
                    {
                        curNode.Status = TileStatus.PartOfPath;
                    }
                    curNode = curNode.backNode;
                }

                isSearching = false;
                return;
            }

            foreach (KeyValuePair<Direction, Neighbor> n in curNode.Neighbors.Where(t => t.Value.node.Status != TileStatus.Dequeued && t.Value.node.Status != TileStatus.Occupied && t.Value.node.Status != TileStatus.StartOfPath))
            {
                float priority = Vector2.Distance(n.Value.node.Coordinates, TargetNode.Coordinates);
                if (n.Value.node.Status != TileStatus.Enqueued)
                {
                    queue.Enqueue(n.Value.node, priority);
                    n.Value.node.priority = priority;
                    if (n.Value.node.Status != TileStatus.TargetOfPath)
                        n.Value.node.Status = TileStatus.Enqueued;
                    n.Value.node.backNode = curNode;
                    n.Value.node.lengthFromStart = curNode.lengthFromStart + n.Value.edge.length;
                }
                else if (n.Value.node.lengthFromStart > curNode.lengthFromStart + n.Value.edge.length)
                {
                    queue.ModifyPriority(n.Value.node, priority);
                    n.Value.node.priority = priority;
                    if (n.Value.node.Status != TileStatus.TargetOfPath)
                        n.Value.node.Status = TileStatus.Enqueued;
                    n.Value.node.backNode = curNode;
                    n.Value.node.lengthFromStart = curNode.lengthFromStart + n.Value.edge.length;
                }
            }
        }
        else
        {
            CanvasInteractionsScript.Instance.EditErrorText("A path could not be found");
        }
    }

    /// <summary>
    /// The actual Breadth first searching algorithm. Called every frame
    /// </summary>
    void BreadthFirstSearch()
    {

    }
}
