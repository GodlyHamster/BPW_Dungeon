using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dungeon : MonoBehaviour
{
    public bool debugDungeon;

    public static Dungeon instance;

    [SerializeField]
    private int _roomAmount;

    [Header("Room Scriptables")]
    [SerializeField]
    private RoomScriptableObject _tutorialRoom;
    [SerializeField]
    private RoomScriptableObject _emptyRoom;
    [SerializeField]
    private RoomScriptableObject _enemyRoom;
    [SerializeField]
    private RoomScriptableObject _bossRoom;

    [Header("Room Objects")]
    [SerializeField]
    private GameObject _doorPrefab;
    [SerializeField]
    private List<GameObject> _enemyPrefabs;
    [SerializeField]
    private List<GameObject> _bossPrefabs;

    private List<RoomScriptableObject> _rooms = new List<RoomScriptableObject>();

    private RoomScriptableObject _loadedRoom;
    private GameObject _loadedRoomObject;
    private List<EnemyBase> _enemiesInRoom = new List<EnemyBase>();

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

            //Generate things based on room type
            if (room.roomType == RoomType.ENEMY)
            {
                Vector2Int randomPos = room.possibleEnemySpawns.RandomItem();
                room.enemies.Add(new EnemyData(_enemyPrefabs.RandomItem(), randomPos));
            }
            else if (room.roomType == RoomType.BOSS)
            {
                Vector2Int randomPos = room.possibleEnemySpawns.RandomItem();
                room.enemies.Add(new EnemyData(_bossPrefabs.RandomItem(), randomPos));
            }
        }

        LoadRoom(_roomPositions[0]);
    }

    private void AddRoom(Vector2Int pos)
    {
        RoomScriptableObject rso = ScriptableObject.CreateInstance<RoomScriptableObject>();

        //Decide room type
        if (pos == Vector2Int.zero)
        {
            rso = Instantiate(_tutorialRoom);
        }
        else if (_roomPositions.Count == _roomAmount - 1)
        {
            rso = Instantiate(_bossRoom);
        }
        else if (Random.Range(0, 2) == 1)
        {
            rso = Instantiate(_enemyRoom);
        }
        else
        {
            rso = Instantiate(_emptyRoom);
        }
        rso.roomPosition = pos;

        _roomPositions.Add(pos);
        _rooms.Add(rso);
    }

    public void LoadRoom(Vector2Int roomPos)
    {
        if (_loadedRoomObject != null)
        {
            SaveAndUnloadRoom();
        }
        //instantiate room and set correct room data
        _loadedRoom = GetRoomFromPosition(roomPos);
        _loadedRoomObject = Instantiate(_loadedRoom.prefab);
        _loadedRoomObject.GetComponent<Room>().roomData = _loadedRoom;

        //place doors
        Vector2Int size = _loadedRoom.roomSize;
        if (_loadedRoom.doors.top)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(0, size.y/2, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(0, 1);
        }
        if (_loadedRoom.doors.bottom) {
            GameObject door = Instantiate(_doorPrefab, new Vector3(0, -size.y/2, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(0, -1);
        }
        if (_loadedRoom.doors.right)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(size.x/2, 0, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(1, 0);
        }
        if (_loadedRoom.doors.left)
        {
            GameObject door = Instantiate(_doorPrefab, new Vector3(-size.x/2, 0, 0), Quaternion.identity, _loadedRoomObject.transform);
            door.GetComponent<Door>().linksToRoom = roomPos + new Vector2Int(-1, 0);
        }

        //load enemies
        if (_loadedRoom.roomType == RoomType.ENEMY || _loadedRoom.roomType == RoomType.BOSS)
        {
            foreach (EnemyData enemyData in _loadedRoom.enemies)
            {
                Vector3 enemyPos = new Vector3(enemyData.position.x, enemyData.position.y, 0);
                GameObject enemyObj = Instantiate(enemyData.prefab, enemyPos, Quaternion.identity, _loadedRoomObject.transform);
                EnemyBase enemy = enemyObj.GetComponent(typeof(EnemyBase)) as EnemyBase;
                enemy.enemyData = enemyData;
                _enemiesInRoom.Add(enemy);
            }
        }
        OnRoomLoaded.Invoke();
    }

    public void SaveAndUnloadRoom()
    {
        //save data
        _loadedRoom.enemies.Clear();
        foreach (EnemyBase enemy in _enemiesInRoom)
        {
            _loadedRoom.enemies.Add(enemy.GetData());
        }

        int roomIndex = _rooms.IndexOf(GetRoomFromPosition(_loadedRoom.roomPosition));
        _rooms[roomIndex] = _loadedRoom;

        //unload all
        Destroy(_loadedRoomObject);
        _loadedRoomObject = null;
        _loadedRoom = null;

        _enemiesInRoom.Clear();
        TurnManager.instance.ClearEntities();
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        if (_enemiesInRoom.Contains(enemy))
        {
            int index = _enemiesInRoom.IndexOf(enemy);
            Destroy(enemy.gameObject);
            _enemiesInRoom.RemoveAt(index);
        }
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
                else if (GetRoomFromPosition(pos).roomType == RoomType.BOSS)
                {
                    Gizmos.color = Color.black;
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
