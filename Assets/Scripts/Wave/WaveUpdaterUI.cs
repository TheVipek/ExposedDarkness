using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class WaveUpdaterUI : MonoBehaviour,IDisplayUI
{
    [SerializeField] Slider waveSlider;
    [SerializeField] TMP_Text currentWaveUI;
    [SerializeField] TMP_Text maxWaveUI;
    [SerializeField] GameObject panelWithBar;
    [SerializeField] GameObject panelWithTimer;




    [SerializeField] float lerpDuration;
    private float lerpElapsed;
    private float endValue;
    private float startValue;
    private Queue<IEnumerator> barCoroutines = new Queue<IEnumerator>();
    private Coroutine barCoroutine = null;
    
    private WaveController _waveController;
    public static bool isSliderUpdating;
    
    private void Awake() {
        _waveController = WaveController.Instance;
        maxWaveUI.text = _waveController.waveContainer.amountOfSubwaves.ToString();
        UpdateWaveValue();
    }
    private void OnEnable() {
        EnemiesAliveCounter.onEnemyAliveChange += smoothBarMoveCaller;
        
        WaveController.onWave += UpdateWaveValue;
        WaveController.onBreakStarted += DisplayUI;
        WaveController.onBreakEnded += DisplayUI;
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= smoothBarMoveCaller;

        WaveController.onWave -= UpdateWaveValue;
        WaveController.onBreakStarted -= DisplayUI;
        WaveController.onBreakEnded -= DisplayUI;
        
    }

    public void UpdateWaveValue()
    {
        currentWaveUI.text = _waveController.waveContainer.currentSubwave.ToString();
    }
    public void smoothBarMoveCaller()
    {
        if(barCoroutine != null)
        {
            barCoroutines.Enqueue(smoothBarMove());
        }else
        {
            barCoroutine = StartCoroutine(smoothBarMove());
        }
    }
    // public void showWaveToDecrease()
    // {
    //     waveSlider.GetComponent<RectTransform>().
    // }
    public void DisplayUI()
    {
        Debug.Log("Swapping wave UI");
        if(panelWithBar == null || panelWithTimer == null) return;
        panelWithBar.SetActive(!panelWithBar.activeSelf);
        panelWithTimer.SetActive(!panelWithTimer.activeSelf);

    }
    IEnumerator smoothBarMove()
    {
        yield return null;
        Debug.Log("startValue" + waveSlider.value);
        startValue = waveSlider.value;
        endValue = (float)EnemiesAliveCounter.currentEnemiesCount/(float)EnemiesAliveCounter.maxEnemiesCount;
        Debug.Log("endValue" + endValue);
      
        
        lerpElapsed =0;
        isSliderUpdating = true;
        while(lerpElapsed < lerpDuration)
        {
            waveSlider.value = Mathf.Lerp(startValue, endValue,lerpElapsed/lerpDuration);
            lerpElapsed += Time.deltaTime;
            yield return null;
        }
        waveSlider.value = endValue;
        if(barCoroutines.Count > 0)
        {
            yield return StartCoroutine(barCoroutines.Dequeue());
        }else
        {
            barCoroutine = null;
            isSliderUpdating = false;
        }

        
    }
}
