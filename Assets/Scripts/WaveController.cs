using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WaveController : MonoBehaviour
{
    public static WaveController Instance{get; private set;}
    public GameObject wavePanel;
    public List<GameObject> UIToDisable; 
    public WaveContainer currWave;
    public static Action onWaveStarted;
    public static Action onWaveEnded;
    public float timeBetweenMonster;
    public float timeBetweenSubwave;
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
    public void triggerWaveEntryEvents()
    {
       onWaveStarted();

    }
    public void initWave(WaveContainer wave)
    {
        currWave = wave;         
        StartCoroutine(spawnSubwave());
    }
    public IEnumerator spawnSubwave()
    {
        
        WaitForSeconds _timeBetweenMonster = new WaitForSeconds(timeBetweenMonster);
        for(int i=0 ; i<currWave.getAmountToSpawn(); i++)
        {
            spawnEnemy(currWave.listOfEnemies[i]);
            yield return _timeBetweenMonster;
        }
        
    }
    public IEnumerator BreakBetweenWaves(float breakLength = 15f)
    {
        while(breakLength>0)
        {
            breakLength -= Time.deltaTime;
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
