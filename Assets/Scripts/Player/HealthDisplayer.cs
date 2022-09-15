using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HealthDisplayer : MonoBehaviour
{
    public HealthDisplayer instance;
    [SerializeField] TMP_Text actualHp;
    [SerializeField] TMP_Text maxHp;
    
    [SerializeField] float smoothLoseHp = 2f;
    float sliderComplete = 0f;
    [SerializeField] Image imageFillerHp;
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
        PlayerHealth.onDamageTaken += DisplayHealth;
        PlayerHealth.onFightOver += DisplayHealth;
    }
    private void OnDisable() {
        PlayerHealth.onDamageTaken -= DisplayHealth;
        PlayerHealth.onFightOver -= DisplayHealth;
        
    }

    private void Start() {
        maxHp.text = PlayerHealth.instance.MaxHealth.ToString();
        actualHp.text = PlayerHealth.instance.CurrentHealth.ToString();
        imageFillerHp.fillAmount = PlayerHealth.instance.CurrentHealth /100;
    }
    void DisplayHealth()
    {
        actualHp.text = PlayerHealth.instance.CurrentHealth.ToString();
        imageFillerHp.fillAmount = PlayerHealth.instance.CurrentHealth /100;
        
    }

    

}
