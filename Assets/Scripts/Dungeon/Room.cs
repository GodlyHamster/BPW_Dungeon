using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Room(Vector2Int pos)
    {
        roomPosition = pos;
    }

    public RoomData roomData;

    public GameObject prefab;
    public RoomType roomType;

    public Vector2Int roomPosition;

    [Header("Doors")]
    public Door topDoor;
    public Door rigthDoor;
    public Door bottomDoor;
    public Door leftDoor;
}
