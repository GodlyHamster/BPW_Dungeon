using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager instance;

    [SerializeField]
    private GameObject _attackPreview;

    private Dictionary<AttackScriptableObject, List<GameObject>> _attacks = new Dictionary<AttackScriptableObject, List<GameObject>>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TurnManager.instance.OnRoundEnd.AddListener(UpdateAttackSpaces);
        Dungeon.instance.OnRoomLoaded.AddListener(delegate { ClearAllAttacks(true); });
    }

    public bool AddAttack(AttackScriptableObject attack)
    {
        if (_attacks.ContainsKey(attack)) return false;
        AttackScriptableObject attackToAdd = Instantiate(attack);
        _attacks.Add(attackToAdd, new List<GameObject>());
        foreach (Vector2Int pos in attack.positions)
        {
            _attacks[attackToAdd].Add(Instantiate(_attackPreview, new Vector3(pos.x, pos.y), Quaternion.identity));
        }
        return true;
    }

    public bool AddAttackAtPos(AttackScriptableObject attack, Vector2Int addition)
    {
        if (_attacks.ContainsKey(attack)) return false;
        AttackScriptableObject attackToAdd = Instantiate(attack);
        _attacks.Add(attackToAdd, new List<GameObject>());
        foreach (Vector2Int pos in attack.positions)
        {
            _attacks[attackToAdd].Add(Instantiate(_attackPreview, new Vector3(pos.x + addition.x, pos.y + addition.y), Quaternion.identity));
        }
        return true;
    }

    private void ClearAllAttacks(bool clearActive)
    {
        if (_attacks.Count == 0) return;
        int attacksToClear = _attacks.Count - 1;
        for (int i = attacksToClear; i >= 0; i--)
        {
            if (!clearActive && _attacks.ElementAt(i).Key.executesInTurns >= 0) continue;
            foreach (var obj in _attacks.ElementAt(i).Value)
            {
                Destroy(obj);
            }
            _attacks.RemoveAt(i);
        }
        if (clearActive) _attacks.Clear();
    }

    private void UpdateAttackSpaces()
    {
        if (_attacks.Count == 0) return;
        foreach (KeyValuePair<AttackScriptableObject, List<GameObject>> item in _attacks)
        {
            AttackScriptableObject currentAttack = item.Key;
            if (currentAttack.executesInTurns <= 0)
            {
                foreach (var pos in currentAttack.positions)
                {
                    GameObject entityInPos = DungeonGrid.instance.GetObjectFromPos(pos);
                    if (entityInPos == null) continue;
                    if (entityInPos.TryGetComponent<Health>(out Health hp))
                    {
                        hp.TakeDamage(currentAttack.damage);
                    }
                }
            }
            currentAttack.executesInTurns -= 1;
        }
        ClearAllAttacks(false);
    }
}
