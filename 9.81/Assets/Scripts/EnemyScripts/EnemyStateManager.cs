using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(PathingAgent))]
[RequireComponent(typeof(AttackAgent))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyDamager))]
public class EnemyStateManager : MonoBehaviour
{

    public EnemyScriptableObject enemyScriptableObject;
    [SerializeField] public GameObject exclaimationPoint;
    [HideInInspector] public GameObject agroTarget;
    [HideInInspector] public PathingAgent pathingAgent;
    [HideInInspector] public AttackAgent attackAgent;

    public GameObject[] wanderPoints;
    
    [HideInInspector] public int currentWanderPoint;
    [HideInInspector] public Transform target;
    [HideInInspector] public Rigidbody2D rb;

    private int layerMask = 1 << 7;

    public EnemyBaseState currentState;
    public Animator anim;

    public WanderState wanderState = new WanderState();
    public ChaseState chaseState = new ChaseState();
    public ChargingState chargingState = new ChargingState();
    public AttackState attackState = new AttackState();
    public AlertState alertState = new AlertState();
    public IdleState idleState = new IdleState();
    public int currentAttack = 0;
    private Vector2 attackDirection;
    [HideInInspector] public bool canAttack;

    EnemyDamager enemyDamager;
    EnemyHealth enemyHealth;

    // Start is called before the first frame update
    private void Awake()
    {
        canAttack = true;
        currentState = wanderState;
        rb = GetComponent<Rigidbody2D>();
        agroTarget = GameObject.FindGameObjectWithTag(enemyScriptableObject.targetTag);
        attackAgent = GetComponent<AttackAgent>();
        pathingAgent = GetComponent<PathingAgent>();
        pathingAgent.SetRefreshRate(enemyScriptableObject.reactionTime);
        pathingAgent.SetSpeed(enemyScriptableObject.speed);
        pathingAgent.SetJumpHeight(enemyScriptableObject.jumpHeight);

        enemyDamager = GetComponent<EnemyDamager>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyDamager.SetDamage(enemyScriptableObject._typesOfAttacks[currentAttack].damage);
        enemyHealth.SetKnockBack(enemyScriptableObject.xKnockbackScale, enemyScriptableObject.yKnockbackScale);
        enemyHealth.SetHealth(enemyScriptableObject.hitsCanTake);
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
        // and the raycast must not hit level terrain
        if (!(Vector2.Distance(transform.position, agroTarget.transform.position) <= enemyScriptableObject.detectionRadius)) { return false; }

        Vector2 direction = agroTarget.transform.position - transform.position;
        if (Physics2D.Raycast(transform.position, direction, enemyScriptableObject.detectionRadius, layerMask)) { return false; }

        anim.SetInteger("AnimState", 1);
        return true;

    }

    public bool InAttackRange()
    {
        if (Vector2.Distance(transform.position, agroTarget.transform.position) > enemyScriptableObject._typesOfAttacks[currentAttack].fireRange) { return false; }

        return true;
    }

    public GameObject[] WanderPoints
    {
        get { return wanderPoints; }
        set { wanderPoints = value; }
    }

    public void SaveAttackDirection ()
    {
        attackDirection = new Vector2(agroTarget.transform.position.x, agroTarget.transform.position.y);
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

    //currently will only do lunging melee attacks
    public void ExecuteAttack(AttackScriptableObject attackScriptableObject)
    {
        
        attackAgent.ExecuteMovementAttack(rb, attackDirection, attackScriptableObject);
    }
}
