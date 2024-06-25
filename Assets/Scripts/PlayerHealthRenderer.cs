using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHealthRenderer : MonoBehaviour
{
    [SerializeField]
    private Health _playerHealth;
    [SerializeField]
    private GameObject _healthSegmentPrefab;

    private List<GameObject> _healthSegments = new List<GameObject>();

    private void Awake()
    {
        _playerHealth.OnDamageTaken.AddListener(UpdateDisplay);
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int health = _playerHealth.health;

        int newSegments = health - _healthSegments.Count;
        for (int j = 0; j < Mathf.Abs(newSegments); j++)
        {
            if (newSegments > 0)
            {
                _healthSegments.Add(Instantiate(_healthSegmentPrefab, transform));
            }
            else if (newSegments < 0)
            {
                if (_healthSegments.Count <= 0) continue;
                Destroy(_healthSegments.Last());
                _healthSegments.RemoveLast();
            }
        }
    }
}
