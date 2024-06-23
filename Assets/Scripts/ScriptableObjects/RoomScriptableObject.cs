using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "ScriptableObjects/Dungeon/Room", order = 1)]
public class RoomScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public RoomType roomType;

    public Vector2Int roomPosition;
    public Vector2Int roomSize;

    [Header("Doors")]
    public DoorsData doors = new DoorsData();

    [Header("Enemy")]
    public List<EnemyData> enemies = new List<EnemyData>();
    public List<Vector2Int> possibleEnemySpawns = new List<Vector2Int>();
}
