using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private TileManager manager;

    Vector2Int[] Path = new Vector2Int[0];

    public Vector2Int[] FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        if (manager.grid[startPos.x, startPos.y] == null || manager.grid[endPos.x, endPos.y] == null)
        {
            UnityEngine.Debug.LogError("invalid tiles");
            return new Vector2Int[0];
        }

        AStar(startPos, endPos);

        return Path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridPos.x - nodeB.gridPos.x);
        int distY = Mathf.Abs(nodeA.gridPos.y - nodeB.gridPos.y);

        return 10 * (distX + distY);
    }

    private void AStar(Vector2Int startPos, Vector2Int endPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        bool pathSuccess = false;

        Node startNode = manager.grid[startPos.x,startPos.y];
        Node endNode = manager.grid[endPos.x, endPos.y];

        Heap<Node> openSet = new Heap<Node>(manager.maxSize());
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node curentNode = openSet.RemoveFirst();
            closedSet.Add(curentNode);

            if (curentNode == endNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + "ms");

                pathSuccess = true;
                closedSet.Add(endNode);
                break;
            }

            foreach (Node neighbour in manager.GetNeighbours(curentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovCostToNeighbour = curentNode.gCost + GetDistance(curentNode, neighbour);
                if (newMovCostToNeighbour < curentNode.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = curentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }
        }

        if (pathSuccess)
        {
            RetracePath(startNode, endNode);
        }
        else
            Path = new Vector2Int[0];
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> _Path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            _Path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector2Int[] waypoints = SimplifyPath(_Path);

        Array.Reverse(waypoints);

        Path = waypoints;
    }

    private Vector2Int[] SimplifyPath(List<Node> path)
    {
        List<Vector2Int> waypoints = new List<Vector2Int>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridPos.x - path[i].gridPos.x, path[i - 1].gridPos.y - path[i].gridPos.y);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].gridPos);
            }
            //directionOld = directionNew;
        }

        waypoints.Add(path[^1].gridPos);

        return waypoints.ToArray();
    }

}
