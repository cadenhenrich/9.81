
using UnityEngine;

public class WanderState : EnemyBaseState
{
    public override void FixedUpdateState(EnemyStateManager stateManager)
    {
        stateManager.pathingAgent.UpdateMovement();
        if (stateManager.currentState == stateManager.wanderState && stateManager.pathingAgent.ReachedTarget())
        {
            
            stateManager.ChangeState(stateManager.idleState);
        }
    }

    public override void OnStateCollisionEnter(EnemyStateManager stateManager, Collision2D collision)
    {
        if (collision.gameObject.CompareTag(stateManager.agroTarget.gameObject.tag)) 
        { 
            stateManager.ChangeState(stateManager.alertState);
        }
    }

    public override void OnEnterState(EnemyStateManager stateManager)
    {
        stateManager.pathingAgent.PathRefreshing(true);
        stateManager.pathingAgent.SetTarget(stateManager.wanderPoints[stateManager.currentWanderPoint].transform);
        stateManager.currentWanderPoint = (stateManager.currentWanderPoint + 1) % stateManager.wanderPoints.Length;
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        if (stateManager.CanSeeTarget())
        {
            stateManager.ChangeState(stateManager.alertState);
        }
    }
}
