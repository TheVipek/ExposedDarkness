using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ZombieFPS/EnemyStats", order = 0)]
public class EnemyStats : ScriptableObject {
    public float baseDamage;
    public float baseHitpoints;
    public float baseSpeed;
    //How long should enemy lie at ground until disabling
    public float baseDeathStateLength;
    public float baseAttackRange;
}
