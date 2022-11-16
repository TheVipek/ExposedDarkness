using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySetting", menuName = "ZombieFPS/EnemySetting", order = 0)]
public class EnemySetting : ScriptableObject {

    //Stats
    
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

    //Sounds
    // public AudioClip[] attackSounds;

    // public AudioClip getAttackSound
    // {
    //     get { return attackSounds[Random.Range(0,attackSounds.Length)]; }
    // }
    // public AudioClip[] patrollingSounds;
    
    // public AudioClip[] damagedSounds;
    // public AudioClip[] deathSounds;
    // public AudioClip[] provokeSounds;
    // public AudioClip[] searchingSounds;

}
