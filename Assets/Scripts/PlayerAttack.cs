using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttackManager.instance.AddAttack(1, DungeonGrid.instance.GetPos(gameObject));
            TurnCompleteInvoker.Invoke();
        }
    }
}
