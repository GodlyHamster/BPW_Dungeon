using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    private EntityTurnManager _entityTurnManager;

    protected GridMovement gridmovement;

    private void Awake()
    {
        _entityTurnManager = GetComponent<EntityTurnManager>();
        _entityTurnManager.OnStartTurn.AddListener(DoAction);
    }

    private void Start()
    {
        transform.position = gridmovement.SetPos(new Vector2Int(0, 2));
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
