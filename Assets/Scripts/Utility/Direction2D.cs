using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction2D
{
    public static Vector2Int[] directions =
    {
        new Vector2Int(0, 1), //up
        new Vector2Int(0, -1), //down
        new Vector2Int(-1, 0), //left
        new Vector2Int(1, 0), //right
    };

    public static Vector2Int GetRandomDirection()
    {
        return directions[Random.Range(0, directions.Length)];
    }
}
