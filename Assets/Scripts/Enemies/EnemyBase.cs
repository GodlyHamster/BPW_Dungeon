using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    public EnemyData enemyData;

    private EntityTurnManager _entityTurnManager;

    private Health _health;

    private void Awake()
    {
        _entityTurnManager = GetComponent<EntityTurnManager>();
        _health = GetComponent<Health>();

        _entityTurnManager.OnStartTurn.AddListener(DoAction);
        Dungeon.instance.OnRoomLoaded.AddListener(SetStartLocation);
        _health.OnDeath.AddListener(delegate { Dungeon.instance.RemoveEnemy(this); });
    }

    private void OnDestroy()
    {
        Dungeon.instance.RemoveEnemy(this);
        DungeonGrid.instance.RemoveEntityFromGrid(this.gameObject);
        TurnManager.instance.RemoveEntity(_entityTurnManager);
    }

    private void SetStartLocation()
    {
        DungeonGrid.instance.AddEntityToGrid(gameObject, enemyData.position);
        transform.position = DungeonGrid.instance.SetPos(gameObject, enemyData.position);
    }

    private void DoAction()
    {
        StartCoroutine(ExecuteAction());
    }

    protected virtual IEnumerator ExecuteAction()
    {
        yield return new WaitForSeconds(1);
        Vector2Int newPos = DungeonGrid.instance.GetPos(gameObject) + Direction2D.GetRandomDirection();
        transform.position = DungeonGrid.instance.SetPos(gameObject, newPos);
        TurnCompleteInvoker.Invoke();
    }
}
