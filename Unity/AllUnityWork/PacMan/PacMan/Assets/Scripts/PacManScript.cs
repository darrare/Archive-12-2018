using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PacManScript : PausableObjectScript
{
    Direction mostRecentInput = Direction.Left;
    Node targetNode, secondTargetNode;
    float travelTimer = 0;

    /// <summary>
    /// Pacmans current node.
    /// </summary>
    public Node CurNode
    { get; set; }

    /// <summary>
    /// The direction pacman is facing. 
    /// </summary>
    public Direction CurDirection
    { get; set; }

    /// <summary>
    /// Initializes pacman
    /// </summary>
    /// <param name="position">The position we want to initialize at</param>
	public void Initialize(Vector2 position)
    {
        CurNode = LevelManager.Instance.Graph.FindNearestNodeToPoint(new Vector2(position.x + .5f, position.y + .5f));
        targetNode = CurNode.Neighbors[Direction.Left];

        //Snap the player to the grid.
        transform.position = CurNode.GetWorldPosition();
	}

    #region protected methods

    // Update is called once per frame
    protected override void NotPausedUpdate()
    {
        HandleInput();
        HandleAutomaticMovement();
        base.NotPausedUpdate();
    }

    #endregion

    #region private methods

    /// <summary>
    /// Stores all of the users input so that we can use it to control the game.
    /// </summary>
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            mostRecentInput = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            mostRecentInput = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            mostRecentInput = Direction.Down;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            mostRecentInput = Direction.Right;
        }
    }

    /// <summary>
    /// Controls the player character so that it moves correctly in coorespondence to the input the user has last inputed.
    /// </summary>
    void HandleAutomaticMovement()
    {
        //if I am moving towards a target, and that target has a path in mostRecentInput direction, set that direction as the target after we reach our current target
        if (targetNode != null && LevelManager.Instance.CanWeGoInThisDirection(targetNode, mostRecentInput))
        {
            secondTargetNode = targetNode.Neighbors[mostRecentInput];
        }
        //if I am moving towards a target, and that target DOES NOT have a path in mostRecentInput direction, continue going the way we were.
        else if (targetNode != null && LevelManager.Instance.CanWeGoInThisDirection(targetNode, CurDirection))
        {
            secondTargetNode = targetNode.Neighbors[CurDirection];
        }
        //This should only ever run when they are leaving a point where they weren't moving
        else if (targetNode == null && LevelManager.Instance.CanWeGoInThisDirection(CurNode, mostRecentInput))
        {
            targetNode = CurNode.Neighbors[mostRecentInput];
            transform.eulerAngles = new Vector3(0, 0, Constants.PACMAN_ROTATION[mostRecentInput]);
        }
        //The moment they impact with a wall and stop.
        else if (targetNode == null)
        {
            AudioManager.Instance.WakaToggle = true;
        }

        //If we have a target, move towards it
        if (targetNode != null && targetNode.Type == NodeType.Teleporter && CurNode.Type == NodeType.Teleporter)
        {
            transform.position = targetNode.GetWorldPosition();
            travelTimer = Constants.TIME_TO_TRAVEL_BETWEEN_NODES;
        }
        else if (targetNode != null)
        {
            travelTimer += Time.deltaTime;
            transform.position = Vector2.Lerp(CurNode.GetWorldPosition(), targetNode.GetWorldPosition(), travelTimer * (1 / Constants.TIME_TO_TRAVEL_BETWEEN_NODES));
        }

        
        //Once we have reached our target, cycle our targets and reset the timer.
        if (travelTimer >= Constants.TIME_TO_TRAVEL_BETWEEN_NODES)
        {
            CurNode = targetNode;
            targetNode = secondTargetNode;
            secondTargetNode = null;
            travelTimer = 0;

            LevelManager.Instance.EatDot(CurNode.GetWorldPosition());

            if (targetNode != null)
            {
                //If we are going right
                if (CurNode.GetWorldPosition().x < targetNode.GetWorldPosition().x)
                {
                    CurDirection = Direction.Right;
                }
                //if we are going left
                else if (CurNode.GetWorldPosition().x > targetNode.GetWorldPosition().x)
                {
                    CurDirection = Direction.Left;
                }
                //if we are going up
                else if (CurNode.GetWorldPosition().y < targetNode.GetWorldPosition().y)
                {
                    CurDirection = Direction.Up;
                }
                //if we are going down
                else
                {
                    CurDirection = Direction.Down;
                }
            }
            transform.eulerAngles = new Vector3(0, 0, Constants.PACMAN_ROTATION[CurDirection]);
        }
    }

    #endregion
}
