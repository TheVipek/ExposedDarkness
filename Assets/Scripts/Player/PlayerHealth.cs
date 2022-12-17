using System.Collections;
using UnityEngine;
using System;
public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Stats")]
    [SerializeField] PlayerHealthSettings healthSettings;
    [Tooltip("At what currentHealth value trigger blood over face")]
    [SerializeField] float bloodOverFaceValue = 35f;
    [Tooltip("Time that needs elapse to start regenerating health")]
    [SerializeField] float timeToRegenerate = 5f;
    [Tooltip("Time that needs to elapse to regenerate 1HP")]
    [SerializeField] float regenerationSpeed = 0.2f;
    private bool bloodOverFace;
    public bool BloodOverFace{get{return bloodOverFace;}set{bloodOverFace = value;}}
    private bool poisonTriggered;
    public bool PoisonTriggered{get{return poisonTriggered;}set{poisonTriggered = value;}}


    public static Action onDamageTaken;
    public static Action onHealthRestore;
    //Hp drops below bloodOverFaceValue
    public GameEvent onLowHp;
    //Hp is above bloodOverFaceValue
    public GameEvent onNormalHp;
    public GameEvent onDeath;
    
    public bool IsDead{get{return animator.GetBool("death");} set{animator.SetBool("death",value);}}
    Coroutine regenerationChecker;
    Coroutine regenerationProcess;

    private void Awake() {
        healthSettings.SetPlayerHealth(this);
    }

    public void RestoreHp()
    {
        healthSettings.Restore();
        onHealthRestore();
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
        WaitForSeconds _regenerationSpeed = new WaitForSeconds(regenerationSpeed); 

        while(healthSettings.CurrentHealth<healthSettings.MaxHealth)
        {
            healthSettings.AddHp(1);
            //Update UI
            onHealthRestore();
            if(healthSettings.CurrentHealth > bloodOverFaceValue && bloodOverFace == true && poisonTriggered == false)
            {
                bloodOverFace = false;
                onNormalHp.Raise();
            }
            yield return _regenerationSpeed;
        }
        yield return null;
    }


    public void TakeDamage(float damage){
        healthSettings.TakeHp(damage);
        //CALLS ALL FUNCTIONS SUBSCRIBED TO EVENT
        onDamageTaken();
        if(healthSettings.CurrentHealth <= 0)
        {
            onDeath.Raise();
            return;
        }
        else if(healthSettings.CurrentHealth > 0)
        {

            //VINGETTE EFFECT
            //which means if player has less than x Hp and dont have already blood over face casted
            if(healthSettings.CurrentHealth < bloodOverFaceValue && bloodOverFace == false && poisonTriggered == false)
            {
                bloodOverFace = true;
                onLowHp.Raise();
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

}
