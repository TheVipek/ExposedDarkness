using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //[SerializeField] Transform target;
    [SerializeField] float damage;
    [SerializeField] PlayerHealth target;
    [SerializeField] AnimationClip attackAnimation;
    public bool isAttackOver =true;

    private void Start() {
        target = PlayerHealth.instance;
    }
    public void AttackHitEvent()
    {
        if(target == null || target.IsDead == true) return;
        PlayerCamera.instance.TriggerShake();
        target.TakeDamage(damage);
    }
    public IEnumerator Attack()
    {
        isAttackOver=false;
        AttackHitEvent();
        yield return new WaitForSeconds(.5f);
        isAttackOver=true;
    }
}
