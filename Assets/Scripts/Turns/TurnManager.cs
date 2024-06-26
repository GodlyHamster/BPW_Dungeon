using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    private List<EntityTurnManager> _entityTurnManagers = new List<EntityTurnManager>();

    public UnityEvent OnRoundEnd = new UnityEvent();

    private void Awake()
    {
        instance = this;
        Dungeon.instance.OnRoomLoaded.AddListener(GetEntitiesAndStart);
    }

    public void RemoveEntity(EntityTurnManager entity)
    {
        _entityTurnManagers.Remove(entity);
    }

    public void ClearEntities()
    {
        StopAllCoroutines();
        _entityTurnManagers.Clear();
    }

    private void GetEntitiesAndStart()
    {
        StartCoroutine(GatherEntitiesAndStart());
    }

    private bool RemoveAllEmpty()
    {
        for (int i = 0; i < _entityTurnManagers.Count; i++)
        {
            if (_entityTurnManagers[i] == null)
            {
                print("removed emtpy");
                _entityTurnManagers.RemoveAt(i);
            }
        }
        return true;
    }

    private IEnumerator GatherEntitiesAndStart()
    {
        yield return new WaitUntil(() => _entityTurnManagers.Count == 0);
        _entityTurnManagers = FindObjectsOfType<EntityTurnManager>().ToList();
        _entityTurnManagers = _entityTurnManagers.OrderByDescending(etm => etm.turnPriority).ToList();
        StartCoroutine(StartNextRound());
        yield return null;
    }

    private IEnumerator Round()
    {
        foreach (var turnEntity in _entityTurnManagers)
        {
            //start entities turn and wait until completed
            turnEntity.SetActiveTurn(true);
            yield return new WaitUntil(() => turnEntity.completedTurn == true);
        }
        yield return null;
        StartCoroutine(StartNextRound());
    }

    private IEnumerator StartNextRound()
    {
        OnRoundEnd.Invoke();
        foreach (var turnEntity in _entityTurnManagers)
        {
            //reset all entities their turn
            turnEntity.ResetTurn();
        }
        //restart the round
        yield return new WaitUntil(() => RemoveAllEmpty());
        StartCoroutine(Round());
        yield return null;
    }
}
