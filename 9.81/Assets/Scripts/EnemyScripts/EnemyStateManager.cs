using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyBaseState currentState;

    public WanderState wanderState;
    public ChaseState chaseState;
    public ChargingState chargingState;
    public AttackState attackState;
    public AlertState alertState;
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = wanderState;
        currentState.OnEnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.OnEnterState(this);
    }
}
