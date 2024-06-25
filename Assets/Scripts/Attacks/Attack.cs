using System;
using UnityEngine;

[Serializable]
public class Attack
{
    public Attack()
    {
        damage = 1;
        position = Vector2Int.zero;
        executesInTurns = 1;
    }

    public Attack(int damage, Vector2Int position, int executesInTurns)
    {
        this.damage = damage;
        this.position = position;
        this.executesInTurns = executesInTurns;
    }

    public int damage;
    public Vector2Int position;
    public int executesInTurns;
}
