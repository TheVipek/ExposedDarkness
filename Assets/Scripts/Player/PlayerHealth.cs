using System.Collections;
using UnityEngine;
using System;
public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Stats")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    [Tooltip("At what currentHealth value trigger blood over face")]
    [SerializeField] float bloodOverFaceValue = 35f;
    [Tooltip("Time that needs elapse to start regenerating health")]
    [SerializeField] float timeToRegenerate = 5f;
    [Tooltip("Time that needs to elapse to regenerate 1HP")]
    [SerializeField] float regenerationSpeed = 0.2f;
    public bool bloodOverFace = false;
    public bool poisonTriggered = false;

    public static Action onDamageTaken;
    public static Action onFightOver;
    
    
    
    public float MaxHealth{get{return maxHealth;}}
    public float CurrentHealth{get{return currentHealth;}}
    public bool IsDead{get{return animator.GetBool("death");} set{animator.SetBool("death",value);}}
    public bool IsDeadCasted{get{return animator.GetBool("deathCasted");} set{animator.SetBool("deathCasted",value);}}
    
    

    public static PlayerHealth Instance;



    Coroutine regenerationChecker;
    Coroutine regenerationProcess;


    private void Awake() {
        
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        
        currentHealth = maxHealth;
    }
    public void restoreHp()
    {
        currentHealth = maxHealth;
        onFightOver();
    } 
    IEnumerator RegeneratingChecker()
    {
        float timer =0f;

        while(timer < timeToRegenerate)
        {
            timer+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if(IsDead == false) yield return regenerationProcess = StartCoroutine(RegeneratingProcess());
    }
    IEnumerator RegeneratingProcess()
    {
        while(currentHealth<maxHealth)
        {
            currentHealth +=1;
            onFightOver();
            if(currentHealth > bloodOverFaceValue && bloodOverFace == true && poisonTriggered == false)
            {
                bloodOverFace = false;
            }
            yield return new WaitForSeconds(regenerationSpeed);
        }
        yield return null;
    }
    public void TakeDamage(float damage){

        currentHealth-=damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            onDamageTaken();
            return;
        }
        else if(currentHealth > 0)
        {
            //CALLS ALL FUNCTIONS SUBSCRIBED TO EVENT
            onDamageTaken();

            //VINGETTE EFFECT
            //which means if player has less than x Hp and dont have already blood over face casted
            if(currentHealth < bloodOverFaceValue && bloodOverFace == false && poisonTriggered == false)
            {
                bloodOverFace = true;
                StartCoroutine(VingetteBumping.instance.BloodBumping(VingetteBumping.instance.maxBloodValue,VingetteBumping.instance.entryValue));
            }
            FightStart();
        }
       
    }

    ///<summary> Reset regenerationChecker coroutine if there's any running and start new </summary>
    public void FightStart()
    {
        
        if(regenerationChecker!=null)
        {
            StopCoroutine(regenerationChecker);
        }
        if(regenerationProcess!=null)
        {
            StopCoroutine(regenerationProcess);
        }
        regenerationChecker = StartCoroutine(RegeneratingChecker());
        
    }
    public void SetDeathCasted()
    {
        IsDeadCasted = true;
    }
    public void CallDeathUI()
    {
        DeathHandler.instance.DeathUIanimation();
    }
}
