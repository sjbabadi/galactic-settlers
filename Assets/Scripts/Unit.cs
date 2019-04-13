using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Unit stats
    public float health = 100;
    public float attackPower = 15;
    public Turn owner;
    //public Units unitType; Units enum needs to be created and incorporated into the rest of the code
    public int moveSpeed = 2;

    // Unit location & pathing
    public int tileX;
    public int tileY;
    public List<Node> currentPath = null;
    
    // track if unit has attacked or moved during turn
    public bool turnUsed = false;
}
