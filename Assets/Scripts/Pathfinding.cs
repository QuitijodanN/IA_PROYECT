
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    PathFindingGrid grid;
    void Awake()
    {
        grid = GetComponent<PathFindingGrid>();
    }

    void Start()
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

        int prueba = 0;
        while (openSet.Count > 0 && prueba < 10000)
        {
            prueba++; if (prueba >= 1000) { print("sigue roto"); }
            print("open " +openSet.Count);
            print("close "+closeSet.Count);
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

            if(currentNode.gridX == targetNode.gridX && currentNode.gridY == targetNode.gridY)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.transitable || isInClosed(neighbour))
                {
                    continue;
                }

                int neighbourNewCost = currentNode.gCost + 1 + GetDistance(currentNode, neighbour);
                if (neighbourNewCost < neighbour.gCost || !isInOpen(neighbour))
                {
                    neighbour.gCost = neighbourNewCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!isInOpen(neighbour) && !isInClosed(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }

        }

        bool isInOpen(Node n)
        {
            for(int i=0; i<openSet.Count; i++)
            {
                if(openSet[i].gridX == n.gridX && openSet[i].gridY == n.gridY)
                {
                    return true;
                }
            }
            return false;
        }

        bool isInClosed(Node n)
        {
            for (int i = 0; i < closeSet.Count; i++)
            {
                if (closeSet[i].gridX == n.gridX && closeSet[i].gridY == n.gridY)
                {
                    return true;
                }
            }
            return false;
        }
        void RetracePath (Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            grid.path = path;
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}

