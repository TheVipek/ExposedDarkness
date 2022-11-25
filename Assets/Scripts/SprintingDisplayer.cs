using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SprintingDisplayer : MonoBehaviour
{
    [SerializeField] Canvas sprintingCanvas;
    [SerializeField] Slider sprintingSlider;
    [SerializeField] Animator animator;
    private Coroutine sprintingCoroutine;
    private void OnEnable() {
        PlayerMovement.onSprinting += sprintCounterCall;
    }
    private void OnDisable() {
        PlayerMovement.onSprinting -= sprintCounterCall;
    }

    public void sprintCounterCall()
    {
        Debug.Log(sprintingCoroutine);
        if(sprintingCoroutine == null) sprintingCoroutine = StartCoroutine(sprintingCounter()); 
    }
    public IEnumerator sprintingCounter()
    {
        //calling to avoid going out of while loop at start
        yield return null;
        sprintingSlider.value = PlayerMovement.Instance.currentStamina/PlayerMovement.Instance.StaminaLength;
        animator.SetTrigger("appear");
        while(PlayerMovement.Instance.CurrentStamina < PlayerMovement.Instance.StaminaLength)
        {
            float currValue =  PlayerMovement.Instance.CurrentStamina / PlayerMovement.Instance.StaminaLength;
            sprintingSlider.value = currValue;
            yield return null;
        }
        animator.SetTrigger("disappear");
        sprintingCoroutine = null;

    }
}
