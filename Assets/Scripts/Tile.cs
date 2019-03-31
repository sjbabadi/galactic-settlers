using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public int movementCost;

    //Color ground = new Color(0.6509434f, 0.3101193f, 0.05490196f, 1);

    public List<Tile> adjacencyList = new List<Tile>();


    //Needed for BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
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
     /*   else
        {
            GetComponent<Renderer>().material.color = ground;

        }
        */
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
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {

             //   RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.zero);


             //   RaycastHit hit;

             //   if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
             //   if(!hit)
             //   {
                    adjacencyList.Add(tile);
             //   }
            }
        }
    }
}
