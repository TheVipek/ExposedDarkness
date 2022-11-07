using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public WaveContainer waveToTrigger;
    private WaveController _waveController;
    private void Awake() {
        _waveController = WaveController.Instance;
        _waveController.initWave(waveToTrigger);
    }
    private void OnEnable() 
    {
        WaveController.onWaveStartGlobal();
        EnemiesAliveCounter.onEnemyAliveChange += EndWaveListener;
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= EndWaveListener;
    }
    public void EndWaveListener()
    {
        if(EnemiesAliveCounter.currentEnemiesCount == 0){
            _waveController.waveContainer.currentSubwave +=1;
            if(_waveController.waveContainer.currentSubwave <= _waveController.waveContainer.amountOfSubwaves)
            {
                StartCoroutine(StartingBreak());
                // StartCoroutine(waveController.spawnSubwave());
            }else
            {
                // PRINT END OF WAVES
            }
        }
    }
    IEnumerator StartingBreak()
    {
        yield return null;
        while(WaveUpdaterUI.isSliderUpdating == true)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        WaveController.onBreakStarted();
        
    } 

}
