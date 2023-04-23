using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public bool isWalkable { get; private set; }
    public float hp { get; private set; }
    public float moveCost { get; private set; }

    public Tile(bool _isWalkable, float _moveCost) 
    {
        isWalkable = _isWalkable;
        moveCost = _moveCost;
    }
}
