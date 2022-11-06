using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public WaveContainer waveToTrigger;
    private WaveController waveController;
    private void Awake() {
        waveController = WaveController.Instance;
        waveController.initWave(waveToTrigger);
    }
    private void OnEnable() 
    {
        waveController.startWaveEvents();
        EnemiesAliveCounter.onEnemyAliveChange += EndSubwaveListener;
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= EndSubwaveListener;
    }
    public void EndSubwaveListener()
    {
        if(EnemiesAliveCounter.currentEnemiesCount == 0){
            waveController.currWave.currentSubwave +=1;
            if(waveController.currWave.currentSubwave <= waveController.currWave.amountOfSubwaves)
            {
                waveController.BreakBetweenWaves(); 
                // StartCoroutine(waveController.spawnSubwave());
            }else
            {
                // PRINT END OF WAVES
            }
        }
    }
}
