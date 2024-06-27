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
        if (_entitiesOnGrid.ContainsKey(obj)) return;
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

    public bool GridContainsObjectAt(Vector2Int pos)
    {
        return _entitiesOnGrid.KeyFromValue(pos) != null;
    }
    
    public Vector3 SetPos(GameObject thisObj, Vector2Int position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        Vector2Int currentPos = _entitiesOnGrid[thisObj];
        if (hit.collider == null)
        {
            _entitiesOnGrid[thisObj] = position;
            return new Vector3(position.x, position.y, 0);
        }
        if (hit.collider.gameObject.tag == "Door" && thisObj.tag == "Player")
        {
            _entitiesOnGrid[thisObj] = new Vector2Int(-currentPos.x, -currentPos.y);
            Door door = hit.collider.gameObject.GetComponent<Door>();
            Dungeon.instance.LoadRoom(door.linksToRoom);
        }
        return new Vector3(_entitiesOnGrid[thisObj].x, _entitiesOnGrid[thisObj].y, 0);
    }

    public Vector3 MoveTowards(GameObject thisObj, Vector2Int destination)
    {
        Vector2Int currentPos = _entitiesOnGrid[thisObj];
        if (currentPos.x < destination.x)
        {
            return (SetPos(thisObj, currentPos + Vector2Int.right));
        }
        else if (currentPos.x > destination.x)
        {
            return (SetPos(thisObj, currentPos + Vector2Int.left));
        }
        else if (currentPos.y < destination.y)
        {
            return (SetPos(thisObj, currentPos + Vector2Int.up));
        }
        else if (currentPos.y > destination.y)
        {
            return (SetPos(thisObj, currentPos + Vector2Int.down));
        }
        return new Vector3(currentPos.x, currentPos.y);
    }

    public int Distance(GameObject thisObject, Vector2Int pos)
    {
        int distance = Mathf.Abs(_entitiesOnGrid[thisObject].x - pos.x) + Mathf.Abs(_entitiesOnGrid[thisObject].y - pos.y);
        return distance;
    }
}
