using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[Serializable]
public class WaveContainer
{
    public int currentSubwave = 0;
    public int amountOfSubwaves;
    public List<GameObject> spawnPoints;
    public List<GameObject> listOfEnemies;

    public UnityEvent onSubwavesStartEvent;
    public UnityEvent onSubwavesEndEvent;

    public int GetAmount()
    {
        float amount = (currentSubwave * listOfEnemies.Count) / amountOfSubwaves;
        return (int)Mathf.Floor(amount);
    }
    
}
