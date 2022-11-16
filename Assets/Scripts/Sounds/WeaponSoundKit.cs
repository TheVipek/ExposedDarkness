using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponSoundKit", menuName = "SoundKit/Weapon", order = 0)]
public class WeaponSoundKit : SoundKit
{
    [SerializeField] AudioClip reloadSounds;
    public AudioClip ReloadSound
    {
        get{ return reloadSounds; }
    }
    [SerializeField] AudioClip emptySounds;
    public AudioClip EmptySound
    {
        get{ return  emptySounds;}
    }
    [SerializeField] AudioClip shootSounds;
    public AudioClip ShootSound
    {
        get{ return  shootSounds;}
    }
    [SerializeField] AudioClip shootingTypeSounds;
    public AudioClip ShootingTypeSound
    {
        get{ return  shootingTypeSounds;}
    }
}
