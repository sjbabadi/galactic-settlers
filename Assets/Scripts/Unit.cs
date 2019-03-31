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

    // Unit stats
    public float health;
    public float attackPower;


    private void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        //  Debug.Log("show me a tile: " + tiles[50]);

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        // Obtain references to the list of opponent units and buildings
        //enemyUnits = GameObject.FindObjectOfType<GameState>().enemyUnits;
        //enemyBuildings = GameObject.FindObjectOfType<GameState>().enemyBuildings;

        // Finds the Tile_map game object that is used for unit movement
        map = GameObject.FindObjectOfType<Tile_map>();
    }


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

    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    public bool turnUsed = false;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    protected void Init()
    {
        

      //  

        

    }


    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }


    public Tile GetTargetTile(GameObject target)
    {
      //  RaycastHit hit;
        Tile tile = null;

        RaycastHit2D hit = Physics2D.Raycast(target.transform.position, -Vector3.up);

        if (hit)
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        Debug.Log(tile);

        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight);
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
                        tile.distance = 1 + t.distance;
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
    }

    public void Move()
    {
        while (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

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



    /*


    public int tileX;
    public int tileY;


    public List<Node> currentPath = null;

    int moveSpeed = 2;

    void Update()
    {
        if(currentPath != null)
        {
            int currNode = 0;
            
            while( currNode < currentPath.Count-1 )
            {
                Vector3 start =  map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y);
                Vector3 end = map.TileCoordToWorldCoord(currentPath[currNode+1].x, currentPath[currNode+1].y);

                Debug.DrawLine(start, end, Color.blue);

                currNode++;
            }
        }
    }


    public void MoveNextTile(  )
    {
        float remainingMovement = moveSpeed;
        while(remainingMovement > 0)
        {
            if (currentPath == null)
                return ;

            remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);
          //  Debug.Log("Remaining movement left: " + remainingMovement);
            //move the unit to the next tile in the path
            tileX = currentPath[1].x;
            tileY = currentPath[1].y;
            transform.position = map.TileCoordToWorldCoord( tileX, tileY );

            //remove the old current/first node from the path
            currentPath.RemoveAt(0);
            if(currentPath.Count == 1)
            {
                //lets clear the pathfinding data because we're reached the destination
                currentPath = null;
               // return (int)remainingMovement;
            }
        }
       // return 0;
    }

    */

    public void SelectUnit()
    {
        if(turnUsed == false)
        {
            map.selectedUnit = gameObject;
            FindSelectableTiles();

            turnUsed = true;
        }
    }
}
