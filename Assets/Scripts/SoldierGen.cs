using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGen : MonoBehaviour
{
    [SerializeField]
    GameState gs;
    private GameManager gm;

    private Unit unit;
    public Tile_map map;
    private Building building;
    public GameObject soldier;

   

    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        map = GameObject.FindObjectOfType<Tile_map>();
        building = gameObject.GetComponent<Building>();

    }

    //when a building is clicked on
    public void OnMouseDown()
    {
        //for creation
        int numUnits = gs.Units[(int)gm.CurrentTurn];
        int numUnitsAllowed = gs.UnitMax[(int)gm.CurrentTurn];

        //check to see if building has already been used
        if (building.used == false) {
            //if we have enough farms to create a unit
            if (numUnitsAllowed > numUnits) {

                //create soldier at specified location
                GameObject unitref = Instantiate(soldier, new Vector2(5.0f + 5.0f, 8f), Quaternion.identity);
                unit = unitref.GetComponent<Unit>();

                //set soldier turn as used
                Debug.Log(unit);
                unit.turnUsed = true;
                Debug.Log(unit.turnUsed);

                //add to player's units (will this take up resources?)
                gs.Units[(int)gm.CurrentTurn]++;

                //set barracks as used
                building.used = true;

                
            }
        }
    }
}