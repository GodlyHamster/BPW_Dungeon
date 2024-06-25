using System.Collections.Generic;
using UnityEngine;

public class DungeonGrid : MonoBehaviour
{
    public static DungeonGrid instance;

    private Dictionary<GameObject, Vector2Int> _entitiesOnGrid = new Dictionary<GameObject, Vector2Int>();

    private void Awake()
    {
        instance = this;
    }

    public void AddEntityToGrid(GameObject obj, Vector2Int pos)
    {
        _entitiesOnGrid.Add(obj, pos);
    }

    public void RemoveEntityFromGrid(GameObject obj)
    {
        _entitiesOnGrid.Remove(obj);
    }

    public Vector2Int GetPos(GameObject obj)
    {
        return _entitiesOnGrid[obj];
    }

    public GameObject GetObjectFromPos(Vector2Int pos)
    {
        return _entitiesOnGrid.KeyFromValue(pos);
    }

    public bool GridContainsObject(Vector2Int pos)
    {
        return _entitiesOnGrid.KeyFromValue(pos) != null;
    }
    
    public Vector3 SetPos(GameObject obj, Vector2Int position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        Vector2Int currentPos = _entitiesOnGrid[obj];
        if (hit.collider == null)
        {
            _entitiesOnGrid[obj] = position;
            return new Vector3(position.x, position.y, 0);
        }
        if (hit.collider.gameObject.tag == "Door" && obj.tag == "Player")
        {
            _entitiesOnGrid[obj] = new Vector2Int(-currentPos.x, -currentPos.y);
            Door door = hit.collider.gameObject.GetComponent<Door>();
            Dungeon.instance.LoadRoom(door.linksToRoom);
        }
        return new Vector3(_entitiesOnGrid[obj].x, _entitiesOnGrid[obj].y, 0);
    }
}
