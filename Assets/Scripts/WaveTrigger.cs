using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public WaveContainer waveToTrigger;
    private WaveController waveController;
    private void Awake() {
        waveController = WaveController.Instance;
    }
    private void OnEnable() 
    {
        waveController.triggerWave(waveToTrigger);    
        EnemiesAliveCounter.onEnemyAliveChange += spawnSubwave;
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= spawnSubwave;
    }
    public void spawnSubwave()
    {
        if(EnemiesAliveCounter.enemiesCount == 0){
            waveController.currWave.currentSubwave +=1;
            if(waveController.currWave.currentSubwave <= waveController.currWave.amountOfSubwaves)
            {
                StartCoroutine(waveController.spawnSubwave());
            }
        }
    }
}
