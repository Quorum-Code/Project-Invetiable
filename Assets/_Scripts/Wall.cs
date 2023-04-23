using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall
{
    public bool isPassable { get; private set; }
    public float hp { get; private set; }

    public Wall(bool _isPassable, float _hp) 
    {
        hp = _hp;
    }

    public void damage(float damage) 
    {
        hp -= damage;
    }
}
