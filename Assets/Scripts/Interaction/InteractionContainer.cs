using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(InteractionObject))]
public abstract class InteractionContainer : MonoBehaviour,IInteractionActions
{
    [Tooltip("If source and clip won't be added and attached to script sound won't be played")]
    public AudioSource audioSource;
    [Tooltip("If source and clip won't be added and attached to script sound won't be played")]
    public AudioClip interactionSound;
    [Tooltip("Needs to be added to object and attached to script for raycast")]
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
