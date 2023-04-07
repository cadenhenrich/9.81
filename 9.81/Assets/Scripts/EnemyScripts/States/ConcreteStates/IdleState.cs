
using UnityEngine;

public class IdleState : EnemyBaseState
{
    public override void OnCollisionEnter(EnemyStateManager stateManager, Collision collision)
    {
        stateManager.ChangeState(stateManager.alertState);
    }

    public override void OnEnterState(EnemyStateManager stateManager)
    {
        throw new System.NotImplementedException();
        //Wait for random amount of time 
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        throw new System.NotImplementedException();
        //try to check if player is in sight
    }

    
}
