using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    [SerializeField] float maxHealth = 100;
    public float MaxHealth{get{return maxHealth;}}
    [SerializeField] float currentHealth;
    public float CurrentHealth{get{return currentHealth;}}
    
    
    public bool IsDead{get{return animator.GetBool("death");} set{animator.SetBool("death",value);}}
    public bool IsDeadCasted{get{return animator.GetBool("deathCasted");} set{animator.SetBool("deathCasted",value);}}

    public static PlayerHealth instance;
    public bool bloodOverFace = false;
    public bool poisonTriggered = false;

    [SerializeField] float bloodOverFaceValue = 35f;


    [Tooltip("Time that needs elapse to start regenerating health")]
    [SerializeField] float timeToRegenerate = 5f;
    [Tooltip("Time that needs to elapse to regenerate 1HP")]
    [SerializeField] float regenerationSpeed = 0.2f;
    Coroutine regenerationChecker;
    Coroutine regenerationProcess;
    public delegate void OnDamageTaken();
    public static event OnDamageTaken onDamageTaken;

    public delegate void OnFightOver();
    public static event OnFightOver onFightOver;


    private void Awake() {
        
        currentHealth = maxHealth;
        if(instance != null && instance != this)
        {
            Destroy(this);
        }else
        {
            instance = this;
        }
        
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
            //CAMERA SHAKE EFFECT
            //StartCoroutine(PlayerCamera.instance.shakeCamera());
            //CALLS ALL FUNCTIONS SUBSCRIBED TO EVENT
            onDamageTaken();

            //VINGETTE EFFECT
            //which means if player has less than x Hp and dont have already blood over face casted
            if(currentHealth < bloodOverFaceValue && bloodOverFace == false && poisonTriggered == false)
            {
                bloodOverFace = true;
                StartCoroutine(VingetteBumping.instance.BloodBumping(VingetteBumping.instance.maxBloodValue,VingetteBumping.instance.entryValue));
            }
            //Every time player is attacked by enemy timer starts counting and if it reach x value regenerating process starts
            FightOver();
        }
       
    }
    public void FightOver()
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
