using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //[SerializeField] Transform target;
    [SerializeField] float damage;
    [SerializeField] PlayerHealth target;
    public bool isAttackOver =true;

    private void Start() {
        target = PlayerHealth.instance;
    }
    public void AttackHitEvent()
    {
        if(target==null) return;
        target.TakeDamage(damage);
        Debug.Log("Hit!");
    }
    public IEnumerator Attack()
    {
        isAttackOver=false;
        AttackHitEvent();
        yield return new WaitForSeconds(.5f);
        isAttackOver=true;
    }
}
