using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGen : MonoBehaviour
{
    [SerializeField]
    GameState gs;
    private GameManager gm;

    private Unit unit;
    private Building building;
    public GameObject soldier;

    //check for soldier creation intenion
    public bool soldierGenerate = false;
    int numUnits;
    int numUnitsAllowed;
    int foodAvail;

    //for location purposes
    Vector2 barrLocation;
    Vector2 tileTop;
    Vector2 tileBot;
    Vector2 tileLef;
    Vector2 tileRig;

    //mouse location
    Vector2 mousePos;
    Vector2 spawnPos;


    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        building = gameObject.GetComponent<Building>();
    }

    
    private void Update()
    {
        getMousePos();

    }
    

    //when the barracks is clicked on
    void OnMouseDown()
    {
        //grab the locations of barracks and surrounding tiles
        barrLocation = transform.position;
        tileTop = new Vector3(barrLocation.x, barrLocation.y + 1.0f);
        tileBot = new Vector3(barrLocation.x, barrLocation.y - 1.0f);
        tileLef = new Vector3(barrLocation.x - 1.0f, barrLocation.y);
        tileRig = new Vector3(barrLocation.x + 1.0f, barrLocation.y);

        //check values for creation of soldier
        numUnits = gs.Units[(int)gm.CurrentTurn];
        numUnitsAllowed = gs.UnitMax[(int)gm.CurrentTurn];
        foodAvail = gs.Food[(int)gm.CurrentTurn];

        //check to see if barracks has already been used
        if (building.used == false)
        {
            //if we have enough houses(?) to create a unit
            if (numUnitsAllowed > numUnits)
            {
                //if we have enough food --(2 per soldier?)
                if (foodAvail >= 2)
                {
                    soldierGenerate = true;
                }
            }
        }
    }

    void getMousePos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

            if (soldierGenerate) {
                //if mouse click location = one of the tiles
                if (spawnPos == tileTop || spawnPos == tileBot || spawnPos == tileLef || spawnPos == tileRig)
                {
                    //create soldier at clicked location
                    GameObject unitref = Instantiate(soldier, spawnPos, Quaternion.identity);
                    unit = unitref.GetComponent<Unit>();
                    soldierGenerate = false;

                    //set soldier turn as used
                    unit.Exhaust();

                    //add to player's units
                    gs.Units[(int)gm.CurrentTurn]++;

                    //take away food used
                    gs.Food[(int)gm.CurrentTurn] -= 5;

                    //set barracks as used
                    building.used = true;
                    //Debug.Log(building.used);
                }

            }
        }
    }
}
