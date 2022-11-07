using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class WaveController : MonoBehaviour
{
    public static WaveController Instance{get; private set;}
    public GameObject wavePanel;
    public List<GameObject> UIToDisable; 
    public WaveContainer waveContainer;

    public static Action onWaveStartGlobal;
    public static Action onWaveEndGlobal;
    public static Action onWave;
    public static Action onBreakStarted;
    public static Action onBreakEnded;
    public float timeBetweenMonster;
    public float breakBetweenWaves;
    private void Awake() {
        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
        EnemiesAliveCounter.maxEnemiesCount = 0;
        int i;
        for(i=0; i<waveContainer.getAmountToSpawn(); i++)
        {
            spawnEnemy(waveContainer.listOfEnemies[i]);
            yield return _timeBetweenMonster;
        }
        EnemiesAliveCounter.maxEnemiesCount = i;

    }
    // public IEnumerator BreakBetweenWaves()
    // {
    //     onBreakStarted();
    //     float _breakTime = breakBetweenWaves;
    //     while(_breakTime>0)
    //     {
    //         _breakTime -= Time.deltaTime;
    //         yield return null;
    //     }
    //     onBreakEnded();
    //     yield return null;
    //     yield return StartCoroutine(spawnWave());
    // }
    public void spawnEnemy(GameObject enemy)
    {
        
        enemy.transform.position = waveContainer.spawnPoints[getRespawnPoint()].transform.position;
        enemy.GetComponent<Enemy>().RevertHp();
        enemy.SetActive(true);
        enemy.GetComponent<enemyAI>().SanityMode();
    }

    public int getRespawnPoint()
    {
        return UnityEngine.Random.Range(0,waveContainer.spawnPoints.Count);
    }

}
