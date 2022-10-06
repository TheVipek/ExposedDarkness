using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Zombie))]
public class ZombieAI : enemyAI
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        //target = GetComponent<Zombie>().Target.transform;
    }
    protected override void Update()
    {
        base.Update();
    }
    
    
    
    
}
