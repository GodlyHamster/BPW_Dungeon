using System;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public EnemyData(GameObject prefab, Vector2Int pos)
    {
        this.prefab = prefab;
        position = pos;
    }

    public GameObject prefab;
    public Vector2Int position; 
    public Health health;
}
