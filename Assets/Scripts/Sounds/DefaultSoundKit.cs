using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DefaultSoundKit", menuName = "SoundKit/Default", order = 0)]
public class DefaultSoundKit : SoundKit
{
    [SerializeField] AudioClip sound;
    public AudioClip Sound
    {
        get{ return sound;}
    } 
}
