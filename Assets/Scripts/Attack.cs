using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour, ITurnComponent
{
    private UnityEvent TurnCompleteInvoker = new UnityEvent();
    public UnityEvent OnTurnComplete { get { return TurnCompleteInvoker; } }

    [SerializeField]
    private GameObject _attackPreview;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(_attackPreview, transform.position + new Vector3(1, 0), Quaternion.identity);
            TurnCompleteInvoker.Invoke();
        }
    }
}
