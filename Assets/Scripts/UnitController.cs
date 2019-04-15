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

    public List<Tile> selectableTiles = new List<Tile>();
    Tile[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    [SerializeField]
    Tile currentTile;

    public bool moving = false;
    public Tile targetLocation;

    public int move;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    public Vector2[] neighbors;

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
        currentTile.occupant = gameObject;

        //Determine neighbor locations
        neighbors = FindNeighborLocations();
    }

    void Update()
    {
        if (moving)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetLocation.transform.position, step);
            if (transform.position == targetLocation.transform.position)
            {
                
                moving = false;
                currentTile = GetTargetTile(gameObject);
                neighbors = FindNeighborLocations();

                foreach (Vector2 neighbor in neighbors)
                {
                    RaycastHit2D neigh = Physics2D.Raycast(neighbor, new Vector3(0, 0, -1));
                    //Debug.Log(neigh.collider.name);
                    if (neigh && neigh.collider.GetComponent<UnitController>() && neigh.collider.GetComponent<UnitController>().owner != owner)
                    {
                        neigh.collider.GetComponent<UnitController>().TakeDamage(attackPower);
                        break;
                    }
                    else if (neigh && neigh.collider.GetComponent<BuildingController>() && neigh.collider.GetComponent<Building>().owner != owner)
                    {
                        neigh.collider.GetComponent<BuildingController>().TakeDamage(attackPower);
                        break;
                    }
                }

            }
        }
        else if (gs.selectedUnit != null && gs.selectedUnit.GetComponent<UnitController>() == this)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 position = GetPosition();
                RaycastHit2D[] hits = Physics2D.RaycastAll(position, new Vector3(0,0,-1));

                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.GetComponent<UnitController>())
                    {
                        UnitController target = hit.collider.GetComponent<UnitController>();
                        if (!turnUsed && target.owner != owner && Vector3.Distance(this.transform.position, target.transform.position) <= strikingDistance)
                        {
                            Debug.Log("attack");
                            target.TakeDamage(attackPower);
                            gs.selectedUnit = null;
                            turnUsed = true;
                            RemoveSelectableTiles();
                            break;
                        }
                        else
                        {
                            Debug.Log("Trying to attack a friendly!? Or, are you too far?");
                        }
                    }
                    else if (hit.collider.GetComponent<Tile>())
                    {
                        Tile t = hit.collider.GetComponent<Tile>();

                        if (!turnUsed && t.selectable && t.empty)
                        {
                            if (gs.selectedUnit)
                            {                          
                                SetTargetLocation(t);
                            }
                        }
                    }
                }
            }           
        }
    }

    public void SetTargetLocation(Tile t)
    {
        currentTile.empty = true;
        currentTile.occupant = null;
        t.empty = false;
        t.occupant = gameObject;
        targetLocation = t;
        turnUsed = true;
        moving = true;
        gs.selectedUnit = null;
        RemoveSelectableTiles();
    }

    public Vector2[] FindNeighborLocations()
    {
        Vector2[] locations = new Vector2[4];
        locations[0] = new Vector2(transform.position.x, transform.position.y + 1.0f);
        locations[1] = new Vector2(transform.position.x, transform.position.y - 1.0f);
        locations[2] = new Vector2(transform.position.x - 1.0f, transform.position.y);
        locations[3] = new Vector2(transform.position.x + 1.0f, transform.position.y);
        return locations;
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
        if (!turnUsed)
        {
            gs.selectedUnit = gameObject;
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

    //public void MoveToTile(Tile tile)
    //{
    //    path.Clear();
    //    tile.target = true;
    //    moving = true;

    //    Tile next = tile;
    //    while (next != null)
    //    {
    //        path.Push(next);
    //        next = next.parent;
    //    }
    //    Move();
    //}

    //public void Move()
    //{
    //    while (path.Count > 0)
    //    {
    //        Tile t = path.Peek();
    //        Vector3 target = t.transform.position;

    //        //calculate the unit's position on top of the target tile
    //        //target.y += halfHeight + t.GetComponent<Collider2D>().bounds.extents.y;

    //        if (Vector3.Distance(transform.position, target) >= 0.05f)
    //        {

    //            //Debug.Log("made it here");

    //            CalculateHeading(target);
    //            SetHorizontalVelocity();

    //            //  transform.forward = heading;
    //            transform.position += velocity * Time.deltaTime;
    //        }
    //        else
    //        {
    //            //Tile center reached
    //            gs.selectedUnit.transform.position = target;
    //            //Debug.Log(transform.rotation);
    //            path.Pop();
    //        }

    //    }
    //    RemoveSelectableTiles();

    //    moving = false;
    //}

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
            currentTile.empty = true;
            currentTile.occupant = null;
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
