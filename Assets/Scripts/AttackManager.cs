using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager instance;

    [SerializeField]
    private GameObject _attackPreview;

    private List<Attack> _attacks = new List<Attack>();
    private List<GameObject> _attackObjects = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TurnManager.instance.OnRoundEnd.AddListener(UpdateAttackSpaces);
        Dungeon.instance.OnRoomLoaded.AddListener(ClearAllAttacks);
    }

    public void AddAttack(int damage, Vector2Int pos)
    {
        _attacks.Add(new Attack(damage, pos));
        _attackObjects.Add(Instantiate(_attackPreview, new Vector3(pos.x, pos.y), Quaternion.identity));
    }

    private void ClearAllAttacks()
    {
        print(_attacks.Count);
        if (_attacks.Count == 0) return;
        int attacksToClear = _attacks.Count;
        for (int i = 0; i < attacksToClear; i++)
        {
            Destroy(_attackObjects[i]);
            _attackObjects.RemoveAt(i);
            _attacks.RemoveAt(i);
        }
    }

    private void UpdateAttackSpaces()
    {
        print(_attacks.Count);
        if (_attacks.Count == 0) return;
        int attacksToClear = _attacks.Count;
        for (int i = 0; i < attacksToClear; i++)
        {
            Destroy(_attackObjects[i]);
            _attackObjects.RemoveAt(i);
            _attacks.RemoveAt(i);
        }
    }
}
