using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] GameObject waveUIContainer;

    private void OnEnable() {
        WaveController.onWaveStartGlobal += DisplayUI;
        WaveController.onWaveEndGlobal += DisplayUI;
    }
    private void OnDisable() {
        WaveController.onWaveStartGlobal -= DisplayUI;
        WaveController.onWaveEndGlobal -= DisplayUI;
        
    }
    public void DisplayUI()
    {
        if(waveUIContainer == null) return;
        waveUIContainer.SetActive(!waveUIContainer.activeSelf);
    }
    
}
