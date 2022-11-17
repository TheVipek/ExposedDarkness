using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HealthDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] TMP_Text actualHp;
    [SerializeField] float smoothLoseHp = 2f;
    float sliderComplete = 0f;
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
        actualHp.text = PlayerHealth.instance.CurrentHealth.ToString();
        sliderFill.value = PlayerHealth.instance.CurrentHealth /100;
        
    }

    

}
