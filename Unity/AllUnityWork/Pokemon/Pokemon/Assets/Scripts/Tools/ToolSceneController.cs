using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// This class controls the tool scene, such as drawing the debug lines and setting up the tile data so we can save and load it.
/// </summary>
public class ToolSceneController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    Sprite spriteToEdit;

    [SerializeField]
    Text errorText;

    [SerializeField]
    Camera mainCam;

    [SerializeField]
    ToolScenePanelControl panelController;

    int pixelsPerUnit;
    int width, height;
    int gridWidth, gridHeight;

    TileData[,] grid;

    TileType currentTool = TileType.Normal;

    // Use this for initialization
    void Start ()
    {
        #region Error checking

        spriteToEdit = spriteRenderer.sprite;
	    if (!spriteToEdit)
        {
            errorText.text = "Error: SpriteToEdit GameObject does not have a sprite associated with it. Add a sprite to the SpriteRenderer component of the SpriteToEdit GameObject in the inspector.";
            return;
        }
        if (spriteToEdit.pivot != Vector2.zero)
        {
            errorText.text = "Error: Sprite associated with SpriteToEdit GameObject needs to have its pivot point set to bottom left. Change it in the inspector on the art asset itself.";
            spriteRenderer.sprite = null;
            return;
        }
        if (spriteToEdit.pixelsPerUnit != Mathf.Floor(spriteToEdit.pixelsPerUnit))
        {
            errorText.text = "Error: Sprite associated with SpriteToEdit GameObject needs to have a whole number as its Pixels per Unit. It is currently set at" + spriteToEdit.pixelsPerUnit + ". Change it in the inspector on the art asset itself.";
            spriteRenderer.sprite = null;
            return;
        }

        #endregion

        #region Calculations and cam setup

        pixelsPerUnit = (int)spriteToEdit.pixelsPerUnit;
        width = spriteToEdit.texture.width;
        height = spriteToEdit.texture.height;
        gridWidth = width / pixelsPerUnit;
        gridHeight = height / pixelsPerUnit;

        mainCam.transform.position = new Vector3((float)(spriteToEdit.texture.width / pixelsPerUnit) / 2, (float)(spriteToEdit.texture.height / pixelsPerUnit) / 2, mainCam.transform.position.z);

        //Create the right sized grid
        grid = new TileData[gridWidth, gridHeight];

        //Fill the grid with data
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new TileData(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x + 1, y + 1), new Vector2(x, y + 1));
            }
        }

        #endregion

        #region Draw Grid

        //Draws the base gridlines as a guide for the player
        //for (int x = 0; x < gridWidth; x++)
        //{
        //    Debug.DrawLine(new Vector3(x, 0, 0), new Vector3(x, gridHeight, 0), Color.black, Mathf.Infinity);
        //}

        //for (int y = 0; y < gridHeight; y++)
        //{
        //    Debug.DrawLine(new Vector3(0, y, 0), new Vector3(gridWidth, y, 0), Color.black, Mathf.Infinity);
        //}

        #endregion
    }

    void OnDrawGizmos()
    {
        //Draw gridlines
        Gizmos.color = Color.black;
        for (int x = 0; x < gridWidth + 1; x++)
        {
            Gizmos.DrawLine(new Vector3(x, 0, 0), new Vector3(x, gridHeight, 0));
        }
        for (int y = 0; y < gridHeight + 1; y++)
        {
            Gizmos.DrawLine(new Vector3(0, y, 0), new Vector3(gridWidth, y, 0));
        }

        //Draw the associated image for each of the TileData in the grid.
        foreach (TileData t in grid)
        {
            switch (t.type)
            {
                case TileType.Collider:
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(t.bl, t.tr);
                    Gizmos.DrawLine(t.tl, t.br);
                    break;
                case TileType.TriggerCollider:
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(t.bl, t.tr);
                    Gizmos.DrawLine(t.tl, t.br);
                    break;
                case TileType.Interactable:
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(t.bl, t.tr);
                    Gizmos.DrawLine(t.tl, t.br);
                    break;
                case TileType.OneWayUp:
                    Gizmos.color = Color.blue;

                    break;
                case TileType.OneWayLeft:
                    Gizmos.color = Color.blue;

                    break;
                case TileType.OneWayRight:
                    Gizmos.color = Color.blue;

                    break;
                case TileType.OneWayDown:
                    Gizmos.color = Color.blue;

                    break;
            }

        }
    }

    /// <summary>
    /// Update runs every frame and does game loop calculations
    /// </summary>
    void Update()
    {
        //If the player is holding down the mouse button && not on the panel
        if (Input.GetMouseButton(0) && panelController.IsHidden)
        {
            //Gets the tile index that the mouse is currently over
            Vector2 loc = mainCam.ScreenToWorldPoint(Input.mousePosition);
            loc = new Vector2(Mathf.Floor(loc.x), Mathf.Floor(loc.y));

            //Error check to make sure the mouse is in a location that has a tile.
            if (loc.x > gridWidth - 1 || loc.y > gridHeight - 1 || loc.x < 0 || loc.y < 0)
            {
                
            }
            else
            {
                //Changes the tile to the tool that is currently held.
                grid[(int)loc.x, (int)loc.y].ChangeTileType(currentTool);
            }
        }
    }

    #region Button Clicks

    /// <summary>
    /// Called when the button to get the collider tool is clicked
    /// </summary>
    public void ButtonClickCollider()
    {
        currentTool = TileType.Collider;
    }

    /// <summary>
    /// Called when the button to get the trigger collider tool is clicked
    /// </summary>
    public void ButtonClickTriggerCollider()
    {
        currentTool = TileType.TriggerCollider;
    }

    /// <summary>
    /// Called when the button to get the interactable object tool is clicked
    /// </summary>
    public void ButtonClickInteractable()
    {
        currentTool = TileType.Interactable;
    }

    /// <summary>
    /// Called when the button to get the undo tool is clicked
    /// </summary>
    public void ButtonClickUndo()
    {
        currentTool = TileType.Normal;
    }

    /// <summary>
    /// Called when the button to get the one way up tool is clicked
    /// </summary>
    public void ButtonClickOneWayUp()
    {
        currentTool = TileType.OneWayUp;
    }

    /// <summary>
    /// Called when the button to get the one way left tool is clicked
    /// </summary>
    public void ButtonClickOneWayLeft()
    {
        currentTool = TileType.OneWayLeft;
    }

    /// <summary>
    /// Called when the button to get the one way left tool is clicked
    /// </summary>
    public void ButtonCLickOneWayRight()
    {
        currentTool = TileType.OneWayRight;
    }

    /// <summary>
    /// Called when the button to get the one way down tool is clicked
    /// </summary>
    public void ButtonClickOneWayDown()
    {
        currentTool = TileType.OneWayDown;
    }

    /// <summary>
    /// Called when the save button is clicked
    /// </summary>
    public void ButtonClickSave()
    {
        GenerateSolidColliders();
        GenerateTriggers();
        GenerateInteractableObjects();

        //temp so i can see what i'm doing
        //foreach (Vector2[] v in paths)
        //{
        //    spriteRenderer.GetComponent<PolygonCollider2D>().pathCount = index + 1;
        //    spriteRenderer.GetComponent<PolygonCollider2D>().SetPath(index, v);
        //    index++;
        //}


        //Store the data we just saved into a file so it can be used in another project
        int pathCount = spriteRenderer.GetComponent<PolygonCollider2D>().pathCount;

        List<Vector2[]> paths = new List<Vector2[]>();
        for (int i = 0; i < pathCount; i++)
        {
            paths.Add(spriteRenderer.GetComponent<PolygonCollider2D>().GetPath(i));
        }

        //At this point. "paths" contains the correct data that we need for the solid colliders
        SerializablePolygon poly = new SerializablePolygon(paths);
        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream("MostRecentSave.data", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            formatter.Serialize(stream, poly);
        }
    }

    /// <summary>
    /// Called when the load button is clicked
    /// </summary>
    public void ButtonClickLoad()
    {
        SerializablePolygon poly;
        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new FileStream("MostRecentSave.data", FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            poly = (SerializablePolygon)formatter.Deserialize(stream);
        }

        int index = 0;
        spriteRenderer.GetComponent<PolygonCollider2D>().pathCount = 0;
        foreach (Vector2[] v in poly.Paths)
        {
            spriteRenderer.GetComponent<PolygonCollider2D>().pathCount = index + 1;
            spriteRenderer.GetComponent<PolygonCollider2D>().SetPath(index, v);
            index++;
        }
    }

    #endregion

    #region private methods

    /// <summary>
    /// Generates the new object and sets it up so that it can be raycasted to be seen as an interactable object.
    /// </summary>
    private void GenerateInteractableObjects()
    {
        //Whenever we do calculations to find groups of polygons, we add them to this list.
        List<TileData> collidersAlreadyTakenCareOf = new List<TileData>();

        //Creates polygons for every cluster of tiles tagged as a Trigger collider
        foreach (TileData t in grid)
        {
            //If the tile data is not already taken care of and 
            if (!collidersAlreadyTakenCareOf.Contains(t) && t.type == TileType.TriggerCollider)
            {
                //Gets all the tiles connected to t
                List<TileData> connectedColliders = GetAllConnectedTileData(t);
                collidersAlreadyTakenCareOf.AddRange(connectedColliders);
                List<Vector2[]> paths = GeneratePolygon2DColliderPath(connectedColliders);
                GameObject newInteractableObj = new GameObject("NewInteractableObj");
                newInteractableObj.transform.SetParent(spriteRenderer.transform);
                newInteractableObj.transform.position = paths[0][0];
                Vector2 temp = new Vector2(paths[0][0].x, paths[0][0].y);
                foreach (Vector2[] v in paths)
                {
                    for (int i = 0; i < v.Length; i++)
                    {
                        v[i] -= temp;
                    }
                }
                newInteractableObj.AddComponent<PolygonCollider2D>();
                newInteractableObj.GetComponent<PolygonCollider2D>().isTrigger = true;

                int index = 0;
                foreach (Vector2[] v in paths)
                {
                    newInteractableObj.GetComponent<PolygonCollider2D>().pathCount = index + 1;
                    newInteractableObj.GetComponent<PolygonCollider2D>().SetPath(index, v);
                    index++;
                }
            //TODO: Eventually we probably need to tag this or put it on a special layer so that raycasting can tell it is an interactable object.
            }
        }
    }

    /// <summary>
    /// Generates the new object and polygon collider for all triggers on this map.
    /// </summary>
    private void GenerateTriggers()
    {
        //Whenever we do calculations to find groups of polygons, we add them to this list.
        List<TileData> collidersAlreadyTakenCareOf = new List<TileData>();

        //Creates polygons for every cluster of tiles tagged as a Trigger collider
        foreach (TileData t in grid)
        {
            //If the tile data is not already taken care of and 
            if (!collidersAlreadyTakenCareOf.Contains(t) && t.type == TileType.TriggerCollider)
            {
                //Gets all the tiles connected to t
                List<TileData> connectedColliders = GetAllConnectedTileData(t);
                collidersAlreadyTakenCareOf.AddRange(connectedColliders);
                List<Vector2[]> paths = GeneratePolygon2DColliderPath(connectedColliders);
                GameObject newTriggerObj = new GameObject("NewTriggerObj");
                newTriggerObj.transform.SetParent(spriteRenderer.transform);
                newTriggerObj.transform.position = paths[0][0];
                Vector2 temp = new Vector2(paths[0][0].x, paths[0][0].y);
                foreach (Vector2[] v in paths)
                {
                    for (int i = 0; i < v.Length; i++)
                    {
                        v[i] -= temp;
                    }
                }
                newTriggerObj.AddComponent<PolygonCollider2D>();
                newTriggerObj.GetComponent<PolygonCollider2D>().isTrigger = true;

                int index = 0;
                foreach (Vector2[] v in paths)
                {
                    newTriggerObj.GetComponent<PolygonCollider2D>().pathCount = index + 1;
                    newTriggerObj.GetComponent<PolygonCollider2D>().SetPath(index, v);
                    index++;
                }
            }
        }
    }

    /// <summary>
    /// Generates the polygon2DCollider around all objects tagged as a collider
    /// </summary>
    private void GenerateSolidColliders()
    {
        //Whenever we do calculations to find groups of polygons, we add them to this list.
        List<TileData> collidersAlreadyTakenCareOf = new List<TileData>();
        int index = 0;

        //Creates polygons for every cluster of tiles tagged as a collider
        foreach (TileData t in grid)
        {
            //If the tile data is not already taken care of and 
            if (!collidersAlreadyTakenCareOf.Contains(t) && t.type == TileType.Collider)
            {
                //Gets all the tiles connected to t
                List<TileData> connectedColliders = GetAllConnectedTileData(t);
                collidersAlreadyTakenCareOf.AddRange(connectedColliders);
                List<Vector2[]> paths = GeneratePolygon2DColliderPath(connectedColliders);
                foreach (Vector2[] v in paths)
                {
                    spriteRenderer.GetComponent<PolygonCollider2D>().pathCount = index + 1;
                    spriteRenderer.GetComponent<PolygonCollider2D>().SetPath(index, v);
                    index++;
                }
            }
        }
    }

    /// <summary>
    /// Creates an array of points that can be used to create a new path on a polygon collider
    /// </summary>
    /// <param name="startTile">The tile that is part of a group we want to create a polygon around</param>
    private List<Vector2[]> GeneratePolygon2DColliderPath(List<TileData> tileDataGroup)
    {
        //Converts the connectedColliders list into a list of vertices
        List<Vector2> vertices = new List<Vector2>();
        foreach (TileData t in tileDataGroup)
        {
            vertices.Add(t.bl);
            vertices.Add(t.br);
            vertices.Add(t.tr);
            vertices.Add(t.tl);
        }

        //Gets rid of any doubles
        vertices = vertices.Distinct().ToList();

        //Removes all vertices that are not on an edge
        vertices = GetPerimeterOfVerticeList(vertices, tileDataGroup[0].type);

        //Sorts the list of verticies counter clockwise so it can convert to a polygon easily.
        List<List<Vector2>> paths = SortPerimeterCounterClockwise(vertices, tileDataGroup[0].type);
        List<Vector2[]> returnVal = new List<Vector2[]>();
        foreach (List<Vector2> l in paths)
        {
            returnVal.Add(l.ToArray());
        }
        return returnVal;
    }

    /// <summary>
    /// Gets a list of tiles connected to the one sent in as a parameter.
    /// </summary>
    /// <param name="startTile">The tile we want to find all the connected tiles to</param>
    /// <returns></returns>
    private List<TileData> GetAllConnectedTileData(TileData startTile)
    {
        //Create the list of connected tiles that will eventually be returned by this method
        List<TileData> connectedTiles = new List<TileData>();

        //Create the searchQueue and enqueue the first tile into it.
        Queue<TileData> searchQueue = new Queue<TileData>();
        searchQueue.Enqueue(startTile);

        //Create storage for the tile we are currently analyzing as well as its index when attempting to access through the grid variable.
        TileData curTile;
        Vector2 i;

        //While there is something in the search queue
        while (searchQueue.Count > 0)
        {
            curTile = searchQueue.Dequeue();
            i = curTile.GetTileWorldPosition();

            //Check the tile to the left if it exists and isn't already added to the queue or connectedTiles list
            if (i.x >= 1 && grid[(int)i.x - 1, (int)i.y].type == startTile.type
                && !searchQueue.Contains(grid[(int)i.x - 1, (int)i.y]) && !connectedTiles.Contains(grid[(int)i.x - 1, (int)i.y]))
            {
                searchQueue.Enqueue(grid[(int)i.x - 1, (int)i.y]);
            }
            //Check the tile to the top if it exists
            if (i.y < gridHeight - 1 && grid[(int)i.x, (int)i.y + 1].type == startTile.type
                && !searchQueue.Contains(grid[(int)i.x, (int)i.y + 1]) && !connectedTiles.Contains(grid[(int)i.x, (int)i.y + 1]))
            {
                searchQueue.Enqueue(grid[(int)i.x, (int)i.y + 1]);
            }
            //Check the tile to the right if it exists
            if (i.x < gridWidth - 1 && grid[(int)i.x + 1, (int)i.y].type == startTile.type
                && !searchQueue.Contains(grid[(int)i.x + 1, (int)i.y]) && !connectedTiles.Contains(grid[(int)i.x + 1, (int)i.y]))
            {
                searchQueue.Enqueue(grid[(int)i.x + 1, (int)i.y]);
            }
            //check the tile to the elft if it exists
            if (i.y >= 1 && grid[(int)i.x, (int)i.y - 1].type == startTile.type
                && !searchQueue.Contains(grid[(int)i.x, (int)i.y - 1]) && !connectedTiles.Contains(grid[(int)i.x, (int)i.y - 1]))
            {
                searchQueue.Enqueue(grid[(int)i.x, (int)i.y - 1]);
            }

            connectedTiles.Add(curTile);
        }
        return connectedTiles;
    }

    /// <summary>
    /// Removes all the unneeded vertices from the parameter and take just the perimeter
    /// </summary>
    /// <param name="vertices">The complete list we want to make smaller</param>
    /// <returns>The perimeter of the parameter</returns>
    private List<Vector2> GetPerimeterOfVerticeList(List<Vector2> vertices, TileType type)
    {
        int neighborCount = 0;
        List<Vector2> perimeter = new List<Vector2>();

        //Check the four tiles touching this vertex
        foreach (Vector2 v in vertices)
        {
            //upper left tile
            if (v.x - 1 >= 0 && v.y <= gridHeight - 1 && grid[(int)v.x - 1, (int)v.y].type == type)
            {
                neighborCount++;
            }
            //upper right
            if (v.x <= gridWidth - 1 && v.y <= gridHeight - 1 && grid[(int)v.x, (int)v.y].type == type)
            {
                neighborCount++;
            }
            //bottom right
            if (v.y - 1 >= 0 && v.x <= gridWidth - 1 && grid[(int)v.x, (int)v.y - 1].type == type)
            {
                neighborCount++;
            }
            //bottom left
            if (v.y - 1 >= 0 && v.x - 1 >= 0 && grid[(int)v.x - 1, (int)v.y - 1].type == type)
            {
                neighborCount++;
            }


            //If we have less than 4 neighbors, it means we are on the edge. 3 is an inner corner, 2 is a side piece, 1 is an outer corner
            if (neighborCount < 4)
            {
                perimeter.Add(v);
            }

            //Reset the neighbor count back to 0 for the next vertex check.
            neighborCount = 0;
        }
        return perimeter;
    }

    /// <summary>
    /// Takes the perimeter and sorts them in a new list such that they go counter clockwise around the shape.
    /// </summary>
    /// <param name="vertices">The perimeter of the polygon that we want to sort into a usable form</param>
    /// <returns>The sorted parameter</returns>
    private List<List<Vector2>> SortPerimeterCounterClockwise(List<Vector2> vertices, TileType type)
    {
        Vector2 prevDirection = new Vector2(1, 0); //initialized to "Right" because based on our system, the first move will always be right
        Vector2 startVertex = vertices[0];
        Vector2 curVertex;
        List<List<Vector2>> sortedPerimeterLists = new List<List<Vector2>>();
        sortedPerimeterLists.Add(new List<Vector2>());
        List<Vector2> neighbors = new List<Vector2>();
        List<Vector2> alreadyHandledVertices = new List<Vector2>();
        int index = 0;

        //Step1: Find the vertex closest to Vector2.zero
        foreach (Vector2 v in vertices)
        {
            if (Vector2.Distance(Vector2.zero, v) < Vector2.Distance(Vector2.zero, startVertex))
            {
                startVertex = v;
            }
        }
        sortedPerimeterLists[index].Add(startVertex);
        alreadyHandledVertices.Add(startVertex);

        //We will ALWAYS go to the right first, this will always be the case
        sortedPerimeterLists[index].Add(startVertex + Vector2.right);
        alreadyHandledVertices.Add(startVertex + Vector2.right);
        curVertex = startVertex + Vector2.right;

        //Step2: Start going in a counter clockwise direction until you get to the end. Follow the rules
        while (alreadyHandledVertices.Count < vertices.Count)
        {
            //If we have wrapped around and found the first vertex
            if (Vector2.Distance(curVertex, startVertex) == 1 && sortedPerimeterLists[index].Count > 2)
            {
                index++;
                sortedPerimeterLists.Add(new List<Vector2>());

                startVertex = vertices.First(t => !alreadyHandledVertices.Contains(t));

                //Step1: Find the vertex closest to Vector2.zero
                foreach (Vector2 v in vertices)
                {
                    if (!alreadyHandledVertices.Contains(v) && Vector2.Distance(Vector2.zero, v) < Vector2.Distance(Vector2.zero, startVertex))
                    {
                        startVertex = v;
                    }
                }
                sortedPerimeterLists[index].Add(startVertex);
                alreadyHandledVertices.Add(startVertex);

                //We will ALWAYS go to the up first, this will always be the case
                sortedPerimeterLists[index].Add(startVertex + Vector2.up);
                alreadyHandledVertices.Add(startVertex + Vector2.up);
                curVertex = startVertex + Vector2.up;
                prevDirection = Vector2.up;
            }

            neighbors.Clear();
            foreach (Vector2 v in vertices)
            {

                //if the sorted list already doesn't contain v and it is 1 unit away from curVertex
                if (!alreadyHandledVertices.Contains(v) && Vector2.Distance(curVertex, v) == 1)
                {
                    neighbors.Add(v);
                }
            }

            //If only one neighbor was found, it is the only option
            if (neighbors.Count == 1)
            {
                if (prevDirection == neighbors[0] - curVertex)
                {
                    sortedPerimeterLists[index].RemoveAt(sortedPerimeterLists[index].Count - 1);
                }
                prevDirection = neighbors[0] - curVertex;
                sortedPerimeterLists[index].Add(neighbors[0]);
                alreadyHandledVertices.Add(neighbors[0]);
                curVertex = neighbors[0];
            }
            //If multiple neighbors were found, we need to find which one follows our counter clockwise rule.
            else if (neighbors.Count > 1)
            {
                //If prev direction was down
                if (prevDirection == Vector2.down)
                {
                    //if neighbors contains a vertex to the left and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.left) && !alreadyHandledVertices.Contains(curVertex + Vector2.left) && grid[(int)curVertex.x - 1, (int)curVertex.y].type != type && grid[(int)curVertex.x - 1, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.left;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.left);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    //If neighbors contians a vertex downwards and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.down) && !alreadyHandledVertices.Contains(curVertex + Vector2.down) && grid[(int)curVertex.x, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.down;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.down);
                        sortedPerimeterLists[index].RemoveAt(sortedPerimeterLists[index].Count - 1);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex to the right
                    else if (neighbors.Contains(curVertex + Vector2.right) && !alreadyHandledVertices.Contains(curVertex + Vector2.right))
                    {
                        prevDirection = Vector2.right;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.right);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
                //If prev direction was right
                else if (prevDirection == Vector2.right)
                {
                    //if neighbors contains a vertex downwards and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.down) && !alreadyHandledVertices.Contains(curVertex + Vector2.down) && grid[(int)curVertex.x - 1, (int)curVertex.y - 1].type != type && grid[(int)curVertex.x, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.down;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.down);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    //If neighbors contians a vertex to the right and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.right) && !alreadyHandledVertices.Contains(curVertex + Vector2.right) && grid[(int)curVertex.x, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.right;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.right);
                        sortedPerimeterLists[index].RemoveAt(sortedPerimeterLists[index].Count - 1);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex upwards
                    else if (neighbors.Contains(curVertex + Vector2.up) && !alreadyHandledVertices.Contains(curVertex + Vector2.up))
                    {
                        prevDirection = Vector2.up;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.up);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
                //If prev direction was up
                else if (prevDirection == Vector2.up)
                {
                    //if neighbors contains a vertex to the right and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.right) && !alreadyHandledVertices.Contains(curVertex + Vector2.right) && grid[(int)curVertex.x, (int)curVertex.y - 1].type != type && grid[(int)curVertex.x, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.right;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.right);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    //If neighbors contians a vertex upwards and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.up) && !alreadyHandledVertices.Contains(curVertex + Vector2.up) && grid[(int)curVertex.x - 1, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.up;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.up);
                        sortedPerimeterLists[index].RemoveAt(sortedPerimeterLists[index].Count - 1);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex to the left
                    else if (neighbors.Contains(curVertex + Vector2.left) && !alreadyHandledVertices.Contains(curVertex + Vector2.left))
                    {
                        prevDirection = Vector2.left;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.left);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
                //If prev direction was left
                else if (prevDirection == Vector2.left)
                {
                    //if neighbors contains a vertex upwards and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.up) && !alreadyHandledVertices.Contains(curVertex + Vector2.up) && grid[(int)curVertex.x, (int)curVertex.y].type != type && grid[(int)curVertex.x - 1, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.up;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.up);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);

                    }
                    //If neighbors contians a vertex to the left and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.left) && !alreadyHandledVertices.Contains(curVertex + Vector2.left) && grid[(int)curVertex.x - 1, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.left;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.left);
                        sortedPerimeterLists[index].RemoveAt(sortedPerimeterLists[index].Count - 1);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex downwards
                    else if (neighbors.Contains(curVertex + Vector2.down) && !alreadyHandledVertices.Contains(curVertex + Vector2.down))
                    {
                        prevDirection = Vector2.down;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.down);
                        sortedPerimeterLists[index].Add(curVertex);
                        alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
            }
            else
            {
                //The only difference between this and the above is that it only checks to make sure the current neighbor is not in the current list as opposed to the overall list
                //This code is called when there is a corner case
                //[]           []
                //  []       []
                
                Debug.Log("Special case");


                neighbors.Clear();
                foreach (Vector2 v in vertices)
                {

                    //if the sorted list already doesn't contain v and it is 1 unit away from curVertex
                    if (!sortedPerimeterLists[index].Contains(v) && Vector2.Distance(curVertex, v) == 1)
                    {
                        neighbors.Add(v);
                    }
                }

                //If prev direction was down
                if (prevDirection == Vector2.down)
                {
                    //if neighbors contains a vertex to the left and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.left) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.left) && grid[(int)curVertex.x - 1, (int)curVertex.y].type != type && grid[(int)curVertex.x - 1, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.left;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.left);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    //If neighbors contians a vertex downwards and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.down) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.down) && grid[(int)curVertex.x, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.down;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.down);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex to the right
                    else if (neighbors.Contains(curVertex + Vector2.right) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.right))
                    {
                        prevDirection = Vector2.right;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.right);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
                //If prev direction was right
                else if (prevDirection == Vector2.right)
                {
                    //if neighbors contains a vertex downwards and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.down) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.down) && grid[(int)curVertex.x - 1, (int)curVertex.y - 1].type != type && grid[(int)curVertex.x, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.down;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.down);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    //If neighbors contians a vertex to the right and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.right) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.right) && grid[(int)curVertex.x, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.right;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.right);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex upwards
                    else if (neighbors.Contains(curVertex + Vector2.up) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.up))
                    {
                        prevDirection = Vector2.up;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.up);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
                //If prev direction was up
                else if (prevDirection == Vector2.up)
                {
                    //if neighbors contains a vertex to the right and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.right) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.right) && grid[(int)curVertex.x, (int)curVertex.y - 1].type != type && grid[(int)curVertex.x, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.right;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.right);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    //If neighbors contians a vertex upwards and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.up) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.up) && grid[(int)curVertex.x - 1, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.up;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.up);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex to the left
                    else if (neighbors.Contains(curVertex + Vector2.left) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.left))
                    {
                        prevDirection = Vector2.left;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.left);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
                //If prev direction was left
                else if (prevDirection == Vector2.left)
                {
                    //if neighbors contains a vertex upwards and the appropriate TileData is not the same type
                    if (neighbors.Contains(curVertex + Vector2.up) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.up) && grid[(int)curVertex.x, (int)curVertex.y].type != type && grid[(int)curVertex.x - 1, (int)curVertex.y].type == type)
                    {
                        prevDirection = Vector2.up;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.up);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);

                    }
                    //If neighbors contians a vertex to the left and the appropriate TileData is the same type
                    else if (neighbors.Contains(curVertex + Vector2.left) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.left) && grid[(int)curVertex.x - 1, (int)curVertex.y - 1].type == type)
                    {
                        prevDirection = Vector2.left;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.left);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    //If there is a vertex downwards
                    else if (neighbors.Contains(curVertex + Vector2.down) && !sortedPerimeterLists[index].Contains(curVertex + Vector2.down))
                    {
                        prevDirection = Vector2.down;
                        curVertex = neighbors.First(t => t == curVertex + Vector2.down);
                        sortedPerimeterLists[index].Add(curVertex);
                        //alreadyHandledVertices.Add(curVertex);
                    }
                    else
                    {
                        Debug.Log("We have an error here, something went wrong.");
                    }
                }
            }
        }

        return sortedPerimeterLists;

    }

    #endregion
}

#region Helper classes and enums

/// <summary>
/// A class that represents the data of a specific tile.
/// </summary>
class TileData
{
    public TileType type = TileType.Normal; //The type of tile this is. Defaults to TileType.Normal
    public Vector2 bl, br, tr, tl; //Verticie information for each of the four corners of this tile. THIS IS IN WORLD SPACE!!! IE: THE BOTTOM LEFT TILE WILL BE (0, 1) (1, 1) (1, 0) (0, 0)!!!

    /// <summary>
    /// Constructor for the tileData class
    /// </summary>
    /// <param name="bl">The bottom left vertex</param>
    /// <param name="br">The bottom right vertex</param>
    /// <param name="tr">The top right vertex</param>
    /// <param name="tl">The top left vertex</param>
    public TileData(Vector2 bl, Vector2 br, Vector2 tr, Vector2 tl)
    {
        this.bl = bl;
        this.br = br;
        this.tr = tr;
        this.tl = tl;
    }

    /// <summary>
    /// The bottom left tile should always be the coordinates on the grid.
    /// </summary>
    /// <returns>Returns the coordinates to access this data through the grid.</returns>
    public Vector2 GetTileWorldPosition()
    {
        return bl;
    }

    public void ChangeTileType(TileType type)
    {
        this.type = type;
    }
}

/// <summary>
/// An enumeration that represents the types of tiles that could be in this game.
/// </summary>
public enum TileType
{
    Normal,            //A traditional walkable space.
    Collider,          //A solid object with no interactions
    TriggerCollider,   //A trigger collider that will do something when the player steps on this object.
    Interactable,      //An interactable collider that will do something when the player is one unit away, facing it, and presses the interact button.
    OneWayUp,          //A collider that prevents the player from going any other direction but UP
    OneWayLeft,        //A collider that prevents the player from going any other direction but LEFT
    OneWayRight,       //A collider that prevents the player from going any other direction but RIGHT
    OneWayDown,        //A collider that prevents the player from going any other direction but DOWN
};

#endregion

[Serializable]
public class SerializablePolygon
{
    List<Vec2Array> paths;

    public SerializablePolygon(List<Vector2[]> paths)
    {
        this.paths = new List<Vec2Array>();

        foreach(Vector2[] path in paths)
        {
            this.paths.Add(new Vec2Array(path));
        }
    }

    public List<Vector2[]> Paths
    {
        get
        {
            List<Vector2[]> cPaths = new List<Vector2[]>();
            foreach (Vec2Array path in paths)
            {
                cPaths.Add(path.ConvertToVector2());
            }
            return cPaths;
        }
    }



    /// <summary>
    /// Internal class to convert a Vector2 so it is serializable
    /// </summary>
    [Serializable]
    class Vec2Array
    {
        public float[] x;
        public float[] y;

        public Vec2Array(Vector2[] arr)
        {
            x = new float[arr.Length];
            y = new float[arr.Length];

            //convert vector2 array into our data members
            for (int i = 0; i < arr.Length; i++)
            {
                x[i] = arr[i].x;
                y[i] = arr[i].y;
            }
        }

        public Vector2[] ConvertToVector2()
        {
            Vector2[] path = new Vector2[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                path[i] = new Vector2(x[i], y[i]);
            }
            return path;
        }
    }
}