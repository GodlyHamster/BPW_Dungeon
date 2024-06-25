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
        Dungeon.instance.OnRoomLoaded.AddListener(ClearAllAttacks);
    }

    public bool AddAttack(Attack attack)
    {
        if (_attacks.ContainsKey(attack)) return false;
        GameObject newAttackObj = Instantiate(_attackPreview, new Vector3(attack.position.x, attack.position.y), Quaternion.identity);
        _attacks.Add(attack, newAttackObj);
        return true;
    }

    private void ClearAllAttacks()
    {
        if (_attacks.Count == 0) return;
        int attacksToClear = _attacks.Count - 1;
        for (int i = attacksToClear; i >= 0; i--)
        {
            Destroy(_attacks.ElementAt(i).Value);
            _attacks.RemoveAt(i);
        }
        _attacks.Clear();
    }

    private void UpdateAttackSpaces()
    {
        //TODO update spaces
    }
}
