using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDisplayer : MonoBehaviour,IDisplayUI
{
    [SerializeField] GameObject objectiveUIContainer;
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
        if(objectiveUIContainer == null) return;
        objectiveUIContainer.SetActive(!objectiveUIContainer.activeSelf);
    }
}
