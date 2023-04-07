
using UnityEngine;

public class WanderState : EnemyBaseState
{
    public override void OnCollisionEnter(EnemyStateManager stateManager, Collision collision)
    {
        stateManager.ChangeState(stateManager.alertState);
    }

    public override void OnEnterState(EnemyStateManager stateManager)
    {
        throw new System.NotImplementedException();
        /* 
         
         */
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        throw new System.NotImplementedException();
        /* 
         move enemy towards
         */
    }
}
