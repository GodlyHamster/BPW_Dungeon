using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GridMovement))]
public class ITurnEntity : MonoBehaviour
{
    private bool _finishedTurn;
    public bool finishedTurn { get { return _finishedTurn; } }

    private bool _didAction;

    private GridMovement _gridMovement;

    private void Awake()
    {
        _gridMovement = GetComponent<GridMovement>();
    }

    private void Start()
    {
        _gridMovement.OnMoved.AddListener(() => _didAction = true);
    }

    public void ResetTurn()
    {
        _finishedTurn = false;
        _didAction = false;
    }

    public void StartTurn()
    {
        StartCoroutine(DoTurn());
    }

    private IEnumerator DoTurn()
    {
        yield return new WaitUntil(() => _didAction == true);
        _finishedTurn = true;
    }
}
