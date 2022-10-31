using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ZombieFPS/EnemyStats", order = 0)]
public class EnemyStats : ScriptableObject {
    public float baseDamage;
    public float baseHitpoints;
    public float baseSpeed;
    public float baseChaseSpeed;

    [Tooltip("How long should enemy lie at ground until disabling")]
    public float baseDeathStateLength;
    [Tooltip("Distance in which enemy can attack player (lower for meele ,higher for range types)")]
    public float baseAttackRange;
    [Tooltip("Range for player when is walking/running")]
    //5
    public float baseChaseRange;
    [Tooltip("Range for player when is crouching")]
    //3
    public float baseCrouchRange;
    [Tooltip("How fast zombie is rotating when detect player")]
    //5
    public float baseRotateSpeed;
}
