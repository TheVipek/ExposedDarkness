using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FlashlightDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] TMP_Text stateText;
    [SerializeField] GameObject flashLightContainer;
    [SerializeField] GameObject deactivatedPanel;
    
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
    private void Update() {
        
    }
    public void DisplayFlashlighter()
    {
        
        
    }
    public void DisplayUI()
    {
        bool shouldBeVisible = FlashLightSystem.canDisplay() ? true : false;
        if(shouldBeVisible == true)
        {
            flashLightContainer.SetActive(true);
        }
        else
        {
            flashLightContainer.SetActive(false);
        }
    }
    public void SetFlashlightTextState(FlashlightState state)
    {
        stateText.text = state.ToString();
    }
   
   
}
