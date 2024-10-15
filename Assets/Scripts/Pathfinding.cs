using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Diagnostics;

<<<<<<< Updated upstream
public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;
    
    Grid grid;
=======
public class Pathfinding : MonoBehaviour {

	PathRequestManager requestManager;
	Grid grid;

	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}


	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos, targetPos));
	}

	public Vector3 GetRandom()
	{
		float pos = UnityEngine.Random.Range(0,grid.walkablePos.Count);

		return grid.walkablePos[Mathf.RoundToInt(pos)];
    }
	
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
>>>>>>> Stashed changes

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        Findpath(seeker.position, target.position);
    }
    void Findpath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) { 
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            foreach (Node neighbour in grid.GetNeighbours(currentNode)) { 
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistanceNode(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) { 
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistanceNode(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node starNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node courrentNode = endNode;

        while (courrentNode != starNode) { 
            path.Add(courrentNode);
            courrentNode = courrentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int GetDistanceNode(Node nodeA, Node nodeB) { 
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14* dstX +10 * (dstY - dstX);
    }
}
