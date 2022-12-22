using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonListener : MonoBehaviour
{

    [Tooltip("Sound to be played after click")]
    [SerializeField] AudioClip onClickSound;
    [Tooltip("Audio source for sound - note that if there won't be source,sound won't be played")]
    [SerializeField] AudioSource audioSource;
    [Header("Audio Clip Play Speed")]
    [Range(.1f,1.5f)]
    [SerializeField] float clipSpeed;
    [Tooltip("What happens when we click")]
    [SerializeField] UnityEvent onClickEvent;

    [Header("Transition")]
    [Header("If transition is OFF ,the rest is ignored")]
    [SerializeField] bool transition;
    [Tooltip("Put there transitionController that we want to get info from")]
    [SerializeField] TransitionController transitionController;
    [SerializeField] bool transitionIn;
    [SerializeField] bool transitionOut;
    private float waitTime;

    public void OnElementAction()
    {
        if(audioSource != null && onClickSound != null)
        {
            AudioManager.playSound(audioSource,onClickSound,pitch: clipSpeed);
            float clipLength = onClickSound.length / Mathf.Abs(audioSource.pitch);
            StartCoroutine(actionDelayer(clipLength));
        }
        else
        {
            StartCoroutine(actionDelayer(0));
        }
    }
    public IEnumerator actionDelayer(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if(transition)
        {
            if(transitionIn && transitionOut)
            {
                transitionController.TransitionBoth();
                waitTime = transitionController.TransitionInLength *.5f;
                yield return new WaitForSecondsRealtime(waitTime);
            }
            else if(transitionIn)
            {
                transitionController.TransitionIn();
                waitTime = transitionController.TransitionInLength *.5f;
                yield return new WaitForSecondsRealtime(waitTime);
            }
            else if(transitionOut) 
            {
                transitionController.TransitionOut();
                waitTime = transitionController.TransitionOutLength *.25f;
                yield return new WaitForSecondsRealtime(waitTime);
            } 
        }
        
        onClickEvent.Invoke();

    }
    
}
