using System.Collections;
using UnityEngine;

public class AlertState : EnemyBaseState
{
    private bool isAlert;

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
        if (!isAlert)
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
        isAlert = true;
        stateManager.exclaimationPoint.SetActive(true);
        yield return new WaitForSeconds(1);
        stateManager.ChangeState(stateManager.chaseState);
        stateManager.exclaimationPoint.SetActive(false);
        isAlert = false;
    }
}
