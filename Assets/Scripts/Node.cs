using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool transitable;
    public Vector3 worldPos;

    public int gCost;
    public int hCost;

    public Node(bool _transitable, Vector3 _worldPos)
    {
        transitable = _transitable;
        worldPos = _worldPos;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
