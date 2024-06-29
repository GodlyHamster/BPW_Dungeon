using System.Collections;
using UnityEngine;

public class ZeusBoss : EnemyBase
{
    private int _waitTurns = 0;

    protected override IEnumerator ExecuteAction()
    {
        Vector2Int currentPos = DungeonGrid.instance.GetPos(gameObject);
        Vector2Int playerPos = DungeonGrid.instance.GetPos(_player);
        int playerDistance = DungeonGrid.instance.Distance(gameObject, playerPos);

        yield return new WaitForSeconds(1);

        if (_waitTurns <= 0)
        {
            AttackScriptableObject currentAttack;
            if (playerDistance <= 1)
            {
                currentAttack = Instantiate(_attacks[1]);
                _waitTurns = 2;
            }
            else
            {
                currentAttack = Instantiate(_attacks[0]);
                _waitTurns = 1;
            }
            AttackManager.instance.AddAttack(currentAttack);
        }
        else
        {
            _waitTurns--;
        }

        TurnCompleteInvoker.Invoke();
    }

    private void OnDestroy()
    {
        GameMenuManager.instance.defeatedBoss = true;
    }
}
