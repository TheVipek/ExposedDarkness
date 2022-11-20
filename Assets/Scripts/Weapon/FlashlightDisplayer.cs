using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FlashlightDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] TMP_Text stateText;
    // [SerializeField] GameObject flashLightContainer;
    // [SerializeField] GameObject deactivatedPanel;
    [SerializeField] Canvas flashLightCanvas;
    public static FlashlightDisplayer Instance{get; private set;}
    private void Awake() {
        if(Instance!=this && Instance != null)
        {
            Destroy(this);
        }else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    private void OnEnable() {
        WeaponSwitcher.onWeaponChange+=DisplayUI;
        
    }
     private void OnDisable() {
        WeaponSwitcher.onWeaponChange-=DisplayUI;
    }
    public void DisplayUI()
    {
        bool shouldBeVisible = FlashLightSystem.canDisplay() ? true : false;
        Debug.Log(shouldBeVisible);
        if(shouldBeVisible == true)
        {
            flashLightCanvas.enabled = true;
        }
        else
        {
            flashLightCanvas.enabled = false;
        }
    }
    public static void SetFlashlightTextState(FlashlightState state)
    {
        FlashlightDisplayer.Instance.stateText.text = state.ToString();
    }
   
   
}
