using UnityEngine;
using Pathfinding;

public class ChaseState : EnemyBaseState
{
    public override void OnCollisionEnter(EnemyStateManager stateManager, Collision collision)
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnterState(EnemyStateManager stateManager)
    {
        stateManager.pathingAgent.PathRefreshing(true);

    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        //throw new System.NotImplementedException();
    }

    //void OnPathComplete(Path p)
    //{
    //    Debug.Log("Got path");
    //    if (!p.error) 
    //    {
    //        Debug.Log("Set path");
    //        path = p;
    //        currentWaypoint = 0;
    //    }
    //}

    //public override void UpdateState(EnemyStateManager stateManager)
    //{
    //    currentTimerValue += Time.deltaTime;
    //    if (currentTimerValue >= timerDuration)
    //    {
    //        currentTimerValue = 0.0f;
    //        if (stateManager.seeker.IsDone())
    //        {
    //            stateManager.seeker.StartPath(stateManager.rb.position, stateManager.target.position, OnPathComplete);
    //        }

    //    }
    //    if (path == null) { return; }
    //    if (currentWaypoint >= path.vectorPath.Count) 
    //    { 
    //        stateManager.reachedEndOfPath = true;
    //        return;
    //    } else
    //    {
    //        stateManager.reachedEndOfPath = false;
    //    }

    //    Vector2 direction = new Vector2((path.vectorPath[currentWaypoint].x - stateManager.rb.position.x), 0).normalized;
    //    Vector2 force = direction * stateManager.enemyScriptableObject.speed * Time.deltaTime;

    //    stateManager.rb.AddForce(force);

    //    float distance = Vector2.Distance(stateManager.rb.position, path.vectorPath[currentWaypoint]);

    //    if (distance < stateManager.NEXT_WAYPOINT_DISTANCE)
    //    {
    //        currentWaypoint += 1;
    //    }


    //}

    public override void FixedUpdateState(EnemyStateManager stateManager)
    {
        
        stateManager.pathingAgent.UpdateMovement();
    }
}
