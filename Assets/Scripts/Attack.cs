using System;
using UnityEngine;

[Serializable]
public class Attack
{
    public Attack()
    {
        damage = 1;
        position = Vector2Int.zero;
    }

    public Attack(int damage, Vector2Int position)
    {
        this.damage = damage;
        this.position = position;
    }

    public int damage;
    public Vector2Int position;
}
