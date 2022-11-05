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
        triggerWave();
        EnemiesAliveCounter.onEnemyAliveChange += sendInformationAboutClean;
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= sendInformationAboutClean;
    }
    public void triggerWave()
    {
        waveController.initWave(waveToTrigger);
    }
    public void sendInformationAboutClean()
    {
        if(EnemiesAliveCounter.enemiesCount == 0){
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
