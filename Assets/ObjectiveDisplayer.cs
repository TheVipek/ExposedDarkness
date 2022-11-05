using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] GameObject objectiveUIContainer;
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
        if(objectiveUIContainer == null) return;
        objectiveUIContainer.SetActive(!objectiveUIContainer.activeSelf);
    }
}
