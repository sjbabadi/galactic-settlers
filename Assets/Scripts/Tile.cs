using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public int movementCost;

    //Color ground = new Color(0.6509434f, 0.3101193f, 0.05490196f, 1);

    public List<Tile> adjacencyList = new List<Tile>();


    //Needed for BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

   


    //keep in newest version
    private GameState gs;

    //delete in newest version
    [SerializeField] private bool empty = true;
    [SerializeField] private Unit[] playerUnits;
    [SerializeField] private Building[] playerBuildings;
    [SerializeField] private Unit[] enemyUnits;
    [SerializeField] private Building[] enemyBuildings;

    private void Start()
    {
        //keep in newest version
        gs = FindObjectOfType<GameState>();

        //delete in newest version
        playerUnits = FindObjectsOfType<Unit>();
        playerBuildings = FindObjectsOfType<Building>();
        enemyUnits = FindObjectsOfType<Unit>();
        enemyBuildings = FindObjectsOfType<Building>();

        //simply  for testing
        InvokeRepeating("isEmpty", 2.0f, 20.0f);


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
        else
        {
            GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.GetColor("_Color");

        }
        
    }


    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;


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
            if (tile != null && tile.walkable && tile.isEmpty())
            {
                adjacencyList.Add(tile);
               // Debug.Log("Tile: " + tile + " added with coords: " + tile.GetComponent<Tile>().transform.position.x + ", " + tile.GetComponent<Tile>().transform.position.y);
            }
        }
    }


    public bool isEmpty()
    {
        bool isempty = true;

        // Need to change every reference of player/enemy unit/building to point to list inside gamestate object
        // Ex. change playerUnits to gs.playerUnits
        //foreach (Unit unit in gs.playerUnits)
        for (int i = 0; i < playerUnits.Length; i++) //delete in newest version
        {
            Unit unit = playerUnits[i];
            if (unit.transform.position == transform.position)
            {
                isempty = false;
                empty = false; //simply for testing
                break;
            }
        }
        //foreach (Building building in gs.playerBuildings)
        for (int i = 0; i < playerBuildings.Length; i++) //delete in newest version
        {
            Building building = playerBuildings[i];
            if (building.transform.position == transform.position)
            {
                isempty = false;
                empty = false; //simply for testing
                break;
            }
        }
        //foreach (Unit unit in gs.enemyUnits)
        for (int i = 0; i < enemyUnits.Length; i++) //delete in newest version
        {
            Unit unit = enemyUnits[i];
            if (unit.transform.position == transform.position)
            {
                isempty = false;
                empty = false; //simply for testing
                break;
            }
        }
        //foreach (Building building in gs.enemyBuildings)
        for (int i = 0; i < enemyBuildings.Length; i++) //delete in newest version
        {
            Building building = enemyBuildings[i];
            if (building.transform.position == transform.position)
            {
                isempty = false;
                empty = false; //simply for testing
                break;
            }
        }

        return isempty;
    }

}
