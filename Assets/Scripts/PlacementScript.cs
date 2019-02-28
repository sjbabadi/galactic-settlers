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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 spawnPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

        //select an object
        if (Input.GetKeyDown("f") && isAnObjectSelected == false)
        {
            selectedObjectInArray = 0;
            currentlySelectedObject = (GameObject)Instantiate(selectableObjects[selectedObjectInArray], spawnPos, Quaternion.identity);
            isAnObjectSelected = true;
        }
        else if (Input.GetKeyDown("h") && isAnObjectSelected == false)
        {
            selectedObjectInArray = 1;
            currentlySelectedObject = (GameObject)Instantiate(selectableObjects[selectedObjectInArray], spawnPos, Quaternion.identity);
            isAnObjectSelected = true;
        }

        //clear object selected my right mouse click
        if (Input.GetMouseButtonDown(1) && isAnObjectSelected == true)
        {
            Destroy(currentlySelectedObject);
            isAnObjectSelected = false;
            selectedObjectInArray = 0;
        }
       

    }
}
