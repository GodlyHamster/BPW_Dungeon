using UnityEngine.Events;

public interface ITurnComponent
{
    public UnityEvent OnTurnComplete { get; }
}
