using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void ClearEntities()
    {
        StopAllCoroutines();
        _entityTurnManagers.Clear();
    }

    private void GetEntitiesAndStart()
    {
        StartCoroutine(GatherEntitiesAndStart());
    }

    private IEnumerator GatherEntitiesAndStart()
    {
        yield return new WaitUntil(() => _entityTurnManagers.Count == 0);
        _entityTurnManagers = FindObjectsOfType<EntityTurnManager>().ToList();
        StartCoroutine(StartNextRound());
        yield return null;
    }

    private IEnumerator Round()
    {
        foreach (var turnEntity in _entityTurnManagers)
        {
            //start entities turn and wait until completed
            turnEntity.SetActiveTurn(true);
            print("waiting for " + turnEntity.gameObject.name + " to take turn...");
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
        StartCoroutine(Round());
        yield return null;
    }
}
