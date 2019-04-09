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

    //check for soldier creation intenion and creation
    public bool barracksClicked = false;
    public bool soldierGenerated = false;

    //for location purposes
    Vector2 barrLocation;
    Vector2 tileTop;
    Vector2 tileBot;
    Vector2 tileLef;
    Vector2 tileRig;

    //mouse location
    Vector2 mousePos;
    Vector2 spawnPos;


    //public int[][] neighbors;
    //public TileType[] tileTypes;
    //public TileSelection tileSelection;


    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        map = GameObject.FindObjectOfType<Tile_map>();
        building = gameObject.GetComponent<Building>();

    }

    private void Update()
    {
        getMousePos();
        OnMouseDown();

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
        int numUnits = gs.Units[(int)gm.CurrentTurn];
        int numUnitsAllowed = gs.UnitMax[(int)gm.CurrentTurn];
        int foodAvail = gs.Food[(int)gm.CurrentTurn];

        //check to see if barracks has already been used
        if (building.used == false)
        {
            //if we have enough houses(?) to create a unit
            if (numUnitsAllowed > numUnits)
            {
                //if we have enough food --(2 per soldier?)
                if (foodAvail >= 2)
                {
                    //if mouse click location = one of the tiles
                    if (spawnPos == tileTop || spawnPos == tileBot || spawnPos == tileLef || spawnPos == tileRig)
                    {
                        Debug.Log("I make it through the tile check!");

                        //create soldier at clicked location
                        GameObject unitref = Instantiate(soldier, spawnPos, Quaternion.identity);
                        unit = unitref.GetComponent<Unit>();

                        //set soldier turn as used
                        Debug.Log(unit);
                        unit.turnUsed = true;
                        Debug.Log(unit.turnUsed);

                        //add to player's units
                        gs.Units[(int)gm.CurrentTurn]++;

                        //take away food used
                        gs.Food[(int)gm.CurrentTurn] -= 5;

                        //set barracks as used
                        building.used = true;

                    }
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
        }
    }
}



/*
 *                  //this changes the color of the barracks
                    //GetComponent<Renderer>().material.color = Color.magenta;


                    //neighbors[x][y] = map.GetTile(location);
                    //public map.TileBase GetTile(tilePos);



                    Tile tile = item.GetComponent<Tile>();

                    ----

                    Tile tile = (Tile)tilemap.GetTile(coord);

                    ----

                    BoundsInt bounds = tilemap.cellBounds;
                    TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

                    ----

                    // Get the object Tile or TilesetBrush
                    Tile tile = tilemap.GetTile(-4, -8); // directly from the tilemap
                    tile = tilemap.Tileset.GetTile(tileId); // from the tileset with the tileId

                    ----

                    Vector3 pos = transform.position;
                    pos.x = 12;
                    transform.position = pos;

                    ----

                    neighboringNeighbors[xIndex] = new TileBase[NeighborCount];
                    int index = 0;
                    for (int y = 1; y >= -1; y--)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            if (x != 0 || y != 0)
                            {
                                Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, position.z);
                                neighboringNeighbors[xIndex][index] = tilemap.GetTile(tilePosition);
                                index++;
                            }
                        }
                    }
                }
                */
