using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Enemy
{



    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void Attack()
    {
        base.Attack();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
    public override void MovePossibility(bool isAble)
    {
        //set navmesh and AI script disabled 
        if(GetComponent<ZombieAI>() != null)
        {
            GetComponent<ZombieAI>().enabled = isAble;
            GetComponent<NavMeshAgent>().enabled = isAble;
        }
        base.MovePossibility(isAble);
    }
    public override void Death()
    {
        base.Death();
    }
    public override IEnumerator DeathCoroutine()
    {
        return base.DeathCoroutine();
    }
}
