using UnityEngine;
using UnityEngine.Events;

public class GridMovement : MonoBehaviour
{
    public GridMovement(Vector2Int position)
    {
        _gridPosition = position;
    }

    [SerializeField]
    private Vector2Int _startPosition;

    private Vector2Int _gridPosition;
    public Vector2Int GridPosition { get { return _gridPosition; } }

    public UnityEvent OnMoved = new UnityEvent();

    private void Start()
    {
        _gridPosition = _startPosition;
    }

    public Vector3 MoveToPos(Vector2Int position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider == null)
        {
            _gridPosition = position;
            return new Vector3(position.x, position.y, 0);
        }
        if (hit.collider.gameObject.tag == "Door")
        {
            _gridPosition = new Vector2Int(-_gridPosition.x, -_gridPosition.y);
            Door door = hit.collider.gameObject.GetComponent<Door>();
            Dungeon.instance.LoadRoom(door.linksToRoom);
        }
        OnMoved.Invoke();
        return new Vector3(_gridPosition.x, _gridPosition.y, 0);
    }
}
