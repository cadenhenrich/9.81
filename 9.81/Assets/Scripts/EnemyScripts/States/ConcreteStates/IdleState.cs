
using System.Collections;
using UnityEngine;

public class IdleState : EnemyBaseState
{
    private bool isWaiting = false;
    private float secondsToWait;
    public override void FixedUpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateCollisionEnter(EnemyStateManager stateManager, Collision2D collision)
    {
        //stateManager.ChangeState(stateManager.alertState);
    }

    public override void OnEnterState(EnemyStateManager stateManager)
    {
        
        RangeData range = stateManager.enemyScriptableObject.RandomIdleTimeInSecondsWhileWandering;
        secondsToWait = Random.Range(range.minSeconds, range.maxSeconds);
        if (!isWaiting)
        {
            stateManager.StartCoroutine(Wait(stateManager));
        }
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
        //try to check if player is in sight
    }

    private IEnumerator Wait(EnemyStateManager stateManager)
    {
        isWaiting = true;
        yield return new WaitForSeconds(secondsToWait);
        stateManager.ChangeState(stateManager.wanderState);
        isWaiting = false;
    }
    
}
