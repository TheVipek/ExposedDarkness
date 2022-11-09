using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HealthDisplayer : MonoBehaviour,IDisplayUI
{
    public HealthDisplayer instance;
    [SerializeField] TMP_Text actualHp;
    [SerializeField] float smoothLoseHp = 2f;
    float sliderComplete = 0f;
    [SerializeField] Slider sliderFill;
    private void Awake() {
        if(instance!= this && instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
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
