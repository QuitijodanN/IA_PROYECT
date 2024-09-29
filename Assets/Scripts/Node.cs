using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool transitable;
    public Vector3 worldPos;

    public Node(bool _transitable, Vector3 _worldPos)
    {
        transitable = _transitable;
        worldPos = _worldPos;
    }
}
