
using UnityEngine;

public class AttackState : EnemyBaseState
{
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
        stateManager.ChangeState(stateManager.chaseState);
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
    }
}
