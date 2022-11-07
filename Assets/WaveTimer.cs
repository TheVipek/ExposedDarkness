using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveTimer : MonoBehaviour
{
    [SerializeField] TMP_Text leftSeconds;
    private WaveController _waveController;
    private void Awake() {
        _waveController = WaveController.Instance;
    }
    private void OnEnable() {
        StartCoroutine(BreakBetweenWaves());
    }

    public IEnumerator BreakBetweenWaves()
    {
        float _breakTime = _waveController.breakBetweenWaves;
        while(_breakTime>0)
        {
            _breakTime -= Time.deltaTime;
            string textToDisplay =  (Mathf.Round(_breakTime*100)/100.0).ToString("n2");
            leftSeconds.text = textToDisplay;
            yield return null;
        }
        WaveController.onBreakEnded();
        yield return null;
        
    }
}
