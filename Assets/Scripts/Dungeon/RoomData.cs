using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomData
{
    public RoomData() { }
    public RoomData(GameObject p, RoomType r, Vector2Int pos, DoorsData doors, List<EnemyData> enemydata) { 
        prefab = p;
        roomType = r;
        roomPosition = pos;
        this.doors = doors;
        enemies = enemydata;
    }

    public GameObject prefab;
    public RoomType roomType;

    public Vector2Int roomPosition;
    public Vector2Int roomSize;

    [Header("Doors")]
    public DoorsData doors = new DoorsData();

    [Header("Enemy")]
    public List<EnemyData> enemies = new List<EnemyData>();
    public List<Vector2Int> possibleEnemySpawns = new List<Vector2Int>();

    public RoomData Clone()
    {
        RoomData clone = new RoomData(prefab, roomType, roomPosition, doors, enemies);
        return clone;
    }
}
