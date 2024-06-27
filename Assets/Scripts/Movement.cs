using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    private EntityTurnManager _entityTurnManager;

    private void Awake()
    {
        _entityTurnManager = GetComponent<EntityTurnManager>();
    }

    private void Start()
    {
        DungeonGrid.instance.AddEntityToGrid(gameObject, Vector2Int.zero);
        transform.position = DungeonGrid.instance.SetPos(gameObject, Vector2Int.zero);
    }

    void Update()
    {
        if (_entityTurnManager.activeTurn is false) return;

        Vector2Int newMoveValue = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            newMoveValue = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            newMoveValue = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            newMoveValue = new Vector2Int(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newMoveValue = new Vector2Int(1, 0);
        }

        if (newMoveValue != Vector2Int.zero)
        {
            Vector2Int newPos = DungeonGrid.instance.GetPos(gameObject) + newMoveValue;
            if (!DungeonGrid.instance.GridContainsObjectAt(newPos))
            {
                transform.position = DungeonGrid.instance.SetPos(gameObject, newPos);
                TurnCompleteInvoker.Invoke();
            }
        }
    }
}
