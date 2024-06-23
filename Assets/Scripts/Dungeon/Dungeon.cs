using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dungeon : MonoBehaviour
{
    public bool debugDungeon;

    public static Dungeon instance;

    [SerializeField]
    private int _roomAmount;

    [SerializeField]
    private RoomScriptableObject _roomScriptable;
    [SerializeField]
    private GameObject _doorPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;

    private List<RoomScriptableObject> _rooms = new List<RoomScriptableObject>();

    private RoomScriptableObject _loadedRoom;
    private GameObject _loadedRoomObject;
    private List<GameObject> _enemiesInRoom = new List<GameObject>();

    private List<Vector2Int> _roomPositions = new List<Vector2Int>();

    public UnityEvent OnRoomLoaded = new UnityEvent();

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
        foreach (RoomScriptableObject room in _rooms)
        {
            Vector2Int roomPos = room.roomPosition;
            //set doors to true corresponding to existing rooms in that direction
            if (GetRoomFromPosition(roomPos + new Vector2Int(0, 1)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(0, 1)).doors.bottom = true;
            }
            if (GetRoomFromPosition(roomPos + new Vector2Int(1, 0)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(1, 0)).doors.left = true;
            }
            if (GetRoomFromPosition(roomPos + new Vector2Int(0, -1)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(0, -1)).doors.top = true;
            }
            if (GetRoomFromPosition(roomPos + new Vector2Int(-1, 0)) != null)
            {
                GetRoomFromPosition(roomPos + new Vector2Int(-1, 0)).doors.right = true;
            }

            //Decide room type
            if (roomPos == Vector2Int.zero)
            {
                room.roomType = RoomType.EMPTY;
            }
            else if (Random.Range(0, 2) == 1)
            {
                room.roomType = RoomType.ENEMY;
                Vector2Int randomPos = room.possibleEnemySpawns.RandomItem();
                room.enemies.Add(new EnemyData(_enemyPrefab, randomPos));
            }
            else
            {
                room.roomType = RoomType.EMPTY;
            }
        }

        LoadRoom(_roomPositions[0]);
    }

    private void AddRoom(Vector2Int pos)
    {
        RoomScriptableObject rso = ScriptableObject.CreateInstance<RoomScriptableObject>();
        rso = Instantiate(_roomScriptable);
        rso.roomPosition = pos;

        _roomPositions.Add(pos);
        _rooms.Add(rso);
    }

    public void LoadRoom(Vector2Int roomPos)
    {
        if (_loadedRoomObject != null)
        {
            UnloadRoom();
        }
        //instantiate room and set correct room data
        _loadedRoomObject = Instantiate(_roomScriptable.prefab);
        _loadedRoom = GetRoomFromPosition(roomPos);
        _loadedRoomObject.GetComponent<Room>().roomData = _loadedRoom;

        //place doors
        Vector2Int size = _loadedRoom.roomSize;
        if (_loadedRoom.doors.top)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(0, 6, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(0, 1);
        }
        if (_loadedRoom.doors.bottom) {
            GameObject door = Instantiate(_doorPrefab, new Vector3(0, -6, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(0, -1);
        }
        if (_loadedRoom.doors.right)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(8, 0, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(1, 0);
        }
        if (_loadedRoom.doors.left)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(-8, 0, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(-1, 0);
        }

        //load enemies
        if (_loadedRoom.roomType == RoomType.ENEMY)
        {
            foreach (EnemyData enemy in _loadedRoom.enemies)
            {
                Vector3 enemyPos = new Vector3(enemy.position.x, enemy.position.y, 0);
                _enemiesInRoom.Add(Instantiate(enemy.prefab, enemyPos, Quaternion.identity));
            }
        }
        OnRoomLoaded.Invoke();
    }

    public void UnloadRoom()
    {
        Destroy(_loadedRoomObject);
        _loadedRoomObject = null;

        //clear all entities from room
        int enemyAmount = _enemiesInRoom.Count;
        for (int i = 0; i < enemyAmount; i++)
        {
            Destroy(_enemiesInRoom[i]);
        }
        _enemiesInRoom.Clear();
        TurnManager.instance.ClearEntities();
    }

    public RoomScriptableObject GetRoomFromPosition(Vector2Int pos)
    {
        foreach(RoomScriptableObject room in _rooms)
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
