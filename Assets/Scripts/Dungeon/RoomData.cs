using System;
using UnityEngine;

[Serializable]
public class RoomData
{
    public RoomData() { }
    public RoomData(GameObject p, RoomType r, Vector2Int pos, Door top, Door right, Door down, Door left) { 
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

    [Header("Doors")]
    public Door topDoor;
    public Door rigthDoor;
    public Door bottomDoor;
    public Door leftDoor;

    public RoomData Clone()
    {
        RoomData clone = new RoomData(prefab, roomType, roomPosition, topDoor, rigthDoor, bottomDoor, leftDoor);
        return clone;
    }
}
