using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn
{
    string pawnName;

    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }

    public int speed { get; private set; }

    public Pawn(int _x, int _y, int _z, int _speed) 
    {
        x = _x;
        y = _y;
        z = _z;

        speed = _speed;
    }

    public void movePawn(int _x, int _y, int _z) 
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public void setPosition(int[] xz) 
    {
        if (xz.Length == 2) 
        {
            x = xz[0];
            z = xz[1];
        }   
    }
}
