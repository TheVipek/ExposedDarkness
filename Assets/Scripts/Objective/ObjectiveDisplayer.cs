using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] Canvas objectiveCanvas;
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
        if(objectiveCanvas == null) return;
        objectiveCanvas.enabled = !objectiveCanvas.enabled;
    }
}
