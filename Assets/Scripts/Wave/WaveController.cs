using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class WaveController : MonoBehaviour
{
    public GameObject wavePanel;
    public List<GameObject> UIToDisable; 
    public WaveContainer waveContainer;

    public static Action onWaveStartGlobal;
    public static Action onWaveEndGlobal;
    [SerializeField] float onWaveEndDelay;
    public static Action onWave;
    public static Action onBreakStarted;
    public static Action onBreakEnded;
    public float timeBetweenMonster;
    public float breakBetweenWaves;
    public static WaveController Instance{get; private set;}
    private void Awake() {
        if(Instance != this && Instance != null) Destroy(this);
        else Instance = this;

        if(!wavePanel || UIToDisable.Count == 0) Debug.LogWarning($"Not all objects assigned in {GetType()}");
    }
    private void OnEnable() {
        onBreakEnded += startWave;
    }
    private void OnDisable() {
        onBreakEnded -= startWave;
    }
    public void initWave(WaveContainer wave)
    {
        waveContainer = wave;         
    }
    public void startWave()
    {
        onWave();
        StartCoroutine(spawnWave());
    }
    public IEnumerator spawnWave()
    {
        
        WaitForSeconds _timeBetweenMonster = new WaitForSeconds(timeBetweenMonster);
        int i;
        for(i=0; i<waveContainer.GetAmount(); i++)
        {
            spawnEnemy(waveContainer.listOfEnemies[i]);
            yield return _timeBetweenMonster;
        }
    }
    public void CallOnWaveGlobalEnd()
    {
        onWaveEndGlobal();
        //Invoke(onWaveEndGlobal.ToString(),onWaveEndDelay);
        //Invoke("onWaveEndGlobal",onWaveEndDelay);
    }
    public void spawnEnemy(GameObject enemy)
    {
        
        enemy.transform.position = waveContainer.spawnPoints[getRespawnPoint()].transform.position;
        enemy.SetActive(true);
        enemy.GetComponent<enemyAI>().SanityMode();
    }

    public int getRespawnPoint()
    {
        return UnityEngine.Random.Range(0,waveContainer.spawnPoints.Count);
    }

}
