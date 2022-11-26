using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(AudioSource),typeof(BoxCollider),typeof(InteractionObject))]
public abstract class InteractionContainer : MonoBehaviour,IInteractionActions
{
    public AudioSource audioSource;
    public AudioClip interactionSound;
    public BoxCollider boxCollider;
    public virtual void OnInteractionStart()
    {
//        Debug.Log("OnInteractionBase");
        if(audioSource != null && interactionSound != null) audioSource.PlayOneShot(interactionSound);
        
    }
    public virtual IEnumerator interactionWaiter(float delay)
    {
        boxCollider.enabled = false;
       // SetActiveChild(false);
        WaitForSeconds _delay = new WaitForSeconds(delay);
        yield return _delay;
        //SetActiveChild(true);
        boxCollider.enabled = true;

        
    }
    public void SetActiveChild(bool active)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}
