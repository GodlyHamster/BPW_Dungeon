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
        DungeonGrid.instance.AddEntityToGrid(gameObject, Vector2Int.zero);
        transform.position = _gridMovement.SetPos(_gridMovement.GridPosition);
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

        if (newMoveValue != Vector2Int.zero)
        {
            //transform.position = _gridMovement.SetPos(_gridMovement.GridPosition + newMoveValue);
            transform.position = DungeonGrid.instance.SetPos(gameObject, DungeonGrid.instance.GetPos(gameObject) + newMoveValue);
        }
    }
}
