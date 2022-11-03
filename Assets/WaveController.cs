using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance{get; private set;}
    public GameObject wavePanel;
    public List<GameObject> UIToDisable; 
    public static Wave currentWave{get; private set;}

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
    public void triggerWave(Wave wave)
    {
       initWave(wave);

    }
    public void initWave(Wave wave)
    {
        currentWave = wave;
        displayUI(false);
        displayWavePanel(true); 
    }
    public void startWave()
    {
        for(int i=0 ; i<currentWave.listOfEnemies.Count; i++)
        {
            spawnEnemy(currentWave.listOfEnemies[i]);
        }
    }
    public void spawnEnemy(GameObject enemy)
    {
        
        enemy.transform.position = currentWave.spawnPoints[getRespawnPoint()].transform.position;
        enemy.GetComponent<Enemy>().RevertHp();
        enemy.GetComponent<enemyAI>().SanityMode();
    }
    public int getRespawnPoint()
    {
        return Random.Range(0,currentWave.spawnPoints.Count);
    }
    public void displayWavePanel(bool active)
    {
        if(wavePanel == null) return;
        wavePanel.SetActive(active);
    }
    public void displayUI(bool active)
    {
        if(UIToDisable.Count == 0) return;
        for(int i=0;i<UIToDisable.Count;i++)
        {
            UIToDisable[i].SetActive(active);
        }
    }
}
