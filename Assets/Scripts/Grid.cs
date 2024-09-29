using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask wallsMask;
    public Vector2 gridWorldSize;
    public float radioNode;
    Node[,] grid;

    float diametroNode;
    int gridSizex;
    int gridSizey;
    
    void Start()
    {
        diametroNode = radioNode * 2;
        gridSizex = Mathf.RoundToInt(gridWorldSize.x / diametroNode);
        gridSizey = Mathf.RoundToInt(gridWorldSize.y / diametroNode);
        CrearGrid();
    }
    void CrearGrid()
    {
        grid = new Node[gridSizex, gridSizey];
        Vector3 worldBotLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
        for (int x = 0; x < gridSizex; x++)
        {
            for (int y = 0; y < gridSizey; y++)
            {
                Vector3 worldPoint = worldBotLeft + Vector3.right * (x * diametroNode + radioNode) + Vector3.forward * (y * diametroNode + radioNode);
                bool walkable = !(Physics.CheckSphere(worldPoint, radioNode, wallsMask));
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.transitable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (diametroNode-.1f));
            }
        }
    }
}
