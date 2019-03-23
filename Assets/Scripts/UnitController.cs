using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // Reference to GameState & GameManager
    private GameState gs;
    private GameManager gm;
    private Unit unit;
    private Tile_map map;

    //Refernces to enemies in GameState
    private UnitController[] enemyUnits;
    private BuildingController[] enemyBuildings;

    // Potential targets
    public UnitController enemyUnit;
    public BuildingController enemyBuilding;
    public string target = "";

    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        unit = gameObject.GetComponent<Unit>();
        map = FindObjectOfType<Tile_map>();

        gs.Units[(int)gm.CurrentTurn]++;
        if (gm.CurrentTurn == Turn.Player)
        {
            gs.playerUnits.Add(unit);
        }
        else
        {
            gs.enemyUnits.Add(unit);
        }
        // Obtain references to the list of opponent units and buildings
        //enemyUnits = GameObject.FindObjectOfType<GameState>().enemyUnits;
        //enemyBuildings = GameObject.FindObjectOfType<GameState>().enemyBuildings;

        // Finds the Tile_map game object that is used for unit movement
        map = GameObject.FindObjectOfType<Tile_map>();
    }

    void Update()
    {
        if (unit.currentPath != null)
        {
            int currNode = 0;

            while (currNode < unit.currentPath.Count - 1)
            {
                Vector3 start = map.TileCoordToWorldCoord(unit.currentPath[currNode].x, unit.currentPath[currNode].y);
                Vector3 end = map.TileCoordToWorldCoord(unit.currentPath[currNode + 1].x, unit.currentPath[currNode + 1].y);

                Debug.DrawLine(start, end, Color.blue);

                currNode++;
            }
        }
    }

    public void MoveNextTile()
    {
        float remainingMovement = unit.moveSpeed;
        while (remainingMovement > 0)
        {
            if (unit.currentPath == null)
                return;

            remainingMovement -= map.CostToEnterTile(unit.currentPath[0].x, unit.currentPath[0].y, unit.currentPath[1].x, unit.currentPath[1].y);
            //  Debug.Log("Remaining movement left: " + remainingMovement);
            //move the unit to the next tile in the path
            unit.tileX = unit.currentPath[1].x;
            unit.tileY = unit.currentPath[1].y;
            transform.position = map.TileCoordToWorldCoord(unit.tileX, unit.tileY);

            //remove the old current/first node from the path
            unit.currentPath.RemoveAt(0);
            if (unit.currentPath.Count == 1)
            {
                //lets clear the pathfinding data because we're reached the destination
                unit.currentPath = null;
                // return (int)remainingMovement;
            }
        }
        // return 0;
    }

    public void SelectUnit()
    {
        if (unit.turnUsed == false)
        {
            map.selectedUnit = gameObject;
            unit.turnUsed = true;
        }
    }

    public void TakeDamage(float damage)
    {
        unit.health -= damage;

        if (unit.health <= 0)
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
                enemyUnit.TakeDamage(unit.attackPower);
            }
        }
        else if (target == "building")
        {
            if (Vector3.Distance(enemyBuilding.transform.position, transform.position) < 2)
            {
                enemyBuilding.GetComponent<BuildingController>().TakeDamage(unit.attackPower);
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
        enemyUnits = FindObjectsOfType<UnitController>();

        float minDist = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (UnitController eU in enemyUnits)
        {
            float dist = Vector3.Distance(eU.transform.position, currentPosition);
            if (dist < minDist && dist != 0)
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
        }*/
    }


}
