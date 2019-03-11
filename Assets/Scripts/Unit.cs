using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //Refernces to enemies in GameState
    private Unit[] enemyUnits;
    private Building[] enemyBuildings;

    // Unit stats
    public float health = 100;
    public float attackPower = 15;

    // Potential targets
    public Unit enemyUnit;
    public Building enemyBuilding;
    public string target = "";

    private void Start()
    {
        // Obtain references to the list of opponent units and buildings
        //enemyUnits = GameObject.FindObjectOfType<GameState>().enemyUnits;
        //enemyBuildings = GameObject.FindObjectOfType<GameState>().enemyBuildings;

        // Finds the Tile_map game object that is used for unit movement
        map = GameObject.FindObjectOfType<Tile_map>();
    }

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
        //FindClosestEnemy();

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

        foreach (Building eB in enemyBuildings)
        {
            float dist = Vector3.Distance(eB.transform.position, currentPosition);
            if (dist < minDist)
            {
                enemyBuilding = eB;
                minDist = dist;
            }
        }
    }


    
    


    public int tileX;
    public int tileY;
    public Tile_map map;

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

    public void SelectUnit()
    {
        map.selectedUnit = gameObject;
    }
}
