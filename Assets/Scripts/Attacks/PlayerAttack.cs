using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    [SerializeField]
    private AttackScriptableObject _attack;

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
            AttackScriptableObject currentAttack = Instantiate(_attack);

            Vector2 characterScreenPos = Camera.main.WorldToScreenPoint(new Vector3(currentPos.x, currentPos.y));

            currentAttack.AttackTowards(characterScreenPos, Input.mousePosition);
            AttackManager.instance.AddAttackAtPos(currentAttack, currentPos);
            TurnCompleteInvoker.Invoke();
        }
    }
}
