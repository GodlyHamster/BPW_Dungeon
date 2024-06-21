using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GridMovement _gridMovement;

    private void Awake()
    {
        _gridMovement = GetComponent<GridMovement>();
    }

    private void Start()
    {
        transform.position = _gridMovement.MoveToPos(_gridMovement.GridPosition);
    }

    void Update()
    {
        Vector2Int newMoveValue = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            newMoveValue = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            newMoveValue = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            newMoveValue = new Vector2Int(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newMoveValue = new Vector2Int(1, 0);
        }

        transform.position = _gridMovement.MoveToPos(_gridMovement.GridPosition + newMoveValue);
    }
}
