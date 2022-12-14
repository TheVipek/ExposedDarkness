using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SprintingDisplayer : MonoBehaviour
{
    [SerializeField] Canvas sprintingCanvas;
    [SerializeField] Slider sprintingSlider;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMoveSettings playerSettings;
    private Coroutine sprintingCoroutine;
   // private void OnEnable() => PlayerMovement.onSprinting += sprintCounterCall;
   // private void OnDisable() => PlayerMovement.onSprinting -= sprintCounterCall;
    public void SprintCounterCall()
    {
        if(playerSettings == null)
        {
            Debug.Log("Player is null");
            return;
        }
        if(sprintingCoroutine == null) 
        {
            sprintingCoroutine = StartCoroutine(SprintingCounter()); 
        }
        else
        {
            StopCoroutine(sprintingCoroutine);
            sprintingCoroutine = StartCoroutine(SprintingCounter()); 

        }
    }
    public IEnumerator SprintingCounter()
    {
        //calling to avoid going out of while loop at start
        yield return null;
        sprintingSlider.value = playerSettings.CurrentStamina/playerSettings.StaminaLength;
        animator.SetBool("appear",true);
        while(playerSettings.CurrentStamina < playerSettings.StaminaLength)
        {
            float currValue =  playerSettings.CurrentStamina / playerSettings.StaminaLength;
            sprintingSlider.value = currValue;
            yield return null;
        }
        sprintingCoroutine = null;
        animator.SetBool("appear",false);
    }
}
