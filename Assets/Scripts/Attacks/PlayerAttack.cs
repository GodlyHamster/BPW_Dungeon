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
            AttackScriptableObject currentAttack = Instantiate(_attack);
            AttackRotation atr = MouseDir();
            if (atr == AttackRotation.NONE) return;

            for (int i = 0; i < _attack.positions.Length; i++)
            {
                currentAttack.positions[i] = currentAttack.positions[i].Rotate((float)atr);
            }
            Vector2Int currentPos = DungeonGrid.instance.GetPos(gameObject);
            AttackManager.instance.AddAttackAtPos(currentAttack, currentPos);
            TurnCompleteInvoker.Invoke();
        }
    }

    public AttackRotation MouseDir()
    {
        Vector2 mouse = Input.mousePosition;
        Vector2 character = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 normal = (mouse - character).normalized;
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
