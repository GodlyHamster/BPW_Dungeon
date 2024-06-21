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
    }
    private void Start()
    {
        Dungeon.instance.OnRoomLoaded.AddListener(GetTurnEntities);
    }

    private void GetTurnEntities()
    {
        StopAllCoroutines();
        _entityTurnManagers = FindObjectsOfType<EntityTurnManager>().ToList();
        StartCoroutine(Round());
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
        foreach (var turnEntity in _entityTurnManagers)
        {
            //reset all entities their turn
            turnEntity.ResetTurn();
        }
        //restart the round
        yield return null;
        StartCoroutine(Round());
    }
}
