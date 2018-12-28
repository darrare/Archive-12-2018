using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public void Enqueue(Node node, float priority)
    {
        int index = list.Count;

        //Finds the first index that the priority is less than the curNode in that spot.
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].priority > priority)
            {
                index = i;
                break;
            }
        }
        list.Insert(index, new PriorityQueueNode(node, priority));
    }

    /// <summary>
    /// Dequeues the first item in the list
    /// </summary>
    /// <returns>The value with the lowest priority in the list.</returns>
    public Node Dequeue()
    {
        Node returnVal = list[0].node;
        list.RemoveAt(0);
        return returnVal;
    }

    /// <summary>
    /// Changes the priority
    /// </summary>
    /// <param name="node">The node to change</param>
    /// <param name="priority">The priority to change to</param>
    public void ModifyPriority(Node node, float priority)
    {
        PriorityQueueNode curNode = list.First(t => t.node == node);
        if (list.Remove(curNode))
        {
            Enqueue(node, priority);
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

    /// <summary>
    /// Constructor for a priority queue node
    /// </summary>
    /// <param name="node">The node</param>
    /// <param name="priority">The value so we can order the queue</param>
    public PriorityQueueNode(Node node, float priority)
    {
        this.node = node;
        this.priority = priority;
    }
}
