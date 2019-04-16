using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGen : MonoBehaviour
{
    [SerializeField]
    GameState gs;
    private GameManager gm;
    PlayerManager player;

    private Unit unit;
    public Tile_map map;
    private Building building;
    public GameObject soldier;

    //check for soldier creation intenion
    public bool soldierGenerate = false;

    Vector2[] buildTiles;
    bool selected;
    int foodCost = 2;


    private void Start()
    {
        gs = FindObjectOfType<GameState>();
        gm = FindObjectOfType<GameManager>();
        map = GameObject.FindObjectOfType<Tile_map>();
        building = gameObject.GetComponent<Building>();
        buildTiles = BuildLocations();
        player = FindObjectOfType<PlayerManager>();

    }

    private void Update()
    {
        if (selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 spawnPos = GetPos();
                Tile tile;
                tile = GetTileAt(spawnPos);
                if (tile && tile.empty)
                {
                    PlayerSoldierGenerate(spawnPos);
                }
            }        
        }
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

    private Vector2 GetPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 spawnPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

        return spawnPos;
    }

    private void PlayerSoldierGenerate(Vector2 spawnPos)
    {
        if (soldierGenerate)
        {
            //if mouse click location = one of the tiles
            if ((spawnPos == buildTiles[0] || spawnPos == buildTiles[1] || spawnPos == buildTiles[2] || spawnPos == buildTiles[3]))
            {
                //create soldier at clicked location
                GameObject unitref = SpawnSoldierAt(soldier, spawnPos);

                UpdateStats();
            }
        }
    }

    private void UpdateStats()
    {
        //add to player's units
        //these get added elsewhere, so don't need to increment here
        //gs.Units[(int)gm.CurrentTurn]++;

        //take away food used
        gs.Food[(int)gm.CurrentTurn] -= foodCost;

        //add to population
        gs.Population[(int)gm.CurrentTurn]++;

        //set barracks as used
        building.used = true;
        //Debug.Log(building.used);

        ResetTiles();
    }

    public void ResetTiles()
    {
        foreach (Vector2 t in buildTiles)
        {
            GetTileAt(t).unitGen = false;
        }
    }

    //when the barracks is clicked on
    void OnMouseDown()
    {
        if (!building.used && ResourcesAvailable() && player.inputEnabled)
        {
            soldierGenerate = true;
        }
        selected = true;

        Tile tile;
        if (!building.used && player.inputEnabled)
        {
            foreach (Vector2 t in buildTiles)
            {
                tile = GetTileAt(t);
                // Debug.Log(tile);
                if (tile.empty)
                {
                    tile.unitGen = true;
                }
            }
        }
    }

    private bool ResourcesAvailable()
    {
        bool available = false;
        //if we have enough houses(?) to create a unit & if we have enough food --(2 per soldier?)
        if (gs.UnitMax[(int)gm.CurrentTurn] > gs.Units[(int)gm.CurrentTurn] && gs.Food[(int)gm.CurrentTurn] >= foodCost)
        {
            available = true;
        }

        return available;
    }

    private Vector2[] BuildLocations()
    {
        //grab the locations of barracks and surrounding tiles
        Vector2[] buildTiles = new Vector2[4];
        buildTiles[0] = new Vector2(transform.position.x, transform.position.y + 1.0f);
        buildTiles[1] = new Vector2(transform.position.x, transform.position.y - 1.0f);
        buildTiles[2] = new Vector2(transform.position.x - 1.0f, transform.position.y);
        buildTiles[3] = new Vector2(transform.position.x + 1.0f, transform.position.y);

        return buildTiles;
    }

    public void SpawnSoldier()
    {
        Vector3 buildPosition = buildTiles[Random.Range(0,buildTiles.Length)];
        SpawnSoldierAt(soldier, buildPosition);
        UpdateStats();
    }

    private GameObject SpawnSoldierAt(GameObject soldier, Vector2 position)
    {
        GameObject unitref = Instantiate(soldier, position, Quaternion.identity);

        unitref.GetComponent<Unit>().turnUsed = true;
        soldierGenerate = false;

        return unitref;
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
