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


    private int maxEnemiesInWave;
    private Queue<IEnumerator> barCoroutines = new Queue<IEnumerator>();
    private Coroutine barCoroutine = null;
    

    private WaveController waveController;
    public static bool isSliderUpdating;
    

    private void OnEnable() {
        WaveController.onWave += UpdateWaveValue;
        WaveController.onBreakStarted += DisplayUI;
        WaveController.onBreakEnded += DisplayUI;
    
        EnemiesManager.OnEnemyAliveChange += smoothBarMoveCaller;    
    }
    private void Start() {
        waveController = WaveController.Instance;
        maxWaveUI.text = waveController.waveContainer.amountOfSubwaves.ToString();
        UpdateWaveValue();
    }
    private void OnDisable() {
        WaveController.onWave -= UpdateWaveValue;
        WaveController.onBreakStarted -= DisplayUI;
        WaveController.onBreakEnded -= DisplayUI;
        
        EnemiesManager.OnEnemyAliveChange -= smoothBarMoveCaller;
    }

    public void UpdateWaveValue()
    {
         if(waveController == null)
        {
            Debug.LogError($"waveController is null in {this.GetType()},please make sure to reference it correctly!");
            return;
        }
        currentWaveUI.text = waveController.waveContainer.currentSubwave.ToString();
        //Every time wave starts , we need to get amount of monsters that are going to be spawned 
        maxEnemiesInWave = waveController.waveContainer.GetAmount();
    }
    public void smoothBarMoveCaller(int enemiesAlive)
    {
        if(barCoroutine != null || barCoroutines.Count > 0)
        {
            Debug.Log("Enqueueing");
            barCoroutines.Enqueue(smoothBarMove(enemiesAlive));
        }else
        {
            Debug.Log("Fresh start");
            barCoroutine = StartCoroutine(smoothBarMove(enemiesAlive));
        }
    }
    public void DisplayUI()
    {
        Debug.Log("Swapping wave UI");
        if(panelWithBar == null || panelWithTimer == null) return;
        panelWithBar.SetActive(!panelWithBar.activeSelf);
        panelWithTimer.SetActive(!panelWithTimer.activeSelf);

    }
    IEnumerator smoothBarMove(int enemiesAlive)
    {
        float _lerpDuration = lerpDuration;
        startValue = waveSlider.value;
        Debug.Log($"currentEnemiesCount: {enemiesAlive}, maxEnemiesCount: {maxEnemiesInWave}");
        if(enemiesAlive != 0 && maxEnemiesInWave != 0)
        {
            endValue = (float)enemiesAlive/(float)maxEnemiesInWave;
            if(endValue > startValue) _lerpDuration/=2;
        }
        else endValue = 0;
        Debug.Log("startValue" + waveSlider.value);
        Debug.Log("endValue" + endValue);

        
        lerpElapsed =0;
        isSliderUpdating = true;
        while(lerpElapsed < _lerpDuration)
        {
            waveSlider.value = Mathf.Lerp(startValue, endValue,lerpElapsed/_lerpDuration);
        //    Debug.Log($"sliderValue:{waveSlider.value}");
            lerpElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log($"After sliderValue:{waveSlider.value}");
        waveSlider.value = endValue;
        Debug.Log($"sliderValue:{waveSlider.value}");
        if(barCoroutines.Count > 0)
        {
            Debug.Log("Starting next coroutine");
            yield return StartCoroutine(barCoroutines.Dequeue());
        }else
        {
            barCoroutine = null;
            isSliderUpdating = false;
        }

        
    }
}
