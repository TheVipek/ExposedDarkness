using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WaveUpdaterUI : MonoBehaviour
{
    [SerializeField] Slider waveSlider;
    [SerializeField] TMP_Text currentWaveUI;
    [SerializeField] TMP_Text maxWaveUI;
   // [SerializeField] Image FilLAmount;
    // [SerializeField] Image waveToDecrease;
    private int maxEnemiesCount;
    private WaveController waveController;
    public float lerpDuration = 3f;
    float lerpElapsed;
    float endValue;
    float startValue;
    Queue<IEnumerator> barCoroutines = new Queue<IEnumerator>();
    Coroutine barCoroutine = null;
    private void Awake() {
        waveController = WaveController.Instance;
        maxWaveUI.text = waveController.currWave.amountOfSubwaves.ToString();
        InitSubwaveValues();
    }
    private void OnEnable() {
        EnemiesAliveCounter.onEnemyAliveChange += smoothBarMoveCaller;
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= smoothBarMoveCaller;
        
    }

    public void InitSubwaveValues()
    {
        Debug.Log(maxEnemiesCount);
        currentWaveUI.text = waveController.currWave.currentSubwave.ToString();
    }
    public void smoothBarMoveCaller()
    {
        if(barCoroutine != null)
        {
            // Debug.Log("Adding to queue");
            barCoroutines.Enqueue(smoothBarMove());
        }else
        {
            // Debug.Log("Simply coroutine");
            barCoroutine = StartCoroutine(smoothBarMove());
        }
    }
    // public void showWaveToDecrease()
    // {
    //     waveSlider.GetComponent<RectTransform>().
    // }
    IEnumerator smoothBarMove()
    {
        // Debug.Log("current: "+ EnemiesAliveCounter.currentEnemiesCount);
        // Debug.Log("max: "+EnemiesAliveCounter.maxEnemiesCount);
        // Debug.Log("valToGo: "+valueToGo);
        startValue = waveSlider.value;
        endValue = (float)EnemiesAliveCounter.currentEnemiesCount/(float)EnemiesAliveCounter.maxEnemiesCount;
        lerpElapsed =0;

        while(lerpElapsed < lerpDuration)
        {
            waveSlider.value = Mathf.Lerp(startValue, endValue,lerpElapsed/lerpDuration);
            lerpElapsed += Time.deltaTime;
            yield return null;
        }
        waveSlider.value = endValue;
        if(barCoroutines.Count > 0)
        {
            // Debug.Log("Starting new coroutine through coroutine");
            yield return StartCoroutine(barCoroutines.Dequeue());
        }else
        {
            // Debug.Log("Setting barCoroutine to null");
            barCoroutine = null;
        }
        
    }
}
