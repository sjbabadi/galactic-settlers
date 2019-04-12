
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buildings { Farm, Mine, Barracks, Base };

public class GameState : MonoBehaviour
{
    public int gameTurn { get; set; }
    public int[] Money { get; set; } = new int[2];
    public int[] Food { get; set; } = new int[2];
    public int[] Population { get; set; } = new int[2];
    public int[] BaseHealth { get; set; } = new int[2];
    public int[] Units { get; set; } = new int[2];
    public int[] UnitMax { get; set; } = new int[2];
    public int[] MaxPop { get; set; } = new int[2];
    public int[,] buildingCounts = new int[2,3];

    public List<Unit> playerUnits = new List<Unit>();
    public List<Building> playerBuildings = new List<Building>();
    public List<Unit> enemyUnits = new List<Unit>();
    public List<Building> enemyBuildings = new List<Building>();
    public Building playerBase;
    public Building enemyBase;

    private HUDController HUDController;
    private PlacementScript placement;
    private GameManager gm;

    private Tile_map map;

    PlayerManager player;
    EnemyManager enemy;

    void Awake()
    {
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        enemy = Object.FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
    }

    private void Start()
    {
        HUDController = FindObjectOfType<HUDController>();
        placement = FindObjectOfType<PlacementScript>();
        gm = FindObjectOfType<GameManager>();
        map = FindObjectOfType<Tile_map>();

        // Set Player and Enemy starting resources
        for (int i = 0; i < 2; i++)
        {
            buildingCounts[i, (int)Buildings.Farm] = 0;
            buildingCounts[i, (int)Buildings.Mine] = 0;
            buildingCounts[i, (int)Buildings.Barracks] = 0;
            Money[i] = 1000;
            Food[i] = 0;
            Population[i] = 0;
            BaseHealth[i] = 100;
            Units[i] = 0;
            MaxPop[i] = 5;
            UnitMax[i] = 0;
            gameTurn = 0;
        }
        HUDController.UpdateStatText();
    }

    public void EndTurn()
    {
        gameTurn++;
        UpdateStats();
        HUDController.UpdateStatText();

        foreach (Unit unit in GameObject.FindObjectsOfType<Unit>())
        {
            unit.turnUsed = false;
        }

        foreach (Building building in GameObject.FindObjectsOfType<Building>())
        {
            building.used = false;
        }

        map.selectedUnit = null;

        player.FinishTurn();
    }


    void UpdateStats()
    {
        CalculateFood();
        CalculateMoney();
        CalculateMaxPop();
        CalculateUnitMax();
    }

    public void CalculateFood()
    {
        Food[(int)gm.CurrentTurn] += buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Farm];
    }

    void CalculateMoney()
    {
        Money[(int)gm.CurrentTurn] += 25 * buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Mine]; // 25 money per turn per mine
    }

    public void CalculateMaxPop()
    {
        MaxPop[(int)gm.CurrentTurn] = 5 * buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Farm]; // 5 ppl allowed per farm
    }

    public void CalculateUnitMax()
    {
        UnitMax[(int)gm.CurrentTurn] = 5 * buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Barracks];
    }
    
}