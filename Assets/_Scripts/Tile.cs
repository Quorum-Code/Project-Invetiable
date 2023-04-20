using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType { None }
    public Pawn tilePawn { get; private set; }

    // NEEDS TO BE IMPLEMENTED
    //public TileObject tileObject;

    public bool isPassable { get; private set; }
    public float moveMultiplier { get; private set; }
    public TileType tileType { get; private set; }

    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }

    public void InitializeTile(bool _isPassable, float _moveMultiplier, Vector3 coords, TileType _tileType)
    {
        isPassable = _isPassable;
        moveMultiplier = _moveMultiplier;

        x = (int)coords.x;
        y = (int)coords.y;
        z = (int)coords.z;

        tileType = _tileType;
    }

    public void setCoords(Vector3 coords) 
    {
        x = (int) coords.x;
        y = (int) coords.y;
        z = (int) coords.z;
    }
}
