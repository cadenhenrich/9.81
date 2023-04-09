using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class EnemyStateManager : MonoBehaviour
{

    public EnemyScriptableObject enemyScriptableObject;
    [HideInInspector] public PathingAgent pathingAgent;

    [HideInInspector] public GameObject[] wanderPoints;
    [HideInInspector] public Transform target;
    [HideInInspector] public Rigidbody2D rb;
    

    public EnemyBaseState currentState;

    public WanderState wanderState = new WanderState();
    public ChaseState chaseState = new ChaseState();
    public ChargingState chargingState = new ChargingState();
    public AttackState attackState = new AttackState();
    public AlertState alertState = new AlertState();
    public IdleState idleState = new IdleState();
    
    // Start is called before the first frame update
    private void Awake()
    {
        currentState = chaseState;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag(enemyScriptableObject.targetTag).GetComponent<Transform>();
        pathingAgent = GetComponent<PathingAgent>();
        pathingAgent.SetRefreshRate(enemyScriptableObject.reactionTime);
        pathingAgent.SetSpeed(enemyScriptableObject.speed);
        pathingAgent.SetTarget(target);
        pathingAgent.SetJumpHeight(enemyScriptableObject.jumpHeight);
        currentState.OnEnterState(this);
    }


    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
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
