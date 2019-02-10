using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //define 2D array to store building placement info
    private Building[,] buildings = new Building[100, 100];

    //more generalized version of buildings 2D array
    public GameObject[,] pieces = new GameObject[100, 100];

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


    //methods below fully used
    public void AddPiece(GameObject gamePiece, Vector3 position)
    {
        GameObject toAdd = Instantiate(gamePiece, position, Quaternion.identity);
        //buildings[(int)position.x, (int)position.z] = toAdd;

        pieces[(int)position.x, (int)position.z] = toAdd.gameObject;
    }

    public void movePiece(Vector3 currentPosition, Vector3 newPosition)
    {
        pieces[(int)newPosition.x, (int)newPosition.z] = pieces[(int)currentPosition.x, (int)currentPosition.z];
        pieces[(int)currentPosition.x, (int)currentPosition.z] = null;
    }

    public void deletePiece(Vector3 position)
    {
        pieces[(int)position.x, (int)position.z] = null;
    }
}
