using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingGrid : MonoBehaviour
{
    public Transform player;
    public LayerMask wallsMask;
    public Vector2 gridWorldSize;
    public float radioNode;
    private Node[,] grid;

    float diametroNode;
    int gridSizex;
    int gridSizey;
    public Node[,] getGrid()
    {
        if (gridGenerated)
        {
            return grid;
        }
        CrearGrid();
        return grid;
    }
    public bool gridGenerated = false;
    void Start()
    {
        
        CrearGrid();
    }
    void CrearGrid()
    {
        if (gridGenerated)
        {
            return;
        }
        gridGenerated = true;
        diametroNode = radioNode * 2;
        gridSizex = Mathf.RoundToInt(gridWorldSize.x / diametroNode);
        gridSizey = Mathf.RoundToInt(gridWorldSize.y / diametroNode);
        grid = new Node[gridSizex, gridSizey];
        Vector3 worldBotLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
        for (int x = 0; x < gridSizex; x++)
        {
            for (int y = 0; y < gridSizey; y++)
            {
                Vector3 worldPoint = worldBotLeft + Vector3.right * (x * diametroNode + radioNode) + Vector3.forward * (y * diametroNode + radioNode);
                bool walkable = !(Physics.CheckSphere(worldPoint, radioNode, wallsMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        CrearGrid();

        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizex - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizey - 1) * percentY);
        return grid[x, y];
    }

    
    public List<Node> GetNeighbours(Node node)
    {
        CrearGrid();
        List<Node> neighbours = new List<Node>();
        for (int x=-1; x <=1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizex && checkY >= 0 && checkY < gridSizey)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            foreach(Node n in grid)
            {
              
                Gizmos.color = (n.transitable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                if (playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (diametroNode-.1f));
            }
        }
    }
}
