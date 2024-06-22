using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    private EntityTurnManager _entityTurnManager;
    private GridMovement _gridMovement;

    private void Awake()
    {
        _entityTurnManager = GetComponent<EntityTurnManager>();
        _gridMovement = GetComponent<GridMovement>();
    }

    private void Start()
    {
        transform.position = _gridMovement.MoveToPos(_gridMovement.GridPosition);
    }

    void Update()
    {
        if (_entityTurnManager.activeTurn is false) return;

        Vector2Int newMoveValue = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            newMoveValue = new Vector2Int(0, 1);
            TurnCompleteInvoker.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            newMoveValue = new Vector2Int(-1, 0);
            TurnCompleteInvoker.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            newMoveValue = new Vector2Int(0, -1);
            TurnCompleteInvoker.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newMoveValue = new Vector2Int(1, 0);
            TurnCompleteInvoker.Invoke();
        }

        transform.position = _gridMovement.MoveToPos(_gridMovement.GridPosition + newMoveValue);
    }
}
