using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// All of the different behaviours the ghosts may have
/// </summary>
public enum GhostBehaviourState
{
    IdleInBase, Chase, Run, Frightened, FrightenedFading, ReturnToBase, 
}

/// <summary>
/// A monobehaviour to be attached to each of the ghosts
/// </summary>
public class GhostScript : PausableObjectScript
{
    #region fields

    delegate void Behaviour();
    Behaviour chaseBehaviour;
    Direction curDirection = Direction.Left;
    GhostBehaviourState curState;
    bool hasChangedRecently = false;
    Animator anim;

    //Pathfinding fields
    Stack<Node> path;
    float travelTimer = 0;

    /// <summary>
    /// The state of the ghost
    /// </summary>
    public GhostBehaviourState CurState
    {
        get { return curState; }
        set
        {
            curState = value;
            HasChangedRecently = true;
        }
    }

    /// <summary>
    /// Used to control whether or not the ghost switches directions when it switches states
    /// </summary>
    public bool HasChangedRecently
    {
        get
        {
            if (hasChangedRecently)
            {
                bool temp = hasChangedRecently;
                hasChangedRecently = !hasChangedRecently;
                return temp;
            }
            return false;
        }
        set { hasChangedRecently = value; }
    }

    /// <summary>
    /// The node the ghost is currently on
    /// </summary>
    public Node CurNode
    { get; set; }

    /// <summary>
    /// The node the ghost was most recently on
    /// </summary>
    public Node PrevNode
    { get; set; }

    /// <summary>
    /// The end goal that this ghost is trying to reach
    /// </summary>
    public Node TargetNode
    { get; set; }

    /// <summary>
    /// The node that the ghost is currently moving to
    /// </summary>
    public Node ImmediateTargetNode
    { get; set; }

    /// <summary>
    /// What ghost this script is attached too
    /// </summary>
    public Ghost WhatGhostIsThis
    { get; set; }


    #endregion

    /// <summary>
    /// Initializes the ghost
    /// </summary>
    /// <param name="position">"The position we are going to spawn at"</param>
    public void Initialize (Vector2 position)
    {
        anim = GetComponent<Animator>();
        CurNode = LevelManager.Instance.Graph.FindNearestNodeToPoint(new Vector2(position.x + .5f, position.y + .5f));
        PrevNode = CurNode;
        ImmediateTargetNode = CurNode;

        path = new Stack<Node>();

        //Snap the player to the grid.
        transform.position = CurNode.GetWorldPosition();

        //Set curBehaviour equal to the right method here.
        if (gameObject.name.Contains("Blinky"))
        {
            chaseBehaviour = new Behaviour(BlinkyChaseBehaviour);
            WhatGhostIsThis = Ghost.Blinky;
        }
        else if (gameObject.name.Contains("Pinky"))
        {
            chaseBehaviour = new Behaviour(PinkyChaseBehaviour);
            WhatGhostIsThis = Ghost.Pinky;
        }
        else if (gameObject.name.Contains("Clyde"))
        {
            chaseBehaviour = new Behaviour(ClydeChaseBehaviour);
            WhatGhostIsThis = Ghost.Clyde;
        }
        else if (gameObject.name.Contains("Inky"))
        {
            chaseBehaviour = new Behaviour(InkyChaseBehaviour);
            WhatGhostIsThis = Ghost.Inky;
        }

        switch (CurState)
        {
            case GhostBehaviourState.IdleInBase:
                IdleInBaseBehaviour();
                break;
            case GhostBehaviourState.Chase:
                chaseBehaviour();
                break;
            case GhostBehaviourState.Run:
                ScatterBehaviour();
                break;
            case GhostBehaviourState.Frightened:
                FrightenedBehaviour();
                break;
            case GhostBehaviourState.FrightenedFading:
                FrightenedBehaviour();
                break;
            case GhostBehaviourState.ReturnToBase:
                ReturnToBaseBehaviour();
                break;
        }
        if (path.Count > 0)
        {
            ImmediateTargetNode = path.Pop();
        }
        
    }

    /// <summary>
    /// Updates the ghost
    /// </summary>
    protected override void NotPausedUpdate()
    {
        base.NotPausedUpdate();
        travelTimer += Time.deltaTime;
        if (ImmediateTargetNode != null && ImmediateTargetNode.Type == NodeType.Teleporter && CurNode.Type == NodeType.Teleporter)
        {
            transform.position = ImmediateTargetNode.GetWorldPosition();
            travelTimer = Constants.GHOST_MOVEMENT_SPEEDS[WhatGhostIsThis][CurState];
        }
        else if (ImmediateTargetNode != null)
        { 
            transform.position = Vector2.Lerp(CurNode.GetWorldPosition(), ImmediateTargetNode.GetWorldPosition(), travelTimer * (1 / Constants.GHOST_MOVEMENT_SPEEDS[WhatGhostIsThis][CurState]));
        }

        //Once we have reached our target, cycle our targets and reset the timer.
        if (travelTimer >= Constants.GHOST_MOVEMENT_SPEEDS[WhatGhostIsThis][CurState])
        {
            PrevNode = CurNode;
            CurNode = ImmediateTargetNode;
            switch (CurState)
            {
                case GhostBehaviourState.IdleInBase:
                    IdleInBaseBehaviour();
                    break;
                case GhostBehaviourState.Chase:
                    chaseBehaviour();
                    break;
                case GhostBehaviourState.Run:
                    ScatterBehaviour();
                    break;
                case GhostBehaviourState.Frightened:
                    FrightenedBehaviour();
                    break;
                case GhostBehaviourState.FrightenedFading:
                    FrightenedBehaviour();
                    break;
                case GhostBehaviourState.ReturnToBase:
                    ReturnToBaseBehaviour();
                    break;
            }
            if (path.Count > 0)
            {
                ImmediateTargetNode = path.Pop();
            }
            travelTimer = 0;

            //We will eventually change sprites here so they are looking in the direction they are going
            if (ImmediateTargetNode != null)
            {
                //If we are going right
                if (CurNode.GetWorldPosition().x < ImmediateTargetNode.GetWorldPosition().x)
                {
                    curDirection = Direction.Right;
                }
                //if we are going left
                else if (CurNode.GetWorldPosition().x > ImmediateTargetNode.GetWorldPosition().x)
                {
                    curDirection = Direction.Left;
                }
                //if we are going up
                else if (CurNode.GetWorldPosition().y < ImmediateTargetNode.GetWorldPosition().y)
                {
                    curDirection = Direction.Up;
                }
                //if we are going down
                else
                {
                    curDirection = Direction.Down;
                }
            }
            //transform.eulerAngles = new Vector3(0, 0, Constants.PACMAN_ROTATION[curDirection]);
        }

        //Handle animation stuff here.
        anim.SetInteger("Direction", (int)curDirection);
        anim.SetInteger("Mode", (int)CurState);
    }

    /// <summary>
    /// Called whenever our trigger collider is triggered
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((CurState == GhostBehaviourState.Frightened || CurState == GhostBehaviourState.FrightenedFading) && collider.gameObject == GameManager.Instance.PacMan.gameObject)
        {
            CurState = GhostBehaviourState.ReturnToBase;
            //TODO: do the points adding thing and shortly pause the game.
        }
        else if (CurState != GhostBehaviourState.ReturnToBase && collider.gameObject == GameManager.Instance.PacMan.gameObject)
        {
            //Kill pacman
        }
    }

    /// <summary>
    /// The behaviour when you want the ghost to idle in base for a short period.
    /// </summary>
    void IdleInBaseBehaviour()
    {

    }

    /// <summary>
    /// the behaviour when the ghost is blue and scared of pacman.
    /// </summary>
    void FrightenedBehaviour()
    {
        //only path if we are at an intersection or if they don't already have a path.
        if (CurNode.Neighbors.Where(t => t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor).ToList().Count > 2 || path.Count < 1)
        {
            TargetNode = LevelManager.Instance.Graph.GetPseudoRandomNodeNearLocation(CurNode.GetWorldPosition());
            if (TargetNode == CurNode)
            {
                TargetNode = CurNode.Neighbors.First(t => (t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor) && t.Value != PrevNode).Value;
            }
            path = AStarAlgorithm(CurNode, PrevNode, TargetNode, HasChangedRecently);
        }
    }

    /// <summary>
    /// Ghosts behaviour when they are scattering.
    /// </summary>
    void ScatterBehaviour()
    {
        //only path if we are at an intersection or if they don't already have a path.
        if (CurNode.Neighbors.Where(t => t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor).ToList().Count > 2 || path.Count < 1)
        {
            TargetNode = LevelManager.Instance.Graph.FindNearestNodeToPoint(Constants.GHOST_HOME_LOCATIONS[WhatGhostIsThis]);
            if (TargetNode == CurNode)
            {
                TargetNode = CurNode.Neighbors.First(t => (t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor) && t.Value != PrevNode).Value;
            }
            path = AStarAlgorithm(CurNode, PrevNode, TargetNode, HasChangedRecently);
        }
    }

    /// <summary>
    /// The behaviour when you want the ghost to return to the base (when eaten)
    /// </summary>
    void ReturnToBaseBehaviour()
    {
            if (Vector2.Distance(CurNode.GetWorldPosition(), Vector2.zero) < 2)
            {
                CurState = GhostBehaviourState.Chase;
            }
            else
            {
                TargetNode = LevelManager.Instance.Graph.FindNearestNodeToPoint(Vector2.zero);
                path = AStarAlgorithm(CurNode, PrevNode, TargetNode, HasChangedRecently);
            }
    }

    /// <summary>
    /// Blinkys behaviour when he is chasing pacman
    /// </summary>
    void BlinkyChaseBehaviour()
    {
        //only path if we are at an intersection or if they don't already have a path.
        if (CurNode.Neighbors.Where(t => t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor).ToList().Count > 2 || path.Count < 1)
        {
            TargetNode = GameManager.Instance.PacMan.CurNode;
            path = AStarAlgorithm(CurNode, PrevNode, TargetNode, HasChangedRecently);
        }
    }

    /// <summary>
    /// Pinkys behaviour when he is chasing pacman
    /// </summary>
    void PinkyChaseBehaviour()
    {
        //only path if we are at an intersection or if they don't already have a path.
        if (CurNode.Neighbors.Where(t => t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor).ToList().Count > 2 || path.Count < 1)
        {
            TargetNode = LevelManager.Instance.Graph.FindNearestNodeToPoint(GameManager.Instance.PacMan.CurNode.GetWorldPosition() + Constants.PINKY_BEHAVIOUR_DIRECTION[GameManager.Instance.PacMan.CurDirection] * Constants.PINKY_DISTANCE_OFFSET);
            if (TargetNode == CurNode)
            {
                TargetNode = CurNode.Neighbors.First(t => (t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor) && t.Value != PrevNode).Value;
            }
            path = AStarAlgorithm(CurNode, PrevNode, TargetNode, HasChangedRecently);
        }
    }

    /// <summary>
    /// Clydes behaviour when he is chasing pacman
    /// </summary>
    void ClydeChaseBehaviour()
    {
        //only path if we are at an intersection or if they don't already have a path.
        if (CurNode.Neighbors.Where(t => t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor).ToList().Count > 2 || path.Count < 1)
        {
            if (Vector2.Distance(CurNode.GetWorldPosition(), GameManager.Instance.PacMan.CurNode.GetWorldPosition()) >= 8)
            {
                //Do blinky's pathfinding
                TargetNode = GameManager.Instance.PacMan.CurNode;
                path = AStarAlgorithm(CurNode, PrevNode, TargetNode, HasChangedRecently);
            }
            else
            {
                ScatterBehaviour();
            }

        }
    }

    /// <summary>
    /// Inkys behaviour when he is chasing pacman
    /// </summary>
    void InkyChaseBehaviour()
    {
        //only path if we are at an intersection or if they don't already have a path.
        if (CurNode.Neighbors.Where(t => t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor).ToList().Count > 2 || path.Count < 1)
        {
            TargetNode = LevelManager.Instance.Graph.FindNearestNodeToPoint(GameManager.Instance.PacMan.CurNode.GetWorldPosition() + Constants.INKY_BEHAVIOUR_DIRECTION[GameManager.Instance.PacMan.CurDirection] * Constants.INKY_DISTANCE_OFFSET);
            Vector2 blinkyDir = TargetNode.GetWorldPosition() - LevelManager.Instance.Ghosts[Ghost.Blinky].CurNode.GetWorldPosition();
            TargetNode = LevelManager.Instance.Graph.FindNearestNodeToPoint(TargetNode.GetWorldPosition() + blinkyDir);
            if (TargetNode == CurNode)
            {
                TargetNode = CurNode.Neighbors.First(t => (t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor) && t.Value != PrevNode).Value;
            }
            path = AStarAlgorithm(CurNode, PrevNode, TargetNode, HasChangedRecently);
        }
    }

    /// <summary>
    /// An A* algorithm used to find the ghosts path to their target.
    /// </summary>
    /// <param name="start">The node we are starting at</param>
    /// <param name="prevNode">The most recent node we were at (so we can't go backwards)</param>
    /// <param name="target">The node we want to find a path to</param>
    Stack<Node> AStarAlgorithm(Node start, Node prevNode, Node target, bool isTurningAround = false)
    {
        List<PriorityQueueNode> searched = new List<PriorityQueueNode>();
        PriorityQueue queue = new PriorityQueue();
        Stack<Node> path = new Stack<Node>();
        PriorityQueueNode curNode;

        //set our first node to start the search, and add this node to our searched list
        if (!isTurningAround)
        {
            queue.Enqueue(new PriorityQueueNode(start, 0, 0));
        }
        else
        {
            queue.Enqueue(new PriorityQueueNode(prevNode, 0, 0));
        }
        
        
        //If our target is currently on our previous node, set our target to one of the neighbors of our previous node that isn't our current node.
        if (target == prevNode)
        {
            target = prevNode.Neighbors.First(t => t.Value != CurNode && (t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor)).Value;
        }

        //Add the previous node to our searched list so we can't go backwards
        if (!isTurningAround)
        {
            searched.Add(new PriorityQueueNode(prevNode, 0, 0));
        }
        else
        {
            searched.Add(new PriorityQueueNode(start, 0, 0));
        }

        while(queue.Count > 0)
        {
            curNode = queue.Dequeue();
            GameManager.Instance.DrawCross(curNode.node.GetWorldPosition(), .3f, Color.blue, Time.deltaTime * 10);
            

            //If we have found the target node
            if (curNode.node == target)
            {
                path.Push(curNode.node);

                //Loop back through the backnodes to find the fastest path
                while(curNode.backNode != null)
                {
                    curNode = curNode.backNode;
                    path.Push(curNode.node);
                    GameManager.Instance.DrawCross(curNode.node.GetWorldPosition(), .3f, Color.green, Time.deltaTime * 10);
                }
                if (!isTurningAround)
                {
                    path.Pop(); //remove the first node from the list, because it is the node we are currently standing on
                }
                return path;
            }

            //Look through each neighbor to find who we have not checked yet.
            foreach (KeyValuePair<Direction, Node> node in curNode.node.Neighbors.Where(t => t.Value.Type == NodeType.Available || t.Value.Type == NodeType.Teleporter || t.Value.Type == NodeType.GhostDoor))
            {
                float priority = curNode.length + 1 + Vector2.Distance(node.Value.GetWorldPosition(), TargetNode.GetWorldPosition());

                //If the node has not already been searched, give it a priority and add it to the queue
                if (searched.FirstOrDefault(t => t.node == node.Value) == null)
                {
                    PriorityQueueNode n = new PriorityQueueNode(node.Value, priority, curNode.length + 1, curNode);
                    queue.Enqueue(n);
                    searched.Add(n);
                    GameManager.Instance.DrawCross(node.Value.GetWorldPosition(), .3f, Color.red, Time.deltaTime * 10);
                }
                //else if the node has been searched already, but it has a higher length than what it could be.
                else if (searched.FirstOrDefault(t => t.node == node.Value).length > curNode.length + 1)
                {
                    queue.ModifyPriority(searched.FirstOrDefault(t => t.node == node.Value), priority);
                    searched.FirstOrDefault(t => t.node == node.Value).length = curNode.length + 1;
                    searched.FirstOrDefault(t => t.node == node.Value).backNode = curNode;
                }
            }
        }

        //For some reason, a path could not be found
        Debug.Log("For some reason, a path could not be found.");



        return path;
    }

    /// <summary>
    /// Priority queue class
    /// </summary>
    class PriorityQueue
    {
        List<PriorityQueueNode> list = new List<PriorityQueueNode>();

        /// <summary>
        /// Count of the priority queue
        /// </summary>
        public int Count
        { get { return list.Count; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public PriorityQueue() { }

        /// <summary>
        /// Enqueues the node into the list in the right order
        /// </summary>
        /// <param name="node">Node to insert</param>
        /// <param name="priority">Priority of the node, calculated by A* aglorithm, lower is better</param>
        public void Enqueue(PriorityQueueNode node)
        {
            int index = list.Count;

            //Finds the first index that the priority is less than the curNode in that spot.
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].priority > node.priority)
                {
                    index = i;
                    break;
                }
            }
            list.Insert(index, node);
        }

        /// <summary>
        /// Dequeues the first item in the list
        /// </summary>
        /// <returns>The value with the lowest priority in the list.</returns>
        public PriorityQueueNode Dequeue()
        {
            PriorityQueueNode returnVal = list[0];
            list.RemoveAt(0);
            return returnVal;
        }

        /// <summary>
        /// Changes the priority
        /// </summary>
        /// <param name="node">The node to change</param>
        /// <param name="priority">The priority to change to</param>
        public void ModifyPriority(PriorityQueueNode node, float priority)
        {
            if (list.Remove(node))
            {
                node.priority = priority;
                Enqueue(node);
            }
        }
    }

    /// <summary>
    /// Priorty queue node class that we use in our priority queue.
    /// </summary>
    class PriorityQueueNode
    {
        public Node node;
        public float priority;
        public float length;
        public PriorityQueueNode backNode;

        /// <summary>
        /// Constructor for a priority queue node
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="priority">The value so we can order the queue</param>
        /// <param name="length">The length from the start of the search</param>
        public PriorityQueueNode(Node node, float priority, float length, PriorityQueueNode backNode = null)
        {
            this.node = node;
            this.priority = priority;
            this.length = length;
            this.backNode = backNode;
        }
    }
}
