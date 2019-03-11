using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class Tile_map : MonoBehaviour
{
    
    
    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] tiles;
    Node[,] graph;

    int mapSizeX = 30;
    int mapSizeY = 30;

    private void Start()
    {
        GenerateMapData();
        GeneratePathFindingGraph();
        GenerateMapVisual();
    }



    void GenerateMapData() { 
        //Allocate our map tiles
        tiles = new int[mapSizeX, mapSizeY];

        int[,] temptiles = {
                { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 },
                { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 12, 2, 13, 0, 14, 13, 6, 6, 12, 14, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 13, 5, 7, 0, 12, 6, 6, 12, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 12, 10, 11, 12, 14, 6, 6, 6, 13, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 9, 11, 0, 0, 0, 6, 6, 6, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 13, 6, 6, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 15, 0, 0, 0, 12, 6, 6, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 15, 17, 0, 0, 0, 0, 0, 6, 6, 6, 6, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 15, 16, 18, 17, 0, 0, 0, 13, 6, 6, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 16, 18, 17, 17, 0, 0, 0, 12, 6, 6, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 6, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 15, 17, 18, 18, 17, 0, 0, 0, 14, 6, 0, 0, 0, 0, 0, 16, 18, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 16, 18, 18, 17, 16, 0, 0, 0, 6, 12, 0, 0, 0, 0, 18, 18, 16, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 17, 18, 18, 18, 16, 0, 0, 0, 6, 6, 0, 0, 15, 16, 18, 17, 15, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 17, 18, 17, 17, 18, 17, 0, 0, 0, 0, 0, 0, 0, 16, 17, 18, 18, 17, 15, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 17, 18, 17, 15, 16, 18, 17, 0, 0, 0, 0, 0, 0, 17, 18, 18, 18, 18, 15, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 16, 15, 0, 0, 0, 0, 0, 18, 17, 17, 18, 18, 17, 16, 0, 0, 6, 6 },
                { 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 17, 18, 18, 17, 16, 0, 6, 6 },
                { 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 17, 18, 17, 0, 0, 6, 6 },
                { 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 17, 16, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 7, 0, 0, 0, 16, 15, 15, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 2, 12, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 0, 0, 12, 10, 4, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 14, 1, 13, 12, 14, 2, 8, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 13, 14, 6, 6, 11, 12, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 6, 6, 13, 14, 6, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 14, 6, 6, 6, 12, 13, 12, 12, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6 },
                { 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 6, 6, 6, 6, 12, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6 },
                { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 },
                { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 }
        };

        //temp fix cause I'm dumb and above matrix puts out map at 90 degree rotation
        // Traverse each cycle 
        for (int i = 0; i < 30 / 2; i++)
        {
            for (int j = i; j < 30 - i - 1; j++)
            {

                // Swap elements of each cycle 
                // in clockwise direction 
                int temp = temptiles[i, j];
                temptiles[i, j] = temptiles[30 - 1 - j, i];
                temptiles[30 - 1 - j, i] = temptiles[30 - 1 - i, 30 - 1 - j];
                temptiles[30 - 1 - i, 30 - 1 - j] = temptiles[j, 30 - 1 - i];
                temptiles[j, 30 - 1 - i] = temp;
            }
        }
        
        tiles = temptiles;

        // spawn the design here
        GenerateMapVisual();
    }


    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        TileType tt = tileTypes[tiles[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        if( sourceX!=targetX && sourceY!=targetY)
        {
            //makes pathfinding smoother/ less cornering
            cost += 0.001f;
        }
        
        return cost;
    } 



    void GeneratePathFindingGraph()
    {
        // Initialize the array
        graph = new Node[mapSizeX, mapSizeY];

        // Initialize a Node for each spot in the array
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        //calculate the neighbors for each Node in the array
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {

                // This is the 4-way connection version:
     /*           if (x > 0)
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                if (x < mapSizeX - 1)
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
                
    */
                // This is the 8-way connection version (allows diagonal movement)
				// Try left
				if(x > 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                    if (y < mapSizeY - 1)
                        graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
				}

				// Try Right
				if(x < mapSizeX-1)
                {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    if (y < mapSizeY - 1)
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
				}

                // Try straight up and down
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);

            }
        }
    }

        
    void GenerateMapVisual() {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];

                GameObject go = (Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity));

                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
            }
        }
    }


    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }


    public bool UnitCanEnterTile(int x, int y)
    {
        //test unit's movement type (walk, fly, hover,etc) here if we add those units.
        return tileTypes[ tiles[x,y] ].isWalkable;
    }

     
    public void GeneratePathTo(int x, int y)
    {
        // Sets the selected units starting location
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.y;

        //clear out old path
        selectedUnit.GetComponent<Unit>().currentPath = null;

        if( UnitCanEnterTile(x,y) == false)
        {
            //clicked on a tile that can't be moved onto
            return;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        //List of nodes we haven't checked yet
        List<Node> unvisited = new List<Node>();

        Node source = graph[
                            selectedUnit.GetComponent<Unit>().tileX,
                            selectedUnit.GetComponent<Unit>().tileY
                           ];

        Node target = graph[
                            x,
                            y
                           ];


        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            Node u = null;

            foreach(Node possibleU in unvisited)
            {
                if(u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if(u == target)
            {
                break;
            }

            unvisited.Remove(u);

            foreach(Node v in u.neighbours)
            {
                //   float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if (prev[target] == null)
        {
            return;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        while(curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        selectedUnit.GetComponent<Unit>().currentPath = currentPath;
    }

}