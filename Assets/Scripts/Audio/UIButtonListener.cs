using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonListener : MonoBehaviour
{
    [SerializeField] AudioClip onClickSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] UnityEvent onClickEvent;


    [Header("Audio Clip Play Speed")]
    [Range(.1f,1.5f)]
    [SerializeField] float clipSpeed;
    public void OnElementAction()
    {
        AudioManager.playSound(audioSource,onClickSound,pitch: clipSpeed);
        float clipLength = onClickSound.length / Mathf.Abs(audioSource.pitch);
        StartCoroutine(actionDelayer(clipLength));
    }
    public IEnumerator actionDelayer(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        onClickEvent.Invoke();

    }
    
}
