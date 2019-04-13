using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : Unit
{
    //Attack animation
    [SerializeField] GameObject attackAnimation;

    // Reference to GameState & GameManager
    private GameState gs;
    private GameManager gm;
    //private Unit unit;

    //Refernces to enemies in GameState
    private UnitController[] enemyUnits;
    private BuildingController[] enemyBuildings;

    // Potential targets
    public UnitController enemyUnit;
    public BuildingController enemyBuilding;
    public string target = "";

    List<Tile> selectableTiles = new List<Tile>();
    Tile[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    [SerializeField]
    Tile currentTile;

    public bool moving = false;

    public int move;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    //float halfHeight = 0;

    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        //unit = gameObject.GetComponent<Unit>();
        owner = gm.CurrentTurn;

        gs.Units[(int)owner]++;
        if (owner == Turn.Player)
        {
            gs.playerUnits.Add(this);
        }
        else
        {
            gs.enemyUnits.Add(this);
        }

        // Finds the Tile_map game object that is used for unit movement
        tiles = FindObjectsOfType<Tile>();
        currentTile = GetTargetTile(gameObject);
        currentTile.empty = false;

    //    ////for testing takedamage functionality
    //    Invoke("test", 3);
    //    Invoke("test", 4);
    //    Invoke("test", 5);
    //    Invoke("test", 6);
    }

    //void test()
    //{
    //    TakeDamage(30f);
    //}

    void Update()
    {
        if (gs.selectedUnit != null && gs.selectedUnit.GetComponent<UnitController>() == this && !moving)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 position = GetPosition();

                RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

                if (hit)
                {
                    if (hit.collider.tag == "Tile")
                    {
                        Tile t = hit.collider.GetComponent<Tile>();

                        if (t.selectable)
                        {
                            if (gs.selectedUnit)
                            {
                                currentTile.empty = true;
                                MoveToTile(t);
                                t.empty = false;
                                
                                gs.selectedUnit = null;
                                currentTile = GetTargetTile(gameObject);
                                currentTile.empty = false;
                                turnUsed = true;
                            }

                        }
                    }
                }
            }
            
        }
    }

    public void Reset()
    {
        gs.selectedUnit = null;
        turnUsed = false;
        RemoveSelectableTiles();
        moving = false;
    }
    private void OnMouseDown()
    {
        //Debug.Log("Selecting Unit: " + this.name);
        gs.selectedUnit = gameObject;
        if (!turnUsed)
        {
            FindSelectableTiles();
        }
    }

    private Vector3 GetPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        return screenPos;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gs.selectedUnit);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        //Debug.Log(target.transform.position);
        return GetTileAt(target.transform.position);
    }

    public Tile GetTileAt(Vector2 position)
    {
        Tile tile = null;

        RaycastHit2D[] hits = Physics2D.RaycastAll(position, new Vector3(0, 0, 1));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "Tile")
            {
                tile = hit.collider.GetComponent<Tile>();
            }
        }
        return tile;
    }

    public void ComputeAdjacencyLists()
    {
        foreach (Tile tile in tiles)
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

                //Debug.Log("made it here");

                CalculateHeading(target);
                SetHorizontalVelocity();

                //  transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                gs.selectedUnit.transform.position = target;
                //Debug.Log(transform.rotation);
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
        //heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    public void SelectUnit()
    {
        if (turnUsed == false)
        {
            //map.selectedUnit = gameObject;
            FindSelectableTiles();

            turnUsed = true;
        }
    }

    public void TakeDamage(float damage)
    {
        Instantiate(attackAnimation, transform.position + new Vector3(0, 0, -2), Quaternion.identity); //Attack animation position is modified so that it appears on top of the soldier
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else if (health <= 25)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (health <= 50)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
        }
    }

    public void Attack(GameObject target)
    {
        if (target.GetComponent<UnitController>())
        {
            target.GetComponent<UnitController>().TakeDamage(attackPower);
        }
        else if (target.GetComponent<BuildingController>())
        {
            target.GetComponent<BuildingController>().TakeDamage(attackPower);
        }
        else
        {
            Debug.Log("Target is not building or unit");
        }
    }
}
