using UnityEngine;

public class ChaseState : EnemyBaseState
{
    public override void OnStateCollisionEnter(EnemyStateManager stateManager, Collision2D collision)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnEnterState(EnemyStateManager stateManager)
    {
        stateManager.pathingAgent.PathRefreshing(true);
        if (stateManager.agroTarget == null) { return; }
        stateManager.pathingAgent.SetTarget(stateManager.agroTarget.transform);

    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
        if (stateManager.InAttackRange())
        {
            stateManager.ChangeState(stateManager.chargingState);
        }
    }

    public override void FixedUpdateState(EnemyStateManager stateManager)
    {
        stateManager.pathingAgent.UpdateMovement();
    }
}
