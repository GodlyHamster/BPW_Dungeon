using System;
using UnityEngine;

[Serializable]
public class RoomData
{
    public RoomData() { }
    public RoomData(GameObject p, RoomType r, Vector2Int pos, bool top, bool right, bool down, bool left) { 
        prefab = p;
        roomType = r;
        roomPosition = pos;
        topDoor = top;
        rigthDoor = right;
        bottomDoor = down;
        leftDoor = left;
    }

    public GameObject prefab;
    public RoomType roomType;

    public Vector2Int roomPosition;
    public Vector2Int roomSize;

    [Header("Doors")]
    public bool topDoor;
    public bool rigthDoor;
    public bool bottomDoor;
    public bool leftDoor;

    public RoomData Clone()
    {
        RoomData clone = new RoomData(prefab, roomType, roomPosition, topDoor, rigthDoor, bottomDoor, leftDoor);
        return clone;
    }
}
