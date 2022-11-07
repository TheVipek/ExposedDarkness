using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDisplayer : MonoBehaviour,IDisplayUI
{
     [SerializeField] GameObject timerUIContainer;
    private void OnEnable() {
        WaveController.onWaveStartGlobal += DisplayUI;
        WaveController.onWaveEndGlobal += DisplayUI;
    }
    private void OnDisable() {
        WaveController.onWaveStartGlobal -= DisplayUI;
        WaveController.onWaveEndGlobal -= DisplayUI;
        
    }
    void Start()
    {
        
    }

    public void DisplayUI()
    {
        if(timerUIContainer == null) return;
        timerUIContainer.SetActive(!timerUIContainer.activeSelf);
    }
}
