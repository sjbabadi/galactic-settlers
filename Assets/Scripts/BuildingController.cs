﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingController : MonoBehaviour
{
    //Attack animation
    [SerializeField] GameObject attackAnimation;

    // Reference to GameState & GameManager
    private GameState gs;
    private GameManager gm;
    private Building building;
    public Tile location;

    [SerializeField] float radius = 2f;

    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        building = gameObject.GetComponent<Building>();

        location = GetTargetTile(gameObject);
        location.empty = false;
        location.occupant = gameObject;
        //Debug.Log(location.occupant.name);

        if (building.buildingType != Buildings.Base)
        {
            building.owner = gm.CurrentTurn;
        }

        UpdateBuildingLocations();
        AddToBuildingLists();

        building.neighbors = FindNeighborLocations();
    }

    private Vector2[] FindNeighborLocations()
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
        building.used = false;
        if (building.buildingType == Buildings.Barracks)
        {
            GetComponent<SoldierGen>().ResetTiles();
        }
    }

    // called by units to inflict damage
    public void TakeDamage(float damage)
    {
        Instantiate(attackAnimation, building.transform.position + new Vector3(0, 0, -2), Quaternion.identity);
        building.health -= damage;

        if (building.health <= 0)
        {
            location.empty = true;
            location.occupant = null;
            Destroy(gameObject);
        }
    }

    public Tile GetTargetTile(GameObject target)
    {
        Tile tile = null;

        RaycastHit2D[] hits = Physics2D.RaycastAll(target.transform.position, new Vector3(0, 0, 1));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "Tile")
            {
                //Debug.Log(hit.collider.name);
                tile = hit.collider.GetComponent<Tile>();
            }
        }
        return tile;
    }

    private void UpdateBuildingLocations()
    {
        if (radius > 0)
        {
            Collider2D[] nearTiles = Physics2D.OverlapCircleAll(transform.position, radius);
            if (building.owner == Turn.Player)
            {
                foreach (Collider2D c in nearTiles)
                {
                    // layer 8 is BuildingTileLayer
                    if (c.gameObject.layer == 9 && c.GetComponent<Tile>() && !gs.playerBuildingLocations.Contains(c.GetComponent<Tile>()))
                    {
                        //Debug.Log(c.name);
                        gs.playerBuildingLocations.Add(c.GetComponent<Tile>());
                    }
                }
            }
            else
            {
                foreach (Collider2D c in nearTiles)
                {
                    if (c.gameObject.layer == 9 && c.GetComponent<Tile>() && !gs.enemyBuildingLocations.Contains(c.GetComponent<Tile>()))
                    {
                        gs.enemyBuildingLocations.Add(c.GetComponent<Tile>());
                    }
                }
            }
        }
        //CompactLists();
    }

    //private void CompactLists()
    //{

    //}

    private void AddToBuildingLists()
    {
        if (building.buildingType == Buildings.Base)
        {
            if (building.owner == Turn.Player)
            {
                gs.playerBase = building;
            }
            else
            {
                gs.enemyBase = building;
            }
        }
        else
        {

            if (building.owner == Turn.Player)
            {
                gs.playerBuildings.Add(building);
                gs.buildingCounts[(int)Turn.Player, (int)building.buildingType]++;
            }
            else
            {
                gs.enemyBuildings.Add(building);
                gs.buildingCounts[(int)Turn.Enemy, (int)building.buildingType]++;
            }
        }
    }
}
