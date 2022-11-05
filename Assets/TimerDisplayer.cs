using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDisplayer : MonoBehaviour,IDisplayUI
{
     [SerializeField] GameObject timerUIContainer;
    private void OnEnable() {
        WaveController.onWaveStarted += DisplayUI;
        WaveController.onWaveEnded += DisplayUI;
    }
    private void OnDisable() {
        WaveController.onWaveStarted -= DisplayUI;
        WaveController.onWaveEnded -= DisplayUI;
        
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
