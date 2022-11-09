using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class ButtonMouseOverTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(animator.GetBool("isOver") == false)
                {
                    animator.SetBool("isOver",true);
                }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(animator.GetBool("isOver") == true)
                {
                    animator.SetBool("isOver",false);
                }  
    }
}
