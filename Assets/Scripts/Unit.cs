using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //Refernces to enemies in GameState
    private Unit[] enemyUnits;
    private Building[] enemyBuildings;

    // Potential targets
    public Unit enemyUnit;
    public Building enemyBuilding;
    public string target = "";

    public Tile_map map;

    //public GameObject selectedUnit;

    // Unit stats
    public float health;
    public float attackPower;


    private void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile"); 

        // Obtain references to the list of opponent units and buildings
        //enemyUnits = GameObject.FindObjectOfType<GameState>().enemyUnits;
        //enemyBuildings = GameObject.FindObjectOfType<GameState>().enemyBuildings;

        // Finds the Tile_map game object that is used for unit movement
        map = GameObject.FindObjectOfType<Tile_map>();
    }

    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            getUnit();
        }

    }

    
    public void getUnit()
    {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(screenPos, -Vector2.up, 1, LayerMask.NameToLayer("Player"));
        //Debug.Log(screenPos);
        Debug.Log(hit);
            if (hit)
            {

                if ((hit.collider != null) && (hit.collider.tag == "Player"))
                {
                    Debug.Log("got here");
                    selectedUnit = gameObject;

                }
            }

        
        if (selectedUnit)
        {
            if (!turnUsed)
            {
                FindSelectableTiles();
            }

            turnUsed = true;
        }
    }
    */

    /*

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        FindClosestEnemy();

        // Targets units over buildings first as they can fight back
        if (enemyUnit)
        {
            target = "unit";
        }
        else if (enemyBuilding)
        {
            target = "building";
        }
        else
        {
            target = "";
        }
        
        if (target == "unit")
        {
            if (Vector3.Distance(enemyUnit.transform.position, transform.position) < 2)
            {
                enemyUnit.TakeDamage(attackPower);
            }
        }
        else if (target == "building")
        {
            if (Vector3.Distance(enemyBuilding.transform.position, transform.position) < 2)
            {
                enemyBuilding.TakeDamage(attackPower);
            }
        }
        else
        {
            // TODO
            // Maybe need to create a dialog that displays to the user that there are no nearby targets
            Debug.Log("No target");
        }
    }

    // Finds the closest enemy of type Building and Unit
    private void FindClosestEnemy()
    {
        enemyUnits = FindObjectsOfType<Unit>();

        float minDist = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Unit eU in enemyUnits)
        {
            float dist = Vector3.Distance(eU.transform.position, currentPosition);
            if (dist < minDist)
            {
                enemyUnit = eU;
                minDist = dist;
            }
        }
        /*
        foreach (Building eB in enemyBuildings)
        {
            float dist = Vector3.Distance(eB.transform.position, currentPosition);
            if (dist < minDist)
            {
                enemyBuilding = eB;
                minDist = dist;
            }
        }
        */



    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    public bool moving = false;

    public int move = 2;
    
    public float moveSpeed = 2;
    public bool turnUsed = false;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

  //  float halfHeight = 0;

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }


    public Tile GetTargetTile(GameObject target)
    {
      //  RaycastHit hit;
        Tile tile = null;

        RaycastHit2D hit = Physics2D.Raycast(target.transform.position, -Vector2.up);

        if (hit)
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        //Debug.Log(tile);

        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = tile.movementCost + t.distance;
                        //Debug.Log("Tile: " + tile + " added with coords: " + tile.GetComponent<Tile>().transform.position.x + ", " + tile.GetComponent<Tile>().transform.position.y
                        //            + " with distance: " + tile.distance);
                        //Debug.Log(tile.movementCost + ", " + t.distance + ", " + tile.distance);
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
        Move();
    }

    public void Move()
    {
        while (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //calculate the unit's position on top of the target tile
            //target.y += halfHeight + t.GetComponent<Collider2D>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {

                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                transform.position = target;
                path.Pop();
            }

        }
        RemoveSelectableTiles();
        moving = false;
    }

    //remove the selectable tiles
    protected void RemoveSelectableTiles()
    {

        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }


    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    public void SelectUnit()
    {
        if(turnUsed == false)
        {
            //map.selectedUnit = gameObject;
            FindSelectableTiles();

            turnUsed = true;
        }
    }
}
