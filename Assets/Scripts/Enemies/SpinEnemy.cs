using System.Collections;
using UnityEngine;

public class SpinEnemy : EnemyBase
{
    protected override IEnumerator ExecuteAction()
    {
        Vector2Int currentPos = DungeonGrid.instance.GetPos(gameObject);
        Vector2Int playerPos = DungeonGrid.instance.GetPos(_player);
        int playerDistance = DungeonGrid.instance.Distance(gameObject, playerPos);

        yield return new WaitForSeconds(1);

        if (playerDistance > 1)
        {
            transform.position = DungeonGrid.instance.MoveTowards(gameObject, playerPos);
        }
        else
        {
            print("attack player");
            AttackScriptableObject currentAttack = Instantiate(_attack);
            currentAttack.AttackTowards(currentPos, playerPos);
            AttackManager.instance.AddAttackAtPos(currentAttack, currentPos);
        }

        TurnCompleteInvoker.Invoke();
    }
}
