﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class Tile_map : MonoBehaviour
{
    
    //TODO figure out a script for selecting units
    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] tiles;
    Node[,] graph;

    int mapSizeX = 10;
    int mapSizeY = 10;

    private void Start()
    {
        //setup the selected Unit's variable coordinates
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

        GenerateMapData();
        GeneratePathFindingGraph();
        GenerateMapVisual();
    }



    void GenerateMapData() { 
        //Allocate our map tiles
        tiles = new int[mapSizeX, mapSizeY];

        //Initialize our map tiles, starts with filling entire map with ground first
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        // Here's where we decide on the design of the map with other tiles
        tiles[4, 3] = 1;
        tiles[4, 4] = 1;
        tiles[4, 5] = 1;
        tiles[5, 3] = 1;
        tiles[5, 4] = 1;
        tiles[5, 5] = 1;
        tiles[6, 3] = 1;
        tiles[6, 4] = 1;
        tiles[6, 5] = 1;


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

        return tt.movementCost;
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

                if (x > 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                    if (y < mapSizeY - 1)
                        graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                }
                if (x < mapSizeX - 1)
                {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    if (y < mapSizeY - 1)
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                }
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x , y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x , y + 1]);



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