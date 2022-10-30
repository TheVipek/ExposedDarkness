using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMouseOverTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointer over");
        if(animator.GetBool("isOver") == false)
                {
                    animator.SetBool("isOver",true);
                    //Debug.Log("set to true");
                }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(animator.GetBool("isOver") == true)
                {
                    animator.SetBool("isOver",false);
                    //Debug.Log("set to false");

                }  
    }
}
