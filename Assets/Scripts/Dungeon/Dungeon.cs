using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public bool debugDungeon;

    public static Dungeon instance;

    [SerializeField]
    private int _roomAmount;

    [SerializeField]
    private Room[] _roomPrefabs;

    private List<RoomData> _rooms = new List<RoomData>();

    private RoomData _loadedRoom;
    private GameObject _loadedRoomObject;

    private List<Vector2Int> _roomPositions = new List<Vector2Int>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateRoomPositions();
    }

    //Dungeon-room position generation algorithm
    private void GenerateRoomPositions()
    {
        Vector2Int startPos = Vector2Int.zero;
        AddRoom(startPos);

        Vector2Int currentPos = startPos;
        while (_roomPositions.Count < _roomAmount)
        {
            Vector2Int randomDir = currentPos + Direction2D.GetRandomDirection();
            if (_roomPositions.Contains(randomDir))
            {
                currentPos = _roomPositions.RandomItem();
                continue;
            }

            currentPos = randomDir;

            AddRoom(currentPos);
        }

        GenerateRooms();
    }

    private void GenerateRooms()
    {
        foreach (RoomData room in _rooms)
        {
            print(room.roomPosition);
            if (GetRoomFromPosition(room.roomPosition + new Vector2Int(0, 1)) != null)
            {
                room.topDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(0, 1));
            }
            if (GetRoomFromPosition(room.roomPosition + new Vector2Int(1, 0)) != null)
            {
                room.rigthDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(1, 0));
            }
            if (GetRoomFromPosition(room.roomPosition + new Vector2Int(0, -1)) != null)
            {
                room.bottomDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(0, -1));
            }
            if (GetRoomFromPosition(room.roomPosition + new Vector2Int(-1, 0)) != null)
            {
                room.leftDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(-1, 0));
            }
        }

        LoadRoom(_rooms[0]);
    }

    private void AddRoom(Vector2Int pos)
    {
        RoomData roomData = new RoomData();
        roomData = _roomPrefabs[0].roomData.Clone();
        roomData.roomPosition = pos;

        _roomPositions.Add(pos);
        _rooms.Add(roomData);
    }

    public void LoadRoom(RoomData room)
    {
        if (_loadedRoomObject != null)
        {
            Destroy(_loadedRoomObject);
            _loadedRoom = null;
            _loadedRoomObject = null;
        }
        _loadedRoomObject = Instantiate(room.prefab);
        _loadedRoom = room;
    }

    private RoomData GetRoomFromPosition(Vector2Int pos)
    {
        foreach(RoomData room in _rooms)
        {
            if (room.roomPosition == pos)
            {
                return room;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        if (debugDungeon)
        {
            foreach (Vector2Int pos in _roomPositions) { 
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(pos.x, pos.y, 0), Vector3.one);
            }
        }
    }
}
