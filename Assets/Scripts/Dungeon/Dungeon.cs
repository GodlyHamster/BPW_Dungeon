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
            if (_roomPositions.Contains(room.roomPosition + new Vector2Int(0, 1)))
            {
                room.topDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(0, 1));
            }
            if (_roomPositions.Contains(room.roomPosition + new Vector2Int(1, 0)))
            {
                room.rigthDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(1, 0));
            }
            if (_roomPositions.Contains(room.roomPosition + new Vector2Int(0, -1)))
            {
                room.bottomDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(0, -1));
            }
            if (_roomPositions.Contains(room.roomPosition + new Vector2Int(-1, 0)))
            {
                room.leftDoor.linksToRoom = GetRoomFromPosition(room.roomPosition + new Vector2Int(-1, 0));
            }

            if (room.topDoor.linksToRoom == null) room.topDoor.gameObject.SetActive(false);
            if (room.rigthDoor.linksToRoom == null) room.rigthDoor.gameObject.SetActive(false);
            if (room.bottomDoor.linksToRoom == null) room.bottomDoor.gameObject.SetActive(false);
            if (room.leftDoor.linksToRoom == null) room.leftDoor.gameObject.SetActive(false);
        }

        LoadRoom(_rooms[0]);
    }

    private void AddRoom(Vector2Int pos)
    {
        RoomData newRoom = new RoomData();
        newRoom = _roomPrefabs[0].roomData.Clone();
        newRoom.roomPosition = pos;

        _roomPositions.Add(pos);
        _rooms.Add(newRoom);
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
        return new RoomData();
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
