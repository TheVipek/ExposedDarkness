using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.VersionControl;
public class enemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRangeBase = 5f;
    [SerializeField] float crouchRangeBase = 3f;
    [SerializeField] float turnSpeedBase = 5f;
    

    /*[Tooltip("Layer on which sphere cast has to be")]
    [SerializeField] LayerMask layerMask;
    int layerMaskValue;
    */

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    Vector3 startingPosition;
    Waypoint[] patrollingWaypoints;
    int lastWaypoint;
    int currentWaypoint = 0;

    [Tooltip("This is something like global variable that defines whether this gameobject WILL patrol anything or not ")]
    [SerializeField] bool isPatrolling;
    [SerializeField] float patrollingSpeedBase = 0.7f;
    [SerializeField] float chaseSpeedBase = 4f;

    [SerializeField] float nextActionBreakBase = 5f;
    [SerializeField] float tryingCatchTargetBase = 5f;
    float tryingCatchTarget;
    bool isOnBreak = false;

    EnemyAttack enemyAttack;
    Animator animator;
    bool isProvoked;

    bool isOnStartingPosition = true;
    AIState enemyState  = AIState.Idling;

    

    void Start()
    {
        //layerMaskValue = layerMask.value;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
        target = PlayerHealth.instance.transform;
        startingPosition = gameObject.transform.position;
        tryingCatchTarget = tryingCatchTargetBase;
        lastWaypoint = currentWaypoint;
        patrollingWaypoints = GetComponent<Waypoints>().waypoints;
        if(patrollingWaypoints.Length>1)
        {
            isPatrolling = true;
            //Debug.Log(gameObject.name +" has +1 waypoints. Patrolling: "+isPatrolling);
        }else
        {
            isPatrolling = false;
            //Debug.Log(gameObject.name +" has <1 waypoints ,if you want this zombie to patrol set at least 2 waypoints! Patrolling: "+isPatrolling);
        }
    }
    void Update()
    {
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
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        
        if(PlayerMovement.instance.Crouch == false)
        {
            if(distanceToTarget <= chaseRangeBase)
                {
                    isProvoked = true;
                }
        }
        else
        {
            if(distanceToTarget <= crouchRangeBase)
            {
                isProvoked = true;
            }
        }
        
    }

    
    private void EngageTarget()
    {
        //time ended or distance is over possible and player not catched
        //Debug.Log(tryingCatchTarget);
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
        if(distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();

            //Debug.Log(tryingCatchTarget);

            tryingCatchTarget-=Time.deltaTime;
        }
        else if(distanceToTarget < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped =true;
            navMeshAgent.ResetPath();
            AttackTarget(true);
            if(tryingCatchTarget!=tryingCatchTargetBase) tryingCatchTarget = tryingCatchTargetBase;
        }
        

           
        
    }

    public void ProvokeTrigger()
    {
        isProvoked = true;
    }
    private void ChaseTarget()
    {
        isOnStartingPosition = false;
        AttackTarget(false);
        PatrollingAnimateState(false);
        ChaseAnimateState(true);
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget(bool isAttacking)
    {
        animator.SetBool("attack",isAttacking);
        Debug.Log("Attacking!");
    }
    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*turnSpeedBase);
    }
    
    private void BackToStartingPos()
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
    private void PatrollingArea()
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
            Debug.Log("Patrolling next waypoint...");
            PatrollingAnimateState(false);
            StartCoroutine(BreakOnNextAction(patrollingWaypoints[currentWaypoint].Position));

        }
    }
    private void PatrollingAnimateState(bool state)
    {
        //Debug.Log("Patrolling settings...");

        if(state == true)
        {
            navMeshAgent.speed = patrollingSpeedBase;
            enemyState = AIState.Patrolling;
        }
        animator.SetBool("patrol",state);
    }
    private void ChaseAnimateState(bool state)
    {
        if(state)
        {
         enemyState = AIState.Chasing;
            navMeshAgent.speed = chaseSpeedBase;
        }
        animator.SetBool("chase",state);
    }
    private void IdleAnimteState(bool state)
    {
        if(state)
        {
            enemyState = AIState.Idling;
        }
        animator.SetTrigger("idle");
    }
    IEnumerator BreakOnNextAction(Vector3 newDestination)
    {
        //Debug.Log("Break...");

        isOnBreak = true;
        IdleAnimteState(true);
        yield return new WaitForSeconds(nextActionBreakBase);
        navMeshAgent.destination = newDestination;
        Debug.Log(navMeshAgent.destination);
        isOnBreak = false;


    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,chaseRangeBase);
        Gizmos.DrawWireSphere(transform.position,crouchRangeBase);

    }
        /*private void FocusArea()
        Debug.Log("Focus area!");
        if(Physics.CheckSphere(
            position:transform.position,
            radius:chaseRange,
            layerMask:layerMaskValue))
        {
            
            Debug.Log("Bullet triggered zombie!");
            isProvoked = true;
        }
    }*/
}
