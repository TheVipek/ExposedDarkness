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
    public float chaseSpeedBase{get; private set;}
    private WaitForSeconds idleWaitTime;
    private bool isDeath;
    // References
    protected Animator animator;
    PlayerHealth target;

    //Data backpack
    [SerializeField] CapsuleCollider capsuleCollider;
    public EnemySetting enemySetting; 
    public EnemySoundKit enemySoundKit;

    
    //Audio 
    protected AudioSource mainAudioSource;
    [SerializeField] protected AudioSource breathingAudioSource;
    //Properties
    public Animator getAnimator{get{return animator;}}
    public PlayerHealth Target{get{return target;}}
    [SerializeField] protected bool canMove;
    //Events
    public delegate void OnDamageTaken();
    public event OnDamageTaken onDamageTaken;
    protected virtual void InitStats()
    {
        baseDamage = enemySetting.baseDamage;
        baseHitpoints = enemySetting.baseHitpoints;
        deathStateLength = enemySetting.baseDeathStateLength;
    }
    protected virtual void Awake() {
        //Debug.Log("baseHitpoints Awake: " + baseHitpoints);
       // InitStats();
        animator = GetComponentInChildren<Animator>();
        mainAudioSource = GetComponent<AudioSource>();
        //Make sure every enemy has it's own animation 
        Assert.IsNotNull(animator.runtimeAnimatorController,gameObject.name + " not set runtimeAnimatorController");
    }
    protected virtual void OnEnable() {
       
        //calling method with true parameter so if zombie was choosen from pool he'll be able to move
       if(canMove == false) MovePossibility(true);
       
       InitStats();
       
       isDeath = false;

       StartCoroutine(playIdleSounds(enemySoundKit.PatrollingSounds));
       
       EnemiesAliveCounter.increaseEnemiesCount();


    }
    protected virtual void OnDisable()
    {
       // Debug.Log("baseHitpoints onDisable: " + baseHitpoints);

    }
    protected virtual void Start()
    {
        target = PlayerHealth.instance;
        Assert.IsNotNull(target,gameObject.name + " not set target");

    }
    public virtual void Attack()
    {
        //If player dont exist or is dead dont call anything;
        if(target == null || target.IsDead == true) return;
        AudioManager.playSound(mainAudioSource,AudioManager.GetRandom(enemySoundKit.AttackSound));
        
        //Player damage dealing
        target.TakeDamage(baseDamage);
        
    }
    //Zombie health reducing
    public virtual void TakeDamage(float damage)
    {
        onDamageTaken?.Invoke();
       // AudioManager.playSound(audioSource,enemySoundKit.DamageSound);
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
        capsuleCollider.enabled = isAble;
        canMove = isAble;

    }
    
    public virtual void Death()
    {
        isDeath = true;
        AudioManager.playSound(mainAudioSource,AudioManager.GetRandom(enemySoundKit.DeathSounds));
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
    public virtual void gotProvoked()
    {
        Debug.Log("Got provoked");
   
    }
    IEnumerator playIdleSounds(AudioClip[] idleSounds)
    {
        while(true)
        {
            foreach(AudioClip clip in idleSounds)
            {
                if(isDeath == true) yield break;
                idleWaitTime = new WaitForSeconds(clip.length);
                AudioManager.playSound(breathingAudioSource,clip);
                yield return idleWaitTime;
            }
            yield return null;
        }
    }
}