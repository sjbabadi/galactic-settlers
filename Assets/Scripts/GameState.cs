using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int Money { get; set; }
    public int Day { get; set; }
    public float PopulationCurrent { get; set; }
    public float PopulationMax { get; set; }
    public int UnitsCurrent { get; set; }
    public int UnitsMax { get; set; }
    public float Food { get; set; }

    //buildings ID'd by index. current order: Farm[0], Barracks[1], House[2], Mine[3]
    public int[] buildingCounts = new int[4];

    // Start is called before the first frame update
    void Start()
    {
        buildingCounts[0] = 4;
        buildingCounts[1] = 4;
        buildingCounts[2] = 4;
        buildingCounts[3] = 4;

        Money = 10000;
        Food = 6;
        UnitsMax = 10;   
    }

    //methods for calculating each stat (we can change these accordingly), these are run when end turn is clicked
    public void EndTurn()
    {
        Day++;
        CalculateCash();
        CalculateFood();
        CalcuateUnitsMax();
        CalculatePopulationMax();
        Debug.Log("Day ended");
        Debug.LogFormat("Jobs: {0}/{1}" +
        	"Money: {2}" +
        	"Pop: {3}/{4}" +
            "Food: {5}",
            UnitsCurrent, UnitsMax, Money, PopulationCurrent, PopulationMax, Food);
    }

    void CalcuateUnitsMax()
    {
        //can only have 10 units per barracks
        UnitsMax = buildingCounts[1] * 10;
    }

    void CalculateCash()
    {
        //2 money every turn for each mine
        Money += buildingCounts[3] * 2;
    }

    void CalculateFood()
    {
        Food += buildingCounts[0] * 4f;
    }

    void CalculatePopulationMax()
    {
        PopulationMax = buildingCounts[2] * 10;

        //mechanism for an automatically growing worker population. this might be unnecessary, but its there if we need it
        if(Food >= PopulationCurrent && PopulationCurrent < PopulationMax)
        {
            Food -= PopulationCurrent * .5f;
            PopulationCurrent = Mathf.Min(PopulationCurrent += Food * .5f, PopulationMax);
        } else if(Food < PopulationCurrent)
        {
            //if we didn't have enough food, some people die - this formula is arbitrary
            PopulationCurrent -= (PopulationCurrent - Food) * .5f;
        }
    }
}
