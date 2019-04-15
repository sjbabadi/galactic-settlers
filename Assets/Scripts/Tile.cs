using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool movementSelect = false;
    public bool unitGen = false;
    public int movementCost;
    public bool empty = true;
    public GameObject occupant;

    public List<Tile> adjacencyList = new List<Tile>();


    //Needed for BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

   


    //keep in newest version
    private GameState gs;

    

    private void Start()
    {
        //keep in newest version
        gs = FindObjectOfType<GameState>();
    }


    void Update()
    {
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (movementSelect)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (unitGen)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

        }
        
    }


    public void Reset()
    {
        adjacencyList.Clear();

       // Debug.Log(GetComponent<Renderer>().material.GetColor("_Color"));

        current = false;
        target = false;
        selectable = false;
        movementSelect = false;
        unitGen = false;

        visited = false;
        parent = null;
        distance = 0;
    }


    public void FindNeighbors()
    {
        Reset();

        CheckTile(Vector2.up);
        CheckTile(Vector2.down);
        CheckTile(Vector2.right);
        CheckTile(Vector2.left);

    }


    public void CheckTile(Vector2 direction)
    {
        Vector2 halfExtents = new Vector2(0.25f, 1 / 2.0f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y) + direction, halfExtents, 0);

        foreach (Collider2D item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable && tile.empty)
            {
                adjacencyList.Add(tile);
               // Debug.Log("Tile: " + tile + " added with coords: " + tile.GetComponent<Tile>().transform.position.x + ", " + tile.GetComponent<Tile>().transform.position.y);
            }
        }
    }
}
