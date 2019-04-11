using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the location of each tile on the board.
/// </summary>
[System.Serializable]
public struct Point
{
    // Fields
    public int x;
    public int y;

    // Constructor
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // Overloading operators
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }

    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.x - p2.x, p1.y - p2.y);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }

    // Object Overloads
    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }

    public bool Equals(Point p)
    {
        return x == p.x && y == p.y;
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }
}