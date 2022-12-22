using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
[RequireComponent(typeof(Animator))]
public class ButtonMouseOverTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioSource audioSource;
    [Header("Audio Clip Play Speed")]
    [Range(.1f,1.5f)]
    [SerializeField] float clipSpeed = 0.5f;

   // [SerializeField] UnityEvent onHoverEvent;
    Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointer");
        if(animator.GetBool("isOver") == false)
                {
                    animator.SetBool("isOver",true);
                    if(hoverSound != null && audioSource != null)
                    {
                        if(!audioSource.isPlaying) AudioManager.playSound(audioSource,hoverSound,pitch:clipSpeed,playOneShot:false);
                    }
                    //onHoverEvent.Invoke();
                }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("PointerExit");
        if(animator.GetBool("isOver") == true) animator.SetBool("isOver",false);
    }
}
