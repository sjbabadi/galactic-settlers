using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int Money { get; set; }
    public int Turn { get; set; }
    public float PopulationCurrent { get; set; }
    public float PopulationMax { get; set; }
    public int UnitsCurrent { get; set; }
    public int UnitsMax { get; set; }
    public float Food { get; set; }

    //buildings ID'd by index. current order: Farm[0], Barracks[1], House[2], Mine[3]
    public int[] buildingCounts = new int[4];
    private UIController uiController;

    //keep track of all enemies and allies, objects add themselves when created
    public List<Enemy> enemies = new List<Enemy>();
    public List<Soldier> allies = new List<Soldier>();

    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        buildingCounts[0] = 4;
        buildingCounts[1] = 4;
        buildingCounts[2] = 4;
        buildingCounts[3] = 4;

        Money = 10000;
        Food = 6;
        UnitsMax = 10;
        PopulationCurrent = 0;
        Turn = 1;
    }

    //methods for calculating each stat (we can change these accordingly), these are run when end turn is clicked
    public void EndTurn()
    {
        Turn++;
        CalculateCash();
        CalculateFood();
        CalcuateUnitsMax();
        CalculatePopulationMax();
        uiController.UpdateTurnCount();
        uiController.UpdatePlayerData();

        // used to move or attack surrounding tiles for every ally/enemy
        foreach (Soldier ally in allies)
        {
            if (ally != null)
            {
                ally.TakeAction();
            }
        }
        
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.TakeAction();
            }
        }
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
            //PopulationCurrent = Mathf.Min(PopulationCurrent += Food * .5f, PopulationMax);
        } else if(Food < PopulationCurrent)
        {
            //if we didn't have enough food, some people die - this formula is arbitrary
            //PopulationCurrent -= (PopulationCurrent - Food) * .5f;
        }
    }
}
