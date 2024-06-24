using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    public EnemyData enemyData;

    private EntityTurnManager _entityTurnManager;

    protected GridMovement gridmovement;

    private void Awake()
    {
        gridmovement = GetComponent<GridMovement>();
        _entityTurnManager = GetComponent<EntityTurnManager>();
        _entityTurnManager.OnStartTurn.AddListener(DoAction);
        Dungeon.instance.OnRoomLoaded.AddListener(SetStartLocation);
    }

    private void SetStartLocation()
    {
        transform.position = gridmovement.SetPos(enemyData.position);
    }

    private void DoAction()
    {
        StartCoroutine(ExecuteAction());
    }

    private IEnumerator ExecuteAction()
    {
        yield return new WaitForSeconds(1);
        transform.position = gridmovement.SetPos(gridmovement.GridPosition + Direction2D.GetRandomDirection());
        TurnCompleteInvoker.Invoke();
    }
}
