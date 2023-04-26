using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return -compare;
    }

    public Node(bool _walkable, Vector2Int _gridPos)
    {
        walkable = _walkable;
        gridPos = _gridPos;
    }

    public Node(bool _walkable, int _gridPosX, int _gridPosY)
    {
        walkable = _walkable;
        gridPos = new Vector2Int(_gridPosX, _gridPosY);
    }

    public int heapIndex;

    public bool walkable;
    public Vector2Int gridPos;

    public Node parent;

    public int gCost = 0;
    public int hCost = 0;

    public int fCost
    {
        get 
        { 
            return gCost + hCost;
        }
    }
}
