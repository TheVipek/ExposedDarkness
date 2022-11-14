using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class InteractionContainer : MonoBehaviour,IInteractionActions
{
    public AudioSource audioSource;
    public AudioClip interactionSound;

    public virtual void OnInteractionStart()
    {
        Debug.Log("OnInteractionBase");
        if(audioSource != null && interactionSound != null) audioSource.PlayOneShot(interactionSound);
    }
    public virtual IEnumerator interactionWaiter(float delay)
    {
        WaitForSeconds _delay = new WaitForSeconds(delay);
        yield return _delay;
    }
}
