using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityTurnManager : MonoBehaviour
{
    private List<ITurnComponent> _turnComponents = new List<ITurnComponent>();

    private bool _activeTurn = false;
    public bool activeTurn { get { return _activeTurn; } }
    private bool _completedTurn = false;
    public bool completedTurn {  get { return _completedTurn; } }

    public UnityEvent OnStartTurn = new UnityEvent();

    private void Awake()
    {
        var components = gameObject.GetComponents(typeof(ITurnComponent));
        foreach (var comp in components)
        {
            var TurnComponent = comp as ITurnComponent;
            _turnComponents.Add(TurnComponent);
            TurnComponent.OnTurnComplete.AddListener(CompletedAction);
        }
    }

    public void ResetTurn()
    {
        _activeTurn = false;
        _completedTurn = false;
    }

    public void SetActiveTurn(bool activateTurn)
    {
        _activeTurn = activateTurn;
        if (activeTurn) OnStartTurn.Invoke();
    }

    private void CompletedAction()
    {
        if (_activeTurn == false || _completedTurn == true) return;
        _completedTurn = true;
        _activeTurn = false;
    }
}
