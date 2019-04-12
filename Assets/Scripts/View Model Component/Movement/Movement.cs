using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Movement : MonoBehaviour
{
    #region Properties
    public int range;
    protected Unit unit;
    /// <summary>
    /// Assists with animation during the traversal of a path.
    /// </summary>
    protected Transform jumper;
    #endregion

    #region MonoBehaviour
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        jumper = transform.Find("Jumper");
    }
    #endregion

    #region Public
    /// <summary>
    /// Determines which tiles are reachable from the unit's starting tile.
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }

    /// <summary>
    /// Handles the animation of traversing the path.
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public abstract IEnumerator Traverse(Tile tile);
    #endregion

    #region Protected
    /// <summary>
    /// Checks that the next tile is within range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }

    /// <summary>
    /// Removes any tiles with content. For example we can move through a tile with an ally but cannot end our turn there.
    /// </summary>
    /// <param name="tiles"></param>
    protected virtual void Filter(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 1; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

    protected virtual IEnumerator Turn(Directions dir)
    {
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

        // When rotating between North and West, we must make an exception so it looks like the unit
        // rotates the most efficient way (since 0 and 360 are treated the same)
        if (Mathf.Approximately(t.startValue.y, 0f) && Mathf.Approximately(t.endValue.y, 270f))
            t.startValue = new Vector3(t.startValue.x, 360f, t.startValue.z);
        else if (Mathf.Approximately(t.startValue.y, 270) && Mathf.Approximately(t.endValue.y, 0))
            t.endValue = new Vector3(t.startValue.x, 360f, t.startValue.z);

        unit.dir = dir;

        while (t != null)
            yield return null;
    }
    #endregion
}