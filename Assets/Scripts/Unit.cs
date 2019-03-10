using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Unit stats
    public float health = 100;
    public float attackPower = 15;

    // Potential targets
    public Unit enemyUnit;
    public Building enemyBuilding;
    private string target = "";

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        if(target == "unit")
        {
            enemyUnit.TakeDamage(attackPower);
        }
        else if (target == "building")
        {
            enemyBuilding.TakeDamage(attackPower);
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

}
