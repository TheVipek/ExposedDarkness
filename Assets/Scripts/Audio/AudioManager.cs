using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    

    [NonReorderable]
    [SerializeField] Sound[] sounds;
    AudioSource audioSource;
    private Coroutine musicTransitor;
    public static AudioManager Instance{get; private set;}

    private void Awake() {
        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
    public void musicSoundTransition(AudioSource source,bool mute,float transitionLength = 2f)
    {
        //Debug.Log("musicSound called");
        if(musicTransitor != null)
        {
            StopCoroutine(musicTransitor);
        }
        musicTransitor = StartCoroutine(soundVolumeTransitor(source,transitionLength,mute));
        

    }
    IEnumerator soundVolumeTransitor(AudioSource source,float transitionLength,bool mute)
    {
        float currentSoundPoint = source.volume;
        //Debug.Log(currentSoundPoint);
        float currentTransitionPoint = transitionLength * currentSoundPoint;
        //Debug.Log(currentTransitionPoint);

        float maxTransitionPoint = transitionLength;
       // Debug.Log(maxTransitionPoint);
        if(mute)
        {
            while(currentTransitionPoint > 0)
            {
                source.volume = currentTransitionPoint/maxTransitionPoint;
                currentTransitionPoint -= Time.deltaTime;
                yield return null;
            }
            source.volume = 0;

        }else
        {
            while(currentTransitionPoint < maxTransitionPoint)
            {
                source.volume = currentTransitionPoint/maxTransitionPoint;
                currentTransitionPoint += Time.deltaTime;
                yield return null;
            }
            source.volume = 1;
        }
        yield return null;
    }
}
