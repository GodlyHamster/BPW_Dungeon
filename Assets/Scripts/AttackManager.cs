using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager instance;

    [SerializeField]
    private GameObject _attackPreview;

    //perhaps change it to a Vector2Int that is connected to a valuepair <Attack, GameObject>
    private Dictionary<Attack, GameObject> _attacks = new Dictionary<Attack, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TurnManager.instance.OnRoundEnd.AddListener(UpdateAttackSpaces);
        Dungeon.instance.OnRoomLoaded.AddListener(delegate { ClearAllAttacks(true); });
    }

    public bool AddAttack(Attack attack)
    {
        if (_attacks.ContainsKey(attack)) return false;
        GameObject newAttackObj = Instantiate(_attackPreview, new Vector3(attack.position.x, attack.position.y), Quaternion.identity);
        _attacks.Add(attack, newAttackObj);
        return true;
    }

    private void ClearAllAttacks(bool clearActive)
    {
        if (_attacks.Count == 0) return;
        int attacksToClear = _attacks.Count - 1;
        for (int i = attacksToClear; i >= 0; i--)
        {
            if (!clearActive && _attacks.ElementAt(i).Key.executesInTurns >= 0) continue;
            Destroy(_attacks.ElementAt(i).Value);
            _attacks.RemoveAt(i);
        }
        if (clearActive) _attacks.Clear();
    }

    private void UpdateAttackSpaces()
    {
        if (_attacks.Count == 0) return;
        foreach (KeyValuePair<Attack, GameObject> item in _attacks)
        {
            Attack currentAttack = item.Key;
            if (currentAttack.executesInTurns <= 0 && DungeonGrid.instance.GridContainsObject(currentAttack.position))
            {
                GameObject entityInPos = DungeonGrid.instance.GetObjectFromPos(currentAttack.position);
                if (entityInPos.TryGetComponent<Health>(out Health hp))
                {
                    hp.TakeDamage(currentAttack.damage);
                }
            }
            currentAttack.executesInTurns -= 1;
        }
        ClearAllAttacks(false);
    }
}
