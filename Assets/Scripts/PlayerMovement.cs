using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GridMovement gridMovement = new GridMovement(new Vector2Int(0, 0));

    private void Start()
    {
        transform.position = gridMovement.MoveToPos(gridMovement.GridPosition);
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

        transform.position = gridMovement.MoveToPos(gridMovement.GridPosition + newMoveValue);
    }
}
