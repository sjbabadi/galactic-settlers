using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGen : MonoBehaviour
{
    [SerializeField]
    public GameState gs;
    public GameManager gm;

    public Unit unit;
    public Tile_map map;
    private Building building;
    public GameObject soldier;

    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        unit = gameObject.GetComponent<Unit>();
        map = FindObjectOfType<Tile_map>();
        map = GameObject.FindObjectOfType<Tile_map>();
        building = gameObject.GetComponent<Building>();

    }

    //when a building is clicked on
    public void OnMouseDown()
    {
        //create soldier at some random location
        Instantiate(soldier, new Vector2(5.0f + 5.0f, 8f), Quaternion.identity);
        
        //set soldier turn as used
        unit.turnUsed = true;
        Debug.Log(unit.turnUsed);

        //add to player's units
        gs.playerUnits.Add(unit);

        //set barracks as used
        building.used = true;
    }
}
