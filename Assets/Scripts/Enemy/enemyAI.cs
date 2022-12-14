using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
[RequireComponent(typeof(NavMeshAgent),typeof(Waypoints))]

public abstract class enemyAI : MonoBehaviour
{
    Enemy enemy;
    EnemySetting enemySetting;
    EnemySoundKit enemySoundKit;
    AudioSource mainAudioSource;
    

    //It could take from enemy when is provoked 
    [SerializeField] protected PlayerHealthSettings target;
    [SerializeField] protected LayerMask playerLayer;
    
    //Is set to infinity so there wont be any problem when AI script is initializing
    float distanceToTarget = Mathf.Infinity;

    NavMeshAgent navMeshAgent;
    Waypoint[] patrollingWaypoints;
    Vector3 startingPosition;
    int lastWaypoint;
    int currentWaypoint = 0;
    [SerializeField] float nextActionBreakBase = 8f;
    [SerializeField] float tryingCatchTargetBase = 8f;
    float tryingCatchTarget;
    bool isPatrolling,isOnBreak,isProvoked,isAttacking = false;
    Animator animator;
    bool isOnStartingPosition = true;
    AIState enemyState  = AIState.Idling;
    private RaycastHit sphereHit;

    protected virtual void OnEnable()
    {
        enemy.onDamageTaken += ProvokeTrigger;
        
    }
    protected virtual void OnDisable() 
    {
        enemy.onDamageTaken -= ProvokeTrigger;
    }

    protected virtual void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        startingPosition = gameObject.transform.position;
        lastWaypoint = currentWaypoint;
        patrollingWaypoints = GetComponent<Waypoints>().waypoints;
        enemy = GetComponent<Enemy>();
        enemySetting = enemy.enemySetting;
        enemySoundKit = enemy.enemySoundKit;
        mainAudioSource = GetComponent<AudioSource>();
        animator = enemy.getAnimator;
        NormalMode();
    }
    
    protected virtual void Start()
    {
      //  Assert.IsNotNull(target,gameObject.name + " not set target");

        if(patrollingWaypoints.Length>1) isPatrolling = true;
        else isPatrolling = false;
    }
    protected virtual void Update()
    {
        
        CheckArea();
        if(isPatrolling == true && isOnBreak == false  && isProvoked == false)
        {
            PatrollingArea();
        }

        if(target != null)
        {
            if(isProvoked == true)
            {
                EngageTarget();
            }
            if(isPatrolling == false && isOnStartingPosition == false && isProvoked == false && isOnBreak == false)
            {
                BackToStartingPos();
            }
            
            if(target.PlayerHealth.IsDead == false)
            {
                if(distanceToTarget <= enemySetting.baseChaseRange)
                {
                    isProvoked = true;
                }
            }
        }
        
    }
    public virtual void CheckArea()
    {
        // Collider[] hitColliders = Physics.OverlapSphere(transform.position,enemySetting.baseChaseRange,playerLayer);
        // if(hitColliders.Length > 0 && target == null)
        // {
        //     Debug.Log("Player is in range");
        //     Debug.Log($"SphereHit: {hitColliders[0].gameObject.name}");
        //     target = hitColliders[0].gameObject.GetComponent<PlayerHealth>();
        //     //Setting target for Enemy base Class also
        //     GetComponent<Enemy>().Target = target;
        //     Debug.Log($"TargetName:{target.name}");
        //     isProvoked = true;
            
        // }
        if(target == null) return;
        distanceToTarget = Vector3.Distance(
            new Vector3(transform.position.x,0,transform.position.z),
            new Vector3(target.PlayerHealth.transform.position.x,0,target.PlayerHealth.transform.position.z));
    }
    public virtual void SanityMode()
    {
        tryingCatchTarget = Mathf.Infinity;
        tryingCatchTargetBase = Mathf.Infinity;
        isProvoked = true;
    }
    public virtual void NormalMode()
    {
        tryingCatchTarget = tryingCatchTargetBase;
    }
    public virtual void EngageTarget()
    {
        if(target.PlayerHealth.IsDead == true)
        {
            ChaseAnimateState(false);
            isProvoked = false;
            AttackTarget(false);
            StartCoroutine(BreakOnNextAction(patrollingWaypoints[lastWaypoint].Position));
            return;
        }
        if(distanceToTarget <= 10) FaceTarget();
        
        //FaceTarget();
        //time ended or distance is over possible and player not catched
        if(tryingCatchTarget <= 0)
        {
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
        
        if(distanceToTarget >= navMeshAgent.stoppingDistance && isAttacking == false)
        {
            ChaseTarget();
            tryingCatchTarget-=Time.deltaTime;
        }
        else if(distanceToTarget < navMeshAgent.stoppingDistance || isAttacking == true)
        { 
            navMeshAgent.ResetPath();
            if(isAttacking == false)
            {
                isAttacking = true;
                AttackTarget(true);
            
                if(tryingCatchTarget!=tryingCatchTargetBase) tryingCatchTarget = tryingCatchTargetBase;
            }
            else
            {
                if(distanceToTarget > navMeshAgent.stoppingDistance + enemySetting.baseAttackRange)
                {
                    isAttacking = false;
                }
            }
        }    
    }
    public virtual void ProvokeTrigger()
    {
        if(isProvoked == false)
        {
            isProvoked = true;
            AudioManager.playSound(mainAudioSource,AudioManager.GetRandom(enemySoundKit.ProvokeSounds));
        }
    }
    public virtual void ChaseTarget()
    {
        isOnStartingPosition = false;
        AttackTarget(false);
        PatrollingAnimateState(false);
        ChaseAnimateState(true);
//        Debug.Log(navMeshAgent.destination);
        navMeshAgent.SetDestination(target.PlayerHealth.transform.position);
    }
    public virtual void FaceTarget()
    {
        Vector3 direction = (target.PlayerHealth.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime*enemySetting.baseRotateSpeed);
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

        if(navMeshAgent.remainingDistance <= 0.5f)
        {
//            Debug.Log(navMeshAgent.remainingDistance);
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
        if(!navMeshAgent.isActiveAndEnabled) yield break;
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

        if(state)
        {
            navMeshAgent.speed = enemySetting.baseSpeed;
            enemyState = AIState.Patrolling;
            //AudioManager.playSound(audioSource,enemySoundKit.PatrollingSound,false);
 //           Debug.Log("Playing patrolling sound...");


        }
        animator.SetBool("patrol",state);
    }
    protected void ChaseAnimateState(bool state)
    {
        if(state)
        {
            enemyState = AIState.Chasing;
            navMeshAgent.speed = enemySetting.baseChaseSpeed;
         //   AudioManager.playSound(audioSource,enemySoundKit.ProvokeSound,false);
    //        Debug.Log("Playing provoke sound...");

        }
        animator.SetBool("chase",state);
    }
    protected void IdleAnimteState(bool state)
    {
        if(state)
        {
            enemyState = AIState.Idling;
           // AudioManager.playSound(audioSource,enemySoundKit.PatrollingSound,false);
//            Debug.Log("Playing idle sound...");

        }
        animator.SetTrigger("idle");
    }
    protected virtual void OnDrawGizmosSelected() 
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position,enemySetting.baseChaseRange);
        // Gizmos.DrawWireSphere(transform.position,enemySetting.baseCrouchRange);
        //Gizmos.DrawSphere(transform.position,10);
    }
}
