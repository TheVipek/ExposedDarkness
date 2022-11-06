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
    public WaveContainer currWave;
    public static Action onWaveStarted;
    public static Action onWaveEnded;
    public UnityEvent onBreakStarted;
    
    public float timeBetweenMonster;
    public float breakBetweenSubwave;
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
    }
    private void OnDisable() {
    }
    public void initWave(WaveContainer wave)
    {
        currWave = wave;         
    }
    public void startWaveEvents()
    {
        onWaveStarted();
    }
    public void startWave()
    {
        StartCoroutine(spawnSubwave());
    }
    public IEnumerator spawnSubwave()
    {
        
        WaitForSeconds _timeBetweenMonster = new WaitForSeconds(timeBetweenMonster);
        int i;
        for(i=0; i<currWave.getAmountToSpawn(); i++)
        {
            spawnEnemy(currWave.listOfEnemies[i]);
            yield return _timeBetweenMonster;
        }
        EnemiesAliveCounter.maxEnemiesCount = i;
        Debug.Log(EnemiesAliveCounter.maxEnemiesCount);

    }
    public IEnumerator BreakBetweenWaves()
    {
        float _breakTime = breakBetweenSubwave;
        while(_breakTime>0)
        {
            _breakTime -= Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(spawnSubwave());
    }
    public void spawnEnemy(GameObject enemy)
    {
        
        enemy.transform.position = currWave.spawnPoints[getRespawnPoint()].transform.position;
        enemy.GetComponent<Enemy>().RevertHp();
        enemy.SetActive(true);
        enemy.GetComponent<enemyAI>().SanityMode();
    }

    public int getRespawnPoint()
    {
        return UnityEngine.Random.Range(0,currWave.spawnPoints.Count);
    }

}
