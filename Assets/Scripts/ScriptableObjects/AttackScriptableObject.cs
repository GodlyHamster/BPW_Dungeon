using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class AttackScriptableObject : ScriptableObject
{
    public int damage;
    public Vector2Int[] positions;
    public int executesInTurns;
}
