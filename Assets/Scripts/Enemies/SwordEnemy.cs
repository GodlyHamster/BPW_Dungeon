using System.Collections;
using UnityEngine;

public class SwordEnemy : EnemyBase
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
            AttackScriptableObject currentAttack = Instantiate(_attacks.RandomItem());
            currentAttack.AttackTowards(currentPos, playerPos);
            AttackManager.instance.AddAttackAtPos(currentAttack, currentPos);
        }

        TurnCompleteInvoker.Invoke();
    }
}
