using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement
{
    public GridMovement(Vector2Int position)
    {
        _gridPosition = position;
    }

    private Vector2Int _gridPosition;
    public Vector2Int GridPosition { get { return _gridPosition; } }

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
        return new Vector3(_gridPosition.x, _gridPosition.y, 0);
    }
}
