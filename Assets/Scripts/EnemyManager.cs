using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class EnemyManager : TurnManager
{
    GameState gs;
    Vector2 basePos;
    Vector2 nextBuildSpot;
    Tile tileRef;

    int maxXFromBase = 4;
    int maxYFromBase = 4;

    Vector2 spot1;
    Vector2 spot2;
    Vector2 spot3;
    Vector2 spot4;
    Vector2 spot5;
    Vector2 spot6;
    Vector2 spot7;
    Vector2 spot8;
    Vector2 spot9;
    Vector2 spot10;
    Vector2 spot11;
    Vector2 spot12;

    Vector2[] buildSpots = new Vector2[12];


    [SerializeField] public GameObject enemyBarracks;
    [SerializeField] public GameObject enemyFarm;
    [SerializeField] public GameObject enemyMine;
    [SerializeField] public GameObject enemyUnit;

    protected override void Awake()
    {
        base.Awake();
        gs =Object.FindObjectOfType<GameState>().GetComponent<GameState>();
        tileRef = FindObjectOfType<Tile>();

    }

    void Start()
    {
        basePos = new Vector2(gs.enemyBase.transform.position.x, gs.enemyBase.transform.position.y);
        spot1 = basePos + new Vector2(2, -2);
        spot2 = spot1 + new Vector2(0, 2);
        spot3 = spot2 + new Vector2(0, 2);
        spot4 = spot3 + new Vector2(2, 0);
        spot5 = spot4 + new Vector2(2, 0);
        spot6 = spot5 + new Vector2(2, 0);
        spot7 = spot6 + new Vector2(0, -2);
        spot8 = spot7 + new Vector2(-2, 0);
        spot9 = spot8 + new Vector2(-2, 0);
        spot10 = spot9 + new Vector2(2, -2);
        spot11 = spot10 + new Vector2(2, -2);
        spot12 = spot11 + new Vector2(2, -2);
        buildSpots[0] = spot1;
        buildSpots[1] = spot2;
        buildSpots[2] = spot3;
        buildSpots[3] = spot4;
        buildSpots[4] = spot5;
        buildSpots[5] = spot6;
        buildSpots[6] = spot7;
        buildSpots[7] = spot8;
        buildSpots[8] = spot9;
        buildSpots[9] = spot10;
        buildSpots[10] = spot11;
        buildSpots[11] = spot12;


    }

    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        //enemy's action sequence
        yield return new WaitForSeconds(4f);
        FinishTurn();
    }

    [Task]
    public bool IsMyTurn()
    {
        return gm.CurrentTurn == Turn.Enemy;
    }

    [Task]
    public bool AtLeastOneOfEachBuilding()
    {
        return (gs.buildingCounts[1, (int)Buildings.Farm] > 0 &&
        gs.buildingCounts[1, (int)Buildings.Mine] > 0 &&
        gs.buildingCounts[1, (int)Buildings.Barracks] > 0);
    }

    [Task]
    public bool NoUnits()
    {
        return gs.enemyUnits.Count == 0;
    }

    [Task] //go through each barracks and spawn a soldier if barracks is not used and have enough food
    public void SpawnUnits()
    {
        foreach(Building building in gs.enemyBuildings)
        {
            if(building.buildingType == Buildings.Barracks)
            {
                building.GetComponent<SoldierGen>().SpawnSoldier();
            }
        }
        Task.current.Succeed();
    }

    [Task]
    public void BuildFarm()
    {
        if (SpotAvailable())
        {
            GameObject g = (GameObject)Instantiate(enemyFarm, nextBuildSpot, Quaternion.identity);
            nextBuildSpot = GenerateBuildSpot();
        }
        gs.buildingCounts[1, (int)Buildings.Farm]++;
        Task.current.Succeed();
    }

    [Task]
    public void BuildBarracks()
    {
        if (SpotAvailable())
        {
            GameObject g = (GameObject)Instantiate(enemyBarracks, nextBuildSpot, Quaternion.identity);
            nextBuildSpot = GenerateBuildSpot();
        }
        gs.buildingCounts[1, (int)Buildings.Barracks]++;
        Task.current.Succeed();
    }

    [Task]
    public void BuildMine()
    {
        if(SpotAvailable())
        {
            GameObject g = (GameObject)Instantiate(enemyMine, nextBuildSpot, Quaternion.identity);
            nextBuildSpot = GenerateBuildSpot();
        }
        gs.buildingCounts[1, (int)Buildings.Mine]++;
        Task.current.Succeed();
    }

    public Vector2 GenerateBuildSpot()
    {
            foreach (Vector2 spot in buildSpots)
            {
                if (tileRef.GetTileAt(spot).empty)
                {
                    tileRef.GetTileAt(spot).empty = false;
                    return spot;
                }
            }

       return basePos;
    }

    bool SpotAvailable()
    {
        foreach (Vector2 spot in buildSpots)
        {
            if (tileRef.GetTileAt(spot).empty)
            {
                return true;
            }
        }
        return false;
    }

    [Task]
    public void SendAttackers()
    {
        foreach(Unit unit in gs.enemyUnits)
        {
            unit.GetComponent<EnemyUnitController>().attackMode = true;
        }
    }
}