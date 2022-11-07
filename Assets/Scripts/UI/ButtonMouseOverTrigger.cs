using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class ButtonMouseOverTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Animator animator;
    [SerializeField] TMP_Text text; 
    [SerializeField] Color textColorOnHover;
    [SerializeField] Color textColorOutHover;
    [SerializeField] float transitionLength;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointer over");
        if(animator.GetBool("isOver") == false)
                {
                    StartCoroutine(TextColorTransition(textColorOnHover));
                    animator.SetBool("isOver",true);
                    //Debug.Log("set to true");
                }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(animator.GetBool("isOver") == true)
                {
                    StartCoroutine(TextColorTransition(textColorOutHover));
                    animator.SetBool("isOver",false);
                    //Debug.Log("set to false");

                }  
    }
    IEnumerator TextColorTransition(Color colorToGo)
    {
        float _length = transitionLength;
        while(_length > 0)
        {
            float percentageValue = _length / transitionLength;
            text.color = new Color(colorToGo.r * percentageValue , colorToGo.g * percentageValue , colorToGo.b * percentageValue ,255);
            _length -= Time.deltaTime;
            yield return null;
        }
    }
}
