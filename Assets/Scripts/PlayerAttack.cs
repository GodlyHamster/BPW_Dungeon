using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    private GridMovement _gridMovement;

    private void Awake()
    {
        _gridMovement = GetComponent<GridMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttackManager.instance.AddAttack(1, _gridMovement.GridPosition);
            TurnCompleteInvoker.Invoke();
        }
    }
}
