using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementScript : MonoBehaviour
{
    private static int selectedObjectInArray;
    private static GameObject currentlySelectedObject;

    [SerializeField]
    GameState gs;
    private GameManager gm;

    [SerializeField]
    private GameObject[] selectableObjects;

    private static bool isAnObjectSelected = false;

    //this is for UnitGen
    public GameObject Soldier;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //select an object
        if (Input.GetKeyDown("f"))
        {
            SelectObject(0);
        }
        else if (Input.GetKeyDown("h"))
        {
            SelectObject(1);
        }
        else if (Input.GetKeyDown("b"))
        {
            SelectObject(2);
        }
    }

    /// <summary>
    /// Clear the currently selected object.
    /// </summary>
    public static void ClearSelection()
    {
        if (isAnObjectSelected)
        {
            Destroy(currentlySelectedObject);
            isAnObjectSelected = false;
            selectedObjectInArray = 0;
        }
    }

    /// <summary>
    /// Instantiates the object based on key pressed or button clicked
    /// </summary>
    /// <param name="selectedObjectInArray"></param>
    public void SelectObject(int selectedObjectInArray)
    {
        if (isAnObjectSelected == false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 spawnPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

            currentlySelectedObject = (GameObject)Instantiate(selectableObjects[selectedObjectInArray], spawnPos, Quaternion.identity);
            isAnObjectSelected = true;

            //update building counts in gamestate's array
            if (selectedObjectInArray == 0)
            {
                gs.buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Farm]++;
                gs.CalculateMaxPop();
                gs.CalculateFood();
            }
            else if (selectedObjectInArray == 1)
            {
                gs.buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Mine]++;
            }
            else
            {
                gs.buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Barracks]++;
                gs.CalculateUnitMax();
            }
        }

    }

    public void UnitGen()
    {

        int numOfBarracks = gs.buildingCounts[(int)gm.CurrentTurn, (int)Buildings.Barracks];
        int numUnits = gs.Units[(int)gm.CurrentTurn];
        int numUnitsAllowed = gs.UnitMax[(int)gm.CurrentTurn];
        int unitsDiff = numUnitsAllowed - numUnits;

        //if num of units allowed is greater than or equal to num of barracks built
        if (unitsDiff >= numOfBarracks)
        {
            for (int i = 0; i < numOfBarracks; i++)
            {
                Instantiate(Soldier, new Vector2(i + 5.0f, 8f), Quaternion.identity);
                gs.Units[(int)gm.CurrentTurn]++;

            }
        }
        else
        {
            //if num of units allowed is less than num of barracks built
            for (int i = 0; i < numOfBarracks; i++)
            {
                Instantiate(Soldier, new Vector2(i + 5.0f, 8f), Quaternion.identity);
                gs.Units[(int)gm.CurrentTurn]++;
            }

        }

    }

}


