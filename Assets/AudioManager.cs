using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    
    [NonReorderable]
    [SerializeField] Sound[] sounds;
    AudioSource audioSource;
    public static AudioManager Instance{get; private set;}

    private void Awake() {
        if(Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }else
        {
            Instance = this;
        }
    }
    public void playSoundAtPlace(string soundName,Vector3 position)
    {
        foreach (var sound in sounds)
        {
            if(sound.name == soundName)
            {
                AudioSource.PlayClipAtPoint(sound.clip,position);
            }
        }
    }
    public void playSound(AudioSource source,string name)
    {
        foreach (var sound in sounds)
        {
            if(sound.name == name)
            {
                
                source.PlayOneShot(sound.clip,sound.volume);
                return;
            }
        }
        Debug.LogWarning("SOUND NOT FOUND!");    
    }

}
