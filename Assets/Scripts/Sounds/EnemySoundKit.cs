using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySoundKit", menuName = "SoundKit/EnemySoundKit", order = 0)]
public class EnemySoundKit : SoundKit
{
    
    [field: SerializeField] public AudioClip[] AttackSound {get; private set;}
    [field:SerializeField] public AudioClip[] PatrollingSounds {get; private set;}
    //[field:SerializeField] public AudioClip[] DamagedSounds {get; private set;}
    [field:SerializeField] public AudioClip[] DeathSounds {get; private set;}

    [field:SerializeField] public AudioClip[] ProvokeSounds {get; private set;}
    // [field:SerializeField] public AudioClip[] SearchingSounds {get; private set;}

    
}
