using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FlashlightDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] Canvas flashLightCanvas;
    [SerializeField] GameObject flashLightObject;
    // Start is called before the first frame update
    private void OnEnable() {
        WeaponSwitcher.onWeaponChange+=DisplayUI;
        
    }
     private void OnDisable() {
        WeaponSwitcher.onWeaponChange-=DisplayUI;
    }
    public void DisplayUI()
    {
        bool shouldBeVisible = flashLightObject.activeInHierarchy ? true : false;

        if(shouldBeVisible == true)
        {
            flashLightCanvas.enabled = true;
        }
        else
        {
            flashLightCanvas.enabled = false;
        }
    }
   
   
}
