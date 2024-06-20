using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public static Dungeon instance;

    [SerializeField]
    private int _roomAmount;

    [SerializeField]
    private Room[] _roomPrefabs;

    private List<Room> _rooms = new List<Room>();

    private Room _loadedRoom;

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
        _roomPositions.Add(startPos);

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
            _roomPositions.Add(randomDir);
        }
    }

    private void LoadRoom()
    {
        if (_loadedRoom != null)
        {
            Destroy(_loadedRoom.transform.gameObject);
            _loadedRoom = null;
        }

        Room randomRoom = _rooms[Random.Range(0, _rooms.Count)];
        Instantiate(randomRoom.prefab);
        _loadedRoom = randomRoom;
    }
}
