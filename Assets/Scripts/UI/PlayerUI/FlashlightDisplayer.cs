using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FlashlightDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] Canvas flashLightCanvas;
    [SerializeField] GameObject flashLightObject;
    private void OnEnable() => WeaponSwitcher.onWeaponChange+=DisplayUI;
     private void OnDisable() => WeaponSwitcher.onWeaponChange-=DisplayUI;
    public void DisplayUI()
    {
        bool shouldBeVisible = flashLightObject.activeInHierarchy ? 
                                    flashLightCanvas.enabled = true 
                                    : 
                                    flashLightCanvas.enabled = false;
    }
   
   
}
