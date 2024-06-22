using System.Collections.Generic;
using UnityEngine;

public class EntityTurnManager : MonoBehaviour
{
    private List<ITurnComponent> _turnComponents = new List<ITurnComponent>();

    private bool _activeTurn = false;
    private bool _completedTurn = false;
    public bool completedTurn {  get { return _completedTurn; } }

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
    }

    private void CompletedAction()
    {
        if (_activeTurn == false || _completedTurn == true) return;
        print(gameObject.name + " has taken its turn");
        _completedTurn = true;
        _activeTurn = false;
    }
}
