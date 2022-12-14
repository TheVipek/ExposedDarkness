using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HealthDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] TMP_Text actualHp;
    [SerializeField] Slider sliderFill;
    [SerializeField] PlayerHealthSettings healthSettings;
    private void OnEnable() {
        PlayerHealth.onDamageTaken += DisplayUI;
        PlayerHealth.onHealthRestore += DisplayUI;
    }
    private void OnDisable() {
        PlayerHealth.onDamageTaken -= DisplayUI;
        PlayerHealth.onHealthRestore -= DisplayUI;
        
    }

    private void Start() {
        DisplayUI();
    }
    public void DisplayUI()
    {
        actualHp.text = healthSettings.CurrentHealth.ToString();
        sliderFill.value = healthSettings.CurrentHealth /100;
        
    }

    

}
