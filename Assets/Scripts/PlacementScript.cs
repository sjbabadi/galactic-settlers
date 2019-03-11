using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementScript : MonoBehaviour
{
    private int selectedObjectInArray;
    private GameObject currentlySelectedObject;

    [SerializeField]
    private GameObject[] selectableObjects;

    private bool isAnObjectSelected = false;

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

        //clear object selected by right mouse click
        if (Input.GetMouseButtonDown(1) && isAnObjectSelected == true)
        {
            Destroy(currentlySelectedObject);
            isAnObjectSelected = false;
            selectedObjectInArray = 0;
        }
    }

    //Instantiates the object based on key pressed or button clicked
    public void SelectObject(int selectedObjectInArray)
    {
        if(isAnObjectSelected == false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 spawnPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

            currentlySelectedObject = (GameObject)Instantiate(selectableObjects[selectedObjectInArray], spawnPos, Quaternion.identity);
            isAnObjectSelected = true;

            //update building counts in gamestate's array
            if (selectedObjectInArray == 0)
            {
                GameState.buildingCounts[0]++;
            }
            else if (selectedObjectInArray == 1)
            {
                GameState.buildingCounts[1]++;
            } else
            {
                GameState.buildingCounts[2]++;
            }
        }

    }
}
