using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Agent : MonoBehaviour
{
    #region fields

    float movementTimer = 0;

    #endregion

    #region properties

    /// <summary>
    /// The current gridcell that this agent is on.
    /// </summary>
    public GridCell CurCell
    { get; set; }

    /// <summary>
    /// The gridcell the agent is moving towards
    /// </summary>
    public GridCell TargetCell
    { get; set; }

    #endregion

    /// <summary>
    /// Initializes the agent
    /// </summary>
    void Start ()
    {
        CurCell = GameManager.Instance.Graph.GetRandomAvailableCell();
        CurCell.IsOccupied = true;
        TargetCell = CurCell.GetRandomAvailableNeighbor();
        if (TargetCell != null)
        {
            TargetCell.IsTargeted = true;
        }
    }
	
	/// <summary>
    /// Updates the agent
    /// </summary>
	void FixedUpdate ()
    {
        if (TargetCell != null)
        {
            movementTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(CurCell.Position, TargetCell.Position, movementTimer / Constants.AGENT_VELOCITY_MAGNITUDE);

            if (movementTimer >= Constants.AGENT_VELOCITY_MAGNITUDE)
            {
                movementTimer = 0;
                CurCell.IsOccupied = false;
                CurCell = TargetCell;
                CurCell.IsOccupied = true;
                TargetCell.IsTargeted = false;
                TargetCell = CurCell.GetRandomAvailableNeighbor();
                if (TargetCell != null)
                {
                    TargetCell.IsTargeted = true;
                }
            }
        }
        else
        {
            TargetCell = CurCell.GetRandomAvailableNeighbor();
            if (TargetCell != null)
            {
                TargetCell.IsTargeted = true;
            }
        }
	}
}
