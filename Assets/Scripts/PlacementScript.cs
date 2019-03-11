using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementScript : MonoBehaviour
{
    private static int selectedObjectInArray;
    private static GameObject currentlySelectedObject;

    [SerializeField]
    private GameObject[] selectableObjects;

    private static bool isAnObjectSelected = false;

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
        if(isAnObjectSelected == false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 spawnPos = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));

            currentlySelectedObject = (GameObject)Instantiate(selectableObjects[selectedObjectInArray], spawnPos, Quaternion.identity);
            isAnObjectSelected = true;
        }
    }
}