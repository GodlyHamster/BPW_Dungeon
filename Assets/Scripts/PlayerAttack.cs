using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    private EntityTurnManager _entityTurnManager;

    private void Awake()
    {
        _entityTurnManager = GetComponent<EntityTurnManager>();
    }

    private void Update()
    {
        if (_entityTurnManager.activeTurn is false) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2Int currentPos = DungeonGrid.instance.GetPos(gameObject);
            AttackManager.instance.AddAttack(new Attack(1, currentPos + new Vector2Int(1, 0), 1));
            TurnCompleteInvoker.Invoke();
        }
    }
}
