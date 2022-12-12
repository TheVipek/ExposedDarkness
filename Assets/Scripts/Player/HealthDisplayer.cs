using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HealthDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] TMP_Text actualHp;
    [SerializeField] Slider sliderFill;
    private void OnEnable() {
        PlayerHealth.onDamageTaken += DisplayUI;
        PlayerHealth.onFightOver += DisplayUI;
    }
    private void OnDisable() {
        PlayerHealth.onDamageTaken -= DisplayUI;
        PlayerHealth.onFightOver -= DisplayUI;
        
    }

    private void Start() {
        DisplayUI();
    }
    public void DisplayUI()
    {
        actualHp.text = PlayerHealth.Instance.CurrentHealth.ToString();
        sliderFill.value = PlayerHealth.Instance.CurrentHealth /100;
        
    }

    

}
