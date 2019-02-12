using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    Color ground = new Color(0.6509434f, 0.3101193f, 0.05490196f, 1);

    public List<Tilemap> adjacencyList = new List<Tilemap>();


    //Needed for BFS (breadth first search)
    public bool visited = false;
    public Tilemap parent = null;
    public int distance = 0;

    void Update()
    {
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = ground;

        }
    }


    public void Reset()
    {
        adjacencyList.Clear();
        
        current = false;
        target = false;
        selectable = false;


        visited = false;
        parent = null;
        distance = 0;
    }

    
    //jumpHeight will be used in the future to check if units can go up/down ledges
    public void FindNeighbors(float jumpHeight)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight);
        CheckTile(-Vector3.forward, jumpHeight);
        CheckTile(Vector3.right, jumpHeight);
        CheckTile(-Vector3.right, jumpHeight);

    }


    public void CheckTile(Vector3 direction, float jumpHeight)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tilemap tile = item.GetComponent<Tilemap>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }

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
