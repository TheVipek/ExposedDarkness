using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float baseHitPoints = 100f;
    [SerializeField] float currentHitpoints;
    float CurrentHitPoints{get{return currentHitpoints;}}
    [Tooltip("How long zombie will lay on ground")]
    [SerializeField] float deathStateLength =5f;

    [Tooltip("Triggered onEnable and Death")]
    [SerializeField] bool canMove = true;
    
    private void OnEnable() {
        currentHitpoints = baseHitPoints;
        if(!canMove)
        {
            MovePossibility(true);
        }
    }

    public void TakeDamage(float damage)
    {
        GetComponent<enemyAI>().ProvokeTrigger();
        currentHitpoints-=Mathf.Abs(damage);
        if(currentHitpoints <= 0)
        {
            StartCoroutine(DeathCoroutine());
        }
    }
    void Death()
    {
            MovePossibility(false);
            GetComponent<Animator>().SetTrigger("death");
    }

    void MovePossibility(bool isAble)
    {
        GetComponent<enemyAI>().enabled = isAble;
        GetComponent<CapsuleCollider>().enabled = isAble;
        GetComponent<NavMeshAgent>().enabled = isAble;
        canMove=isAble;
    }
    IEnumerator DeathCoroutine()
    {
        Death();
        yield return new WaitForSeconds(deathStateLength);
        gameObject.SetActive(false);
        yield return null;
    }
}
