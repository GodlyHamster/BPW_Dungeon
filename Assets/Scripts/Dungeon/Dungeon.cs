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
    private GameObject _roomPrefab;
    [SerializeField]
    private GameObject _doorPrefab;

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
            Vector2Int roomPos = room.roomPosition;
            //set doors to true corresponding to existing rooms in that direction
            if (GetRoomFromPosition(roomPos + new Vector2Int(0, 1)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(0, 1)).bottomDoor = true;
            }
            if (GetRoomFromPosition(roomPos + new Vector2Int(1, 0)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(1, 0)).leftDoor = true;
            }
            if (GetRoomFromPosition(roomPos + new Vector2Int(0, -1)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(0, -1)).topDoor = true;
            }
            if (GetRoomFromPosition(roomPos + new Vector2Int(-1, 0)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(-1, 0)).rigthDoor = true;
            }

            //Decide room type
            if (Random.Range(0, 2) == 1)
            {
                room.roomType = RoomType.ENEMY;
            }
        }

        LoadRoom(_roomPositions[0]);
    }

    private void AddRoom(Vector2Int pos)
    {
        RoomData roomData = new RoomData();
        roomData.roomPosition = pos;

        _roomPositions.Add(pos);
        _rooms.Add(roomData);
    }

    public void LoadRoom(Vector2Int roomPos)
    {
        if (_loadedRoomObject != null)
        {
            Destroy(_loadedRoomObject);
            _loadedRoomObject = null;
        }
        //instantiate room and set correct room data
        _loadedRoomObject = Instantiate(_roomPrefab);
        _loadedRoom = GetRoomFromPosition(roomPos);
        _loadedRoomObject.GetComponent<Room>().roomData = _loadedRoom;

        //place doors
        Vector2Int size = _loadedRoom.roomSize;
        if (_loadedRoom.topDoor)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(0, 6, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(0, 1);
        }
        if (_loadedRoom.bottomDoor) {
            GameObject door = Instantiate(_doorPrefab, new Vector3(0, -6, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(0, -1);
        }
        if (_loadedRoom.rigthDoor)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(8, 0, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(1, 0);
        }
        if (_loadedRoom.leftDoor)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(-8, 0, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(-1, 0);
        }
    }

    public RoomData GetRoomFromPosition(Vector2Int pos)
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
                if (pos == _loadedRoom.roomPosition)
                {
                    Gizmos.color = Color.green;
                }
                else if (GetRoomFromPosition(pos).roomType == RoomType.ENEMY)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.blue;
                }
                Gizmos.DrawCube(new Vector3(pos.x, pos.y, 0), Vector3.one);
            }
        }
    }
}
