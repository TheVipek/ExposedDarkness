using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class WaveTrigger : MonoBehaviour
{
    public WaveContainer waveToTrigger;
    private WaveController _waveController;
    [SerializeField] float endWavesDelay;
    public UnityEvent OnTriggeredEndEvents;
    private void Awake() {
        _waveController = WaveController.Instance;
        _waveController.initWave(waveToTrigger);
    }
    private void OnEnable() 
    {
        WaveController.onWaveStartGlobal();
        EnemiesManager.OnEnemyAliveChange += EndWaveListener;
    }
    private void OnDisable() {
        EnemiesManager.OnEnemyAliveChange -= EndWaveListener;

    }
    public void EndWaveListener(int enemiesAlive)
    {
        if(enemiesAlive == 0){
            _waveController.waveContainer.currentSubwave +=1;
            if(_waveController.waveContainer.currentSubwave <= _waveController.waveContainer.amountOfSubwaves)
            {
                StartCoroutine(StartingBreak());
            }
            else if(_waveController.waveContainer.currentSubwave > _waveController.waveContainer.amountOfSubwaves)
            {
                StartCoroutine(CallEndWave());
            }
        }
    }
    IEnumerator StartingBreak()
    {
        while(WaveUpdaterUI.isSliderUpdating == true)
        {
            yield return null;
        }
        Debug.Log("Slider is not updating");
        WaveController.onBreakStarted();
        
    } 
    IEnumerator CallEndWave()
    {
        if(WaveUpdaterUI.isSliderUpdating == true)
        {
            yield return null;
        }
        Debug.Log("Slider is not updating");
        yield return new WaitForSeconds(endWavesDelay);
        _waveController.CallOnWaveGlobalEnd();
        OnTriggeredEndEvents.Invoke();
    }

}
