using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSoundKit", menuName = "SoundKit/MeeleWeapon", order = 0)]
public class MeeleWeaponSoundKit : SoundKit
{
   [SerializeField] AudioClip swingSound;
    public AudioClip SwingSound
    {
        get{ return swingSound; }
    }
    [SerializeField] AudioClip hitSound;
    public AudioClip HitSound
    {
        get{ return  hitSound;}
    }
}
