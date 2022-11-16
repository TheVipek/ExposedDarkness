using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySoundKit", menuName = "SoundKit/EnemySoundKit", order = 0)]
public class EnemySoundKit : SoundKit
{
    [SerializeField] AudioClip[] attackSounds;
    public AudioClip AttackSound
    {
        get { return attackSounds[Random.Range(0,attackSounds.Length)]; }
    }
    [SerializeField] AudioClip[] patrollingSounds;
    public AudioClip PatrollingSound
    {
        get { return patrollingSounds[Random.Range(0,patrollingSounds.Length)]; }
    }
    [SerializeField] AudioClip[] damagedSounds;
    public AudioClip DamageSound
    {
        get { return damagedSounds[Random.Range(0,damagedSounds.Length)]; }
    }
    [SerializeField] AudioClip[] deathSounds;
    public AudioClip DeathSound
    {
        get { return deathSounds[Random.Range(0,deathSounds.Length)]; }
    }
    [SerializeField] AudioClip[] provokeSounds;
    public AudioClip ProvokeSound
    {
        get { return provokeSounds[Random.Range(0,provokeSounds.Length)]; }
    }
    [SerializeField] AudioClip[] searchingSounds;
    public AudioClip SearchingSound
    {
        get { return searchingSounds[Random.Range(0,searchingSounds.Length)]; }
    }
}
