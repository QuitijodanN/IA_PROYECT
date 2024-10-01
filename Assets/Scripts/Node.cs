using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool transitable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool _transitable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        transitable = _transitable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
