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
    public static void playSound(AudioSource _source,AudioClip _clip,bool loop = false,float pitch = 0)
    {
        if(_source == null || _clip == null) return;

        if(_source.isPlaying) _source.Stop();

        if(loop) _source.loop = true;
        else _source.loop = false;
        
        if(pitch == 0) _source.pitch = Random.Range(0.9f,1.1f);
       else _source.pitch = pitch;
//        Debug.Log(_source.pitch);
        //  _source.clip = _clip;
        //  _source.Play();
        _source.PlayOneShot(_clip,_source.volume); 
    }
    public static void musicSoundTransition(AudioSource source,bool mute,float transitionLength = 1f)
    {
        AudioManager _instance = AudioManager.Instance;
        //Debug.Log("musicSound called");
        if(musicTransitor != null)
        {
            _instance.StopCoroutine(musicTransitor);
        }
        musicTransitor = _instance.StartCoroutine(_instance.soundVolumeTransitor(source,transitionLength,mute));
        

    }
   public static AudioClip GetRandom(AudioClip[] clips)
   {
        return clips[Random.Range(0,clips.Length)];
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
                if(source == null) yield break;
                source.volume = currentTransitionPoint/maxTransitionPoint;
                currentTransitionPoint -= Time.deltaTime;
                yield return null;
            }
            source.volume = 0;

        }else
        {
            while(currentTransitionPoint < maxTransitionPoint)
            {
                if(source == null) yield break;
                source.volume = currentTransitionPoint/maxTransitionPoint;
                currentTransitionPoint += Time.deltaTime;
                yield return null;
            }
            source.volume = 1;
        }
        yield return null;
    }
}
