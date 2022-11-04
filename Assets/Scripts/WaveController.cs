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
        onWaveStarted += displayUI;
        onWaveStarted += displayWavePanel;

        onWaveEnded += displayUI;
        onWaveEnded += displayWavePanel;
    }
    private void OnDisable() {
        onWaveStarted -= displayUI;
        onWaveStarted -= displayWavePanel;
        
        onWaveEnded -= displayUI;
        onWaveEnded -= displayWavePanel;
    }
    public void triggerWave(WaveContainer wave)
    {
       initWave(wave);

    }
    public void initWave(WaveContainer wave)
    {
        onWaveStarted();
        currWave = wave;      
        StartCoroutine(spawnSubwave());
    }
    public IEnumerator spawnSubwave()
    {
        Debug.Log("spawning subwave!");
        WaitForSeconds _timeBetweenMonster = new WaitForSeconds(timeBetweenMonster);
        for(int i=0 ; i<currWave.getAmountToSpawn(); i++)
        {
            spawnEnemy(currWave.listOfEnemies[i]);
            yield return _timeBetweenMonster;
        }
        
    }
    public void spawnEnemy(GameObject enemy)
    {
        
        enemy.transform.position = currWave.spawnPoints[getRespawnPoint()].transform.position;
        enemy.GetComponent<Enemy>().RevertHp();
        enemy.SetActive(true);
        enemy.GetComponent<enemyAI>().SanityMode();
        //enemy.GetComponent<Enemy>().gotProvoked(true);
    }

    public int getRespawnPoint()
    {
        return UnityEngine.Random.Range(0,currWave.spawnPoints.Count);
    }

    public void displayWavePanel()
    {
        if(wavePanel == null) return;
        wavePanel.SetActive(!wavePanel.activeSelf);
    }
    public void displayUI()
    {
        if(UIToDisable.Count == 0) return;
        for(int i=0;i<UIToDisable.Count;i++)
        {
            UIToDisable[i].SetActive(!UIToDisable[i].activeSelf);
        }
    }
}
