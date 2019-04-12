using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    #region Fields / Properties
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };
    Color selectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);
    #endregion

    #region Public
    public void Load(LevelData data)
    {
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }

    /// <summary>
    /// Get the tile at a given position.
    /// </summary>
    /// <param name="p">The position to look for the tile in.</param>
    /// <returns></returns>
    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    /// <summary>
    /// Returns a list of tiles starting from a specific tile which meet a certain criteria.
    /// </summary>
    /// <param name="start">The starting tile.</param>
    /// <param name="addTile">The criteria used to determine which tiles are added.</param>
    /// <returns></returns>
    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkLater = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0; i < dirs.Length; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                // Check if the new tile passes the specified search criteria.
                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkLater.Enqueue(next);
                    retValue.Add(next);
                }
            }

            // Once we have checked all the checkNow tiles we move on to the checkLater tiles.
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkLater);
        }

        return retValue;
    }

    /// <summary>
    /// Highlight the given tiles by changing their color.
    /// </summary>
    /// <param name="tiles"></param>
    public void SelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
    }

    /// <summary>
    /// Remove highlighting from the given tiles.
    /// </summary>
    /// <param name="tiles"></param>
    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }
    #endregion

    #region Private
    /// <summary>
    /// Clear the results of the previous search.
    /// </summary>
    void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }
    #endregion
}