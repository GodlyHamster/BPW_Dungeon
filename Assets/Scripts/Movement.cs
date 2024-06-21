using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TurnCompleteInvoker.Invoke();
        }
    }
}
