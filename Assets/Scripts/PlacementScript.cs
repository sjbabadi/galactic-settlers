using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementScript : MonoBehaviour
{
    private static int selectedObjectInArray;
    private static GameObject currentlySelectedObject;
    private HUDController hudController;

    [SerializeField]
    GameState gs;
    private GameManager gm;
    PlayerManager player;

    [SerializeField]
    private GameObject[] selectableObjects;

    private static bool isAnObjectSelected = false;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        hudController = FindObjectOfType<HUDController>();
        player = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inputEnabled)
        {
            //select an object
            if (Input.GetKeyDown("f"))
            {
                SelectObject(0);
            }
            else if (Input.GetKeyDown("m"))
            {
                SelectObject(1);
            }
            else if (Input.GetKeyDown("b"))
            {
                SelectObject(2);
            }
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
        if (player.inputEnabled)
        {
            if (isAnObjectSelected == false)
            {
                if (HasEnoughMoney())
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 spawnPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

                    currentlySelectedObject = (GameObject)Instantiate(selectableObjects[selectedObjectInArray], spawnPos, Quaternion.identity);
                    isAnObjectSelected = true;

                    gs.Money[(int)gm.CurrentTurn] -= 100;
                    UpdateBuildingCounts(selectedObjectInArray);
                    hudController.UpdateStatText();

                    foreach (Tile t in gs.playerBuildingLocations)
                    {
                        t.movementSelect = true;
                    }
                }
                else
                {
                    Debug.Log("Not Enough Money");
                }
            }
        }
    }

    public void UpdateBuildingCounts(int selectedObjectInArray)
    { 
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

    public bool HasEnoughMoney()
    {
        return gs.Money[(int)gm.CurrentTurn] >= 100;
    }

}


