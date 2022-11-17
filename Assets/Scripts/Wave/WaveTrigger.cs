using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public WaveContainer waveToTrigger;
    private WaveController _waveController;
    [SerializeField] GameObject missionCompletionCanvas;
    private void Awake() {
        _waveController = WaveController.Instance;
        _waveController.initWave(waveToTrigger);
    }
    private void OnEnable() 
    {
        WaveController.onWaveStartGlobal();
        EnemiesAliveCounter.onEnemyAliveChange += EndWaveListener;
        if(missionCompletionCanvas != null){
            WaveController.onWaveEndGlobal += missionCompletionTrigger; 
        }
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= EndWaveListener;
        if(missionCompletionCanvas != null){
            WaveController.onWaveEndGlobal -= missionCompletionTrigger; 
        } 

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
    public void missionCompletionTrigger()
    {
        missionCompletionCanvas.gameObject.SetActive(true);
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
