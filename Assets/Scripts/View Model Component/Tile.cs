using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    #region Fields / Properties
    public Point pos;
    public Vector2 center { get { return new Vector2(pos.x, pos.y); } }
    public GameObject content;
    /// <summary>
    /// Stores the tile that was traversed to reach this tile.
    /// </summary>
    [HideInInspector] public Tile prev;
    /// <summary>
    /// The number of tiles that have been traversed to reach this tile.
    /// </summary>
    [HideInInspector] public int distance;
    #endregion

    #region Public
    /// <summary>
    /// Load a tile at a given Point.
    /// </summary>
    /// <param name="p"></param>
    public void Load(Point p)
    {
        pos = p;
        Match();
    }

    public void Load(Vector2 v)
    {
        Load(new Point((int)v.x, (int)v.y));
    }
    #endregion

    #region Private
    /// <summary>
    /// Visually reflect changes to a tile's position.
    /// </summary>
    void Match()
    {
        transform.localPosition = new Vector2(pos.x, pos.y);
        transform.localScale = new Vector2(1, 1);
    }
    #endregion
}