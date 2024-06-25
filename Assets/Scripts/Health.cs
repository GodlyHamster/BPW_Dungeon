using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth;
    [SerializeField]
    private int _health;

    public int health { get { return _health; } }

    public UnityEvent OnDamageTaken = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();

    public void TakeDamage(int damage)
    {
        _health -= damage;
        OnDamageTaken.Invoke();
        if (_health <= 0)
        {
            OnDeath.Invoke();
        }
    }

    public void Heal(int healAmount)
    {
        _health += healAmount;
        if (_health > _maxHealth ) _health = _maxHealth;
    }
}
