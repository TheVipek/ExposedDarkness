using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] GameObject waveUIContainer;
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
        if(waveUIContainer == null) return;
        waveUIContainer.SetActive(!waveUIContainer.activeSelf);
    }
}
