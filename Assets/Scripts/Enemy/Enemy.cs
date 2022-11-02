using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;




public abstract class Enemy : MonoBehaviour {

    // Base variables
    protected float baseDamage;
    public float BaseDamage{get{return baseDamage;}}
    protected float baseHitpoints;
    public float BaseHitpoints{get{return baseHitpoints;}}
    protected float deathStateLength;

    // References
    protected Animator animator;
    PlayerHealth target;

    //Data backpack
    public EnemyStats statsScriptable; 
    public float chaseSpeedBase{get; private set;}
    [Range(1,10)]
    public float chaseSpeedMultiplier;
    [SerializeField] string attackSound;
    [SerializeField] string patrollingSound;
    [SerializeField] string deathSound;
    [SerializeField] string provokeSound;
    
    protected AudioSource audioSource;
    //Properties
    public Animator getAnimator{get{return animator;}}
    public PlayerHealth Target{get{return target;}}
    [SerializeField] protected bool canMove;

    //Events
    public delegate void OnDamageTaken();
    public event OnDamageTaken onDamageTaken;
    protected virtual void InitStats()
    {
        baseDamage = statsScriptable.baseDamage;
        baseHitpoints = statsScriptable.baseHitpoints;
        deathStateLength = statsScriptable.baseDeathStateLength;
    }
    protected virtual void Awake() {
        InitStats();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        //Make sure every enemy has it's own animation 
        Assert.IsNotNull(animator.runtimeAnimatorController,gameObject.name + " not set runtimeAnimatorController");
    }
    protected virtual void OnEnable() {
        //calling method with true parameter so if zombie was choosen from pool he'll be able to move
       if(canMove == false) MovePossibility(true);

    }
    protected virtual void OnDisable()
    {
        
    }
    protected virtual void Start()
    {
        AudioManager.Instance.playSound(audioSource,patrollingSound);
        target = PlayerHealth.instance;
        Assert.IsNotNull(target,gameObject.name + " not set target");

    }
    public virtual void Attack()
    {
        AudioManager.Instance.playSound(audioSource,attackSound);
        //If player dont exist or is dead dont call anything;
        if(target == null || target.IsDead == true) return;
        
        //Player damage dealing
        target.TakeDamage(baseDamage);
        
    }
    //Zombie health reducing
    public virtual void TakeDamage(float damage)
    {
        onDamageTaken?.Invoke();
        //Make it always positive
        baseHitpoints-=Mathf.Abs(damage);

        if(baseHitpoints <= 0)
        {
            StartCoroutine(DeathCoroutine());
        }
    }
    //Controlling zombie move
    public virtual void MovePossibility(bool isAble)
    {
        GetComponent<CapsuleCollider>().enabled = isAble;
        canMove = isAble;

    }
    
    public virtual void Death()
    {
        AudioManager.Instance.playSound(audioSource,deathSound);
        if(animator!=null) animator.SetTrigger("death");
        MovePossibility(false);

    }
    public virtual IEnumerator DeathCoroutine()
    {
        Death();
        yield return new WaitForSeconds(deathStateLength);
        gameObject.SetActive(false);
        yield return null;
    }
    public virtual void gotProvoked(bool provokeState)
    {
        if(provokeState == true)
        {
            AudioManager.Instance.playSound(audioSource,provokeSound);
        }
    }
}