using System.Collections.Generic;
using UnityEngine;

public class DungeonGrid : MonoBehaviour
{
    public static DungeonGrid instance;

    private Dictionary<Vector2Int, GameObject> _entitiesOnGrid = new Dictionary<Vector2Int, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void AddEntityToGrid(Vector2Int pos, GameObject obj)
    {
        _entitiesOnGrid.Add(pos, obj);
    }

    public void RemoveEntityFromGrid(Vector2Int pos)
    {
        _entitiesOnGrid.Remove(pos);
    }
}
