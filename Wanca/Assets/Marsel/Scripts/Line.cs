using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class Line
{
    Orientation orientation;
    Vector2Int coordinates;
    public Line (Orientation orientation,Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}
public enum Orientation
{
    horizontal =0,
    vertical = 1,
}