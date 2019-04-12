using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Unit stats
    public float health = 100;
    public float attackPower = 15;
    public string owner;
    //public Units unitType; Units enum needs to be created and incorporated into the rest of the code
    public int moveSpeed = 2;

    // Unit location & pathing
    public int tileX;
    public int tileY;
    public List<Node> currentPath = null;
    
    // track if unit has attacked or moved during turn
    public bool turnUsed = false;


    public Tile tile { get; protected set; }
    public Directions dir;

    public void Place(Tile target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        // Link unit and tile references
        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    public void Match()
    {
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }
}
