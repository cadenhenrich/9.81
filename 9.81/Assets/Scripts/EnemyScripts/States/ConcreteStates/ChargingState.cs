
using System.Collections;
using UnityEngine;

public class ChargingState : EnemyBaseState
{
    private bool isCharging = false;

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
        Debug.Log("Entered Charging State");
        if (!isCharging)
        {
            stateManager.StartCoroutine(Wait(stateManager));
        }
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
    }

    private IEnumerator Wait(EnemyStateManager stateManager)
    {
        isCharging = true;
        stateManager.SaveAttackDirection();
        yield return new WaitForSeconds(stateManager.enemyScriptableObject._typesOfAttacks[stateManager.currentAttack].attackChargeUpInSeconds);
        stateManager.ChangeState(stateManager.attackState);
        isCharging = false;
    }
}
