using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;
using UnityEngine.UI;

public class EnemyStateManager : MonoBehaviour
{

    public EnemyScriptableObject enemyScriptableObject;
    [SerializeField] public GameObject exclaimationPoint;
    [HideInInspector] public GameObject agroTarget;
    [HideInInspector] public PathingAgent pathingAgent;

    public GameObject[] wanderPoints;
    
    [HideInInspector] public int currentWanderPoint;
    [HideInInspector] public Transform target;
    [HideInInspector] public Rigidbody2D rb;

    private int layerMask = 1 << 7;

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
        currentState = wanderState;
        rb = GetComponent<Rigidbody2D>();
        agroTarget = GameObject.FindGameObjectWithTag(enemyScriptableObject.targetTag);
        pathingAgent = GetComponent<PathingAgent>();
        pathingAgent.SetRefreshRate(enemyScriptableObject.reactionTime);
        pathingAgent.SetSpeed(enemyScriptableObject.speed);
        pathingAgent.SetJumpHeight(enemyScriptableObject.jumpHeight);
    }

    private void Start()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnStateCollisionEnter(this, collision);
    }

    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.OnEnterState(this);
    }

    public bool CanSeeTarget()
    {
        // agroTarget has to be within the detection radius
        if (!(Vector2.Distance(transform.position, agroTarget.transform.position) <= enemyScriptableObject.detectionRadius)) { return false; }
        Vector2 direction = transform.position- agroTarget.transform.position;
        if (Physics2D.Raycast(transform.position, direction, enemyScriptableObject.detectionRadius, layerMask)) { return false; }
        return true;

    }

    public GameObject[] WanderPoints
    {
        get { return wanderPoints; }
        set { wanderPoints = value; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyScriptableObject.detectionRadius);
        Gizmos.color = Color.red;
        if (agroTarget != null)
        {
            //Ray ray = new Ray(transform.position, transform.position - agroTarget.transform.position);
            Vector2 direction = (transform.position- agroTarget.transform.position).normalized;
            Gizmos.DrawLine(transform.position, agroTarget.transform.position);
            //Gizmos.DrawRay(ray);
        }
        
    }
}
