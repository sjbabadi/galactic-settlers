using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    // Reference to GameState & GameManager
    private GameState gs;
    private GameManager gm;
    private Building building;

    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        building = gameObject.GetComponent<Building>();

        if (building.buildingType != Buildings.Base)
        {
            gs.buildingCounts[(int)gm.CurrentTurn, (int)building.buildingType]++;
            if (gm.CurrentTurn == Turn.Player)
            {
                gs.playerBuildings.Add(building);
                building.owner = "Player";
            }
            else
            {
                gs.enemyBuildings.Add(building);
                building.owner = "Enemy";
            }
        }
    }

    // called by units to inflict damage
    public void TakeDamage(float damage)
    {
        building.health -= damage;

        if (building.health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
