using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    private List<EntityTurnManager> _entityTurnManagers = new List<EntityTurnManager>();

    private void Awake()
    {
        instance = this;
        Dungeon.instance.OnRoomLoaded.AddListener(GetEntitiesAndStart);
    }

    public void ClearEntities()
    {
        StopAllCoroutines();
        _entityTurnManagers.Clear();
        print(_entityTurnManagers.Count);
    }

    private void GetEntitiesAndStart()
    {
        _entityTurnManagers = FindObjectsOfType<EntityTurnManager>().ToList();
        foreach (var entity in _entityTurnManagers)
        {
            print(entity.gameObject.name);
        }
        StartCoroutine(StartNextRound());
    }

    private IEnumerator Round()
    {
        print(_entityTurnManagers.Count);
        foreach (var turnEntity in _entityTurnManagers)
        {
            print(turnEntity.gameObject.name);
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
        foreach (var turnEntity in _entityTurnManagers)
        {
            //reset all entities their turn
            print(turnEntity.gameObject.name);
            turnEntity.ResetTurn();
        }
        //restart the round
        yield return null;
        StartCoroutine(Round());
    }
}
