﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] public int Turn { get; set; }

    public int Money { get; set; }

    public int Food { get; set; }

    public int Population { get; set; }

    public int BaseHealth { get; set; }

    public static int Units { get; set; }

    public static int UnitMax { get; set; }

    public int MaxPop { get; set; }

    public enum Buildings { Farm, Mine, Barracks };

    public static int[] buildingCounts = new int[3];

    private HUDController HUDController;

    private void Start()
    {
        HUDController = FindObjectOfType<HUDController>();
        buildingCounts[(int)Buildings.Farm] = 0;
        buildingCounts[(int)Buildings.Mine] = 0;
        buildingCounts[(int)Buildings.Barracks] = 0;

        Money = 100;
        Population = 0;
        BaseHealth = 100;
        Units = 0;
        MaxPop = 5;
        UnitMax = 0;
        Turn = 0;
    }

    public void EndTurn()
    {
        Turn++;
        HUDController.UpdateStats();
    }

    void CalculateFood()
    {
        Food += buildingCounts[(int)Buildings.Farm];
    }

    void CalculateMoney()
    {
        Money += 25 * buildingCounts[(int)Buildings.Mine]; // 25 money per turn per mine
    }

    void CalculateMaxPop()
    {
        MaxPop = 5 * buildingCounts[(int)Buildings.Farm]; // 5 ppl allowed per farm
    }

    void CalculateUnitMax()
    {
        UnitMax = 5 * buildingCounts[(int)Buildings.Barracks];
    }


}