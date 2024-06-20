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

    List<Room> _rooms = new List<Room>();

    private Room _loadedRoom;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        for (int i = 0; i < _roomAmount; i++)
        {
            _rooms.Add(_roomPrefabs[Random.Range(0, _roomPrefabs.Length)]);
        }
        LoadRoom();
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
