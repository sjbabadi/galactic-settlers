using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //define 2D array to store building placement info
    private Building[,] buildings = new Building[100, 100];

    public void AddBuilding(Building building, Vector3 position)
    {
        Building toAdd = Instantiate(building, position, Quaternion.identity);
        buildings[(int)position.x, (int)position.z] = toAdd;
    }

    public bool isPositionAvailable(Vector3 position)
    {
        return buildings[(int)position.x, (int)position.z] == null;
    }

    //round the mouse click to int values so it can map to grid coordinates
    public Vector3 CalculateGridPosition(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), 1f, Mathf.Round(position.z));
    }
}
