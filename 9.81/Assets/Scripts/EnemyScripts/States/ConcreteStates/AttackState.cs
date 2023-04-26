
using System.Collections;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    private bool isWaiting = false;

    public override void FixedUpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateCollisionEnter(EnemyStateManager stateManager, Collision2D collision)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnEnterState(EnemyStateManager stateManager)
    {
        Debug.Log("Entered Attack State");
        stateManager.ExecuteAttack(stateManager.enemyScriptableObject._typesOfAttacks[stateManager.currentAttack]);
        stateManager.currentAttack = (stateManager.currentAttack + 1) % stateManager.enemyScriptableObject._typesOfAttacks.Length;
        if (!isWaiting)
        {
            stateManager.StartCoroutine(Wait(stateManager));
        }
        stateManager.ChangeState(stateManager.chaseState);
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
    }
    private IEnumerator Wait(EnemyStateManager stateManager)
    {
        isWaiting = true;
        stateManager.canAttack = false;
        yield return new WaitForSeconds(stateManager.enemyScriptableObject._typesOfAttacks[stateManager.currentAttack].attackCooldownInSeconds);
        stateManager.canAttack = true;
        isWaiting = false;
    }
}
