using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    public float MaxHealth{get{return maxHealth;}}
    [SerializeField] float currentHealth;
    public float CurrentHealth{get{return currentHealth;}}
    public static PlayerHealth instance;
    private bool bloodOverFace = false;
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
   
    IEnumerator RegeneratingChecker()
    {
        float timer =0f;

        while(timer < timeToRegenerate)
        {
            timer+=Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return regenerationProcess = StartCoroutine(RegeneratingProcess());
    }
    IEnumerator RegeneratingProcess()
    {
        while(currentHealth<maxHealth)
        {
            currentHealth +=1;
            onFightOver();
            if(currentHealth > bloodOverFaceValue && bloodOverFace)
            {
                StartCoroutine(VingetteBumping.instance.StopBloodBumping());
                bloodOverFace = false;
            }
            yield return new WaitForSeconds(regenerationSpeed);
        }
        yield return null;
    }
    public void TakeDamage(float damage){

        currentHealth-=damage;
        //CAMERA SHAKE EFFECT
        StartCoroutine(PlayerCamera.instance.shakeCamera());

        //CALLS ALL FUNCTIONS SUBSCRIBED TO EVENT
        onDamageTaken();

        //VINGETTE EFFECT
        //which means if player has less than x Hp and dont have already blood over face casted
        if(currentHealth < bloodOverFaceValue && !bloodOverFace)
        {
            StartCoroutine(VingetteBumping.instance.StartBloodBumping());
            bloodOverFace = true;
        }
        //Every time player is attacked by enemy timer starts counting and if it reach x value regenerating process starts
        FightOver();
       
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
    
    
}
