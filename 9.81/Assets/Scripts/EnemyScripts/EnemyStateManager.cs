using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStateManager : MonoBehaviour
{

    [HideInInspector] public GameObject[] wanderPoints;


    public EnemyBaseState currentState;

    public WanderState wanderState;
    public ChaseState chaseState;
    public ChargingState chargingState;
    public AttackState attackState;
    public AlertState alertState;
    public IdleState idleState;
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = idleState;
        currentState.OnEnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.OnEnterState(this);
    }

    public GameObject[] WanderPoints
    {
        get { return wanderPoints; }
        set { wanderPoints = value; }
    }

    
}
