using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
[RequireComponent(typeof(NavMeshAgent),typeof(Waypoints))]

public abstract class enemyAI : MonoBehaviour
{
    [SerializeField] EnemyStats statsScriptable;
    Enemy enemy;
    protected Transform target;
    [Tooltip("Range for player when is walking/running")]
    [SerializeField] float chaseRange = 5f;
    [Tooltip("Range for player when is crouching")]
    [SerializeField] float crouchRange = 3f;
    [Tooltip("How fast zombie is rotating when detect player")]
    [SerializeField] float rotateSpeed = 5f;
    
    //Is set to infinity so there wont be any problem when AI script is initializing
    float distanceToTarget = Mathf.Infinity;
    

    NavMeshAgent navMeshAgent;
    Waypoint[] patrollingWaypoints;
    Vector3 startingPosition;

    int lastWaypoint;
    int currentWaypoint = 0;


    float patrollingSpeedBase;
    float chaseSpeedBase;

    [SerializeField] float nextActionBreakBase = 5f;
    [SerializeField] float tryingCatchTargetBase = 5f;
    public float tryingCatchTarget;

    public bool isPatrolling;
    public bool isOnBreak = false;
    public bool isProvoked = false;
    public bool isAttacking = false;

    [Tooltip("Distance in which zombie is attacking its target")]
    float attackingRange;
    Animator animator;

    bool isOnStartingPosition = true;
    AIState enemyState  = AIState.Idling;

    protected virtual void OnEnable(){
        enemy.onDamageTaken += ProvokeTrigger;
    }
    protected virtual void OnDisable() {
        enemy.onDamageTaken -= ProvokeTrigger;
    }

    protected virtual void Awake() {
        InitStats();
        navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.stoppingDistance = attackingRange;
        startingPosition = gameObject.transform.position;
        tryingCatchTarget = tryingCatchTargetBase;
        lastWaypoint = currentWaypoint;
        patrollingWaypoints = GetComponent<Waypoints>().waypoints;
        enemy = GetComponent<Enemy>();
    }
    protected virtual void InitStats()
    {
        patrollingSpeedBase = statsScriptable.baseSpeed * 0.15f;
        chaseSpeedBase = statsScriptable.baseSpeed;
        attackingRange = statsScriptable.baseAttackRange;

    }
    protected virtual void Start()
    {
        //OVERRIDE IN EVERY SUBCLASS
        animator = enemy.getAnimator;
        target = PlayerHealth.instance.transform;
        Assert.IsNotNull(target,gameObject.name + " not set target");
        if(patrollingWaypoints.Length>1)
        {
            isPatrolling = true;
        }else
        {
            isPatrolling = false;
        }
    }
    protected virtual void Update()
    {
        distanceToTarget = Vector3.Distance(
            new Vector3(transform.position.x,0,transform.position.z),
            new Vector3(target.position.x,0,target.position.z));
        if(isPatrolling == true && isOnBreak == false  && isProvoked == false)
        {
            PatrollingArea();
        }

        if(isProvoked == true)
        {
            EngageTarget();
        }
        if(isPatrolling == false && isOnStartingPosition == false && isProvoked == false && isOnBreak == false)
        {
            BackToStartingPos();
        }
        
        if(PlayerHealth.instance.IsDead == false)
        {
            if(PlayerMovement.instance.Crouch == false)
            {
                if(distanceToTarget <= chaseSpeedBase)
                {
                    isProvoked = true;
                }
            }
            else
            {
                if(distanceToTarget <= crouchRange)
                {
                    isProvoked = true;
                }
            }
        }
        
    }

    
    public virtual void EngageTarget()
    {
        if(PlayerHealth.instance.IsDead == true)
        {
            ChaseAnimateState(false);
            isProvoked = false;
            AttackTarget(false);
            StartCoroutine(BreakOnNextAction(patrollingWaypoints[lastWaypoint].Position));
            return;
        }

        
        //time ended or distance is over possible and player not catched
        //Debug.Log(tryingCatchTarget);
        //Debug.Log(distanceToTarget);
        if(tryingCatchTarget <= 0)
        {
            //Debug.Log(tryingCatchTarget + "lower than 0");
            isProvoked = false;
            tryingCatchTarget = tryingCatchTargetBase;
            navMeshAgent.ResetPath();
            ChaseAnimateState(false);
            
            if(isPatrolling)
            {
                Debug.Log("Going to last waypoint...");
                StartCoroutine(BreakOnNextAction(patrollingWaypoints[lastWaypoint].Position));
                return;
            }
            else
            {
                Debug.Log("Going back to starting position...");
                StartCoroutine(BreakOnNextAction(startingPosition));
                return;
            }
        }
        FaceTarget();
        
        if(distanceToTarget >= navMeshAgent.stoppingDistance && isAttacking == false)
        {
            ChaseTarget();

            //Debug.Log(tryingCatchTarget);

            tryingCatchTarget-=Time.deltaTime;
        }
        else if(distanceToTarget < navMeshAgent.stoppingDistance || isAttacking == true)
        { 
            Debug.Log("Get to point");  
            navMeshAgent.ResetPath();
            if(isAttacking == false)
            {
                isAttacking = true;
                //navMeshAgent.isStopped =true;
                AttackTarget(true);
            
                if(tryingCatchTarget!=tryingCatchTargetBase) tryingCatchTarget = tryingCatchTargetBase;
            }
            else
            {
                if(distanceToTarget > navMeshAgent.stoppingDistance + attackingRange)
                {
                    isAttacking = false;
                }
            }
        }
        
        

           
        
    }

    public virtual void ProvokeTrigger()
    {
        isProvoked = true;
    }
    public virtual void ChaseTarget()
    {
        isOnStartingPosition = false;
        AttackTarget(false);
        PatrollingAnimateState(false);
        ChaseAnimateState(true);
        navMeshAgent.SetDestination(target.position);
    }
    public virtual void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*rotateSpeed);
    }
    public virtual void BackToStartingPos()
    {
        if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && enemyState == 0) return;
        Debug.Log("Heading towards starting position!");
        Debug.Log("...");
        if(enemyState != 0)
        {
            PatrollingAnimateState(true);
        }
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Debug.Log("Reached starting position!");
            PatrollingAnimateState(false);
            isOnStartingPosition = true;
        }
    }
    public virtual void PatrollingArea()
    {
        if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && enemyState == 0) return;
        
        if(enemyState != 0)
        {
            isOnStartingPosition = false;
            PatrollingAnimateState(true);
        }
        if(currentWaypoint == patrollingWaypoints.Length-1)
        {
            //Debug.Log("Patrolling reset...");

            currentWaypoint = 0;
        }else
        {
            lastWaypoint = currentWaypoint;
            currentWaypoint+=1;
        }

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            //Debug.Log("Patrolling next waypoint...");
            PatrollingAnimateState(false);
            StartCoroutine(BreakOnNextAction(patrollingWaypoints[currentWaypoint].Position));

        }
    }
    public virtual IEnumerator BreakOnNextAction(Vector3 newDestination)
    {
        //Debug.Log("Break...");

        isOnBreak = true;
        IdleAnimteState(true);
        yield return new WaitForSeconds(nextActionBreakBase);
        navMeshAgent.destination = newDestination;
        //Debug.Log(navMeshAgent.destination);
        isOnBreak = false;


    }
    protected void AttackTarget(bool isAttacking)
    {

        animator.SetBool("attack",isAttacking);
        
      //Debug.Log("Attacking!");  
    }
    protected void PatrollingAnimateState(bool state)
    {
        //Debug.Log("Patrolling settings...");

        if(state == true)
        {
            navMeshAgent.speed = patrollingSpeedBase;
            enemyState = AIState.Patrolling;
        }
        animator.SetBool("patrol",state);
    }
    protected void ChaseAnimateState(bool state)
    {
        if(state)
        {
            enemyState = AIState.Chasing;
            navMeshAgent.speed = chaseSpeedBase;
        }
        animator.SetBool("chase",state);
    }
    protected void IdleAnimteState(bool state)
    {
        if(state)
        {
            enemyState = AIState.Idling;
        }
        animator.SetTrigger("idle");
    }
   
    protected virtual void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,chaseRange);
        Gizmos.DrawWireSphere(transform.position,crouchRange);

    }
}
