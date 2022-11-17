using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    

    AudioSource audioSource;
    public static Coroutine musicTransitor {get; private set;}
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
    public static void playSound(AudioSource _source,AudioClip _clip,bool loop = false)
    {
        if(_source == null || _clip == null) return;
        if(_source.isPlaying) _source.Stop();
        if(loop == true)
        {
            _source.loop = true;
        }
        else
        {
            _source.loop = false;
        }

        _source.pitch = Random.Range(0.9f,1.1f);
        Debug.Log(_source.pitch);
        _source.PlayOneShot(_clip,_source.volume); 
    }
    public static void musicSoundTransition(AudioSource source,bool mute,float transitionLength = 2f)
    {
        AudioManager _instance = AudioManager.Instance;
        //Debug.Log("musicSound called");
        if(musicTransitor != null)
        {
            _instance.StopCoroutine(musicTransitor);
        }
        musicTransitor = _instance.StartCoroutine(_instance.soundVolumeTransitor(source,transitionLength,mute));
        

    }
    IEnumerator soundVolumeTransitor(AudioSource source,float transitionLength,bool mute)
    {
        float currentSoundPoint = source.volume;

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
