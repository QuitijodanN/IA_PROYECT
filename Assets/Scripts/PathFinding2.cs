using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding2 : MonoBehaviour
{
    public Transform seeker, target;

    Grid2 grid;
    void Awake()
    {
        grid = GetComponent<Grid2>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        List<Node> closeSet = new List<Node>();
        openSet.Add(startNode);

      
        while (openSet.Count > 0)
        {
            print("open " + openSet.Count);
            print("close " + closeSet.Count);
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            print(openSet.Remove(currentNode));
            closeSet.Add(currentNode);

            if (currentNode.gridX == targetNode.gridX && currentNode.gridY == targetNode.gridY)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.transitable || closeSet.Contains(neighbour))
                {
                    continue;
                }

                int neighbourNewCost = currentNode.gCost + 1 + GetDistance(currentNode, neighbour);
                if (neighbourNewCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = neighbourNewCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }

        }

    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}
