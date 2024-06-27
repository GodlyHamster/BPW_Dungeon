using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class AttackScriptableObject : ScriptableObject
{
    public int damage;
    public Vector2Int[] positions;
    public int executesInTurns;

    public void AttackTowards(Vector2 from, Vector2 towards)
    {
        AttackRotation atr = AttackDirection(from, towards);
        if (atr == AttackRotation.NONE) return;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = positions[i].Rotate((float)atr);
        }
    }

    private AttackRotation AttackDirection(Vector2 from, Vector2 towards)
    {
        Vector2 normal = (towards - from).normalized;
        Vector2Int normalInt = new Vector2Int(Mathf.RoundToInt(normal.x), Mathf.RoundToInt(normal.y));
        switch (normalInt)
        {
            case Vector2Int v when v.Equals(Vector2Int.right):
                return AttackRotation.RIGHT;
            case Vector2Int v when v.Equals(Vector2Int.up):
                return AttackRotation.UP;
            case Vector2Int v when v.Equals(Vector2Int.left):
                return AttackRotation.LEFT;
            case Vector2Int v when v.Equals(Vector2Int.down):
                return AttackRotation.DOWN;
            default:
                return AttackRotation.NONE;
        }
    }
}
