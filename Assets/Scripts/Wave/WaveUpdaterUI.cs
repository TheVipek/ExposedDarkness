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
    
    private WaveController waveController;
    private EnemiesManager enemiesManager;
    public static bool isSliderUpdating;
    
    private void Awake() {
    }
    private void OnEnable() {
        EnemiesManager.onEnemyAliveChange += smoothBarMoveCaller;
        
        WaveController.onWave += UpdateWaveValue;
        WaveController.onBreakStarted += DisplayUI;
        WaveController.onBreakEnded += DisplayUI;
    }
    private void Start() {
        enemiesManager = EnemiesManager.Instance;
        waveController = WaveController.Instance;
        maxWaveUI.text = waveController.waveContainer.amountOfSubwaves.ToString();
        UpdateWaveValue();
        
    }
    private void OnDisable() {
        EnemiesManager.onEnemyAliveChange -= smoothBarMoveCaller;

        WaveController.onWave -= UpdateWaveValue;
        WaveController.onBreakStarted -= DisplayUI;
        WaveController.onBreakEnded -= DisplayUI;
        
    }

    public void UpdateWaveValue()
    {
         if(waveController == null)
        {
            Debug.LogError($"waveController is null in {this.GetType()},please make sure to reference it correctly!");
            return;
        }

        currentWaveUI.text = waveController.waveContainer.currentSubwave.ToString();
    }
    public void smoothBarMoveCaller()
    {
        if(enemiesManager == null)
        {
            Debug.LogError($"enemiesManager is null in {this.GetType()},please make sure to reference it correctly!");
            return;
        }


        if(barCoroutine != null)
        {
            barCoroutines.Enqueue(smoothBarMove());
        }else
        {
            barCoroutine = StartCoroutine(smoothBarMove());
        }
    }
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
        Debug.Log($"currentEnemiesCount: {enemiesManager.CurrentEnemiesCount}, maxEnemiesCount: {enemiesManager.MaxEnemiesCount}");
        if(enemiesManager.CurrentEnemiesCount != 0 && enemiesManager.MaxEnemiesCount != 0)
        {
            endValue = (float)enemiesManager.CurrentEnemiesCount/(float)enemiesManager.MaxEnemiesCount;
        }
        else endValue = 0;
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
