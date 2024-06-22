using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    private EntityTurnManager _entityTurnManager;

    private void Awake()
    {
        _entityTurnManager = GetComponent<EntityTurnManager>();
        _entityTurnManager.OnStartTurn.AddListener(DoAction);
    }

    private void DoAction()
    {
        StartCoroutine(ExecuteAction());
    }

    private IEnumerator ExecuteAction()
    {
        yield return new WaitForSeconds(1);
        TurnCompleteInvoker.Invoke();
    }
}
