using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FlashlightDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text stateText;
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
        WeaponSwitcher.onWeaponChange+=DisplayFlashlighter;
    }
     private void OnDisable() {
        WeaponSwitcher.onWeaponChange-=DisplayFlashlighter;
    }
    private void Update() {
        
    }
    public void DisplayFlashlighter()
    {
        if(FlashLightSystem.canDisplay() == true)
        {
            if(deactivatedPanel.gameObject.activeInHierarchy == true)
            {
                deactivatedPanel.SetActive(false);
            }
        }
        else
        {
            if(deactivatedPanel.gameObject.activeInHierarchy == false)
            {
                deactivatedPanel.SetActive(true);
            }
        }
        
    }
    public void SetFlashlightTextState(FlashlightState state)
    {
        stateText.text = state.ToString();
    }
   
   
}
