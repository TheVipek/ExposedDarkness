using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class WaveContainer
{
    public int currentSubwave = 0;
    public int amountOfSubwaves;
    public List<GameObject> spawnPoints;
    public List<GameObject> listOfEnemies;


    public int getAmountToSpawn()
    {
        float amount = (currentSubwave * listOfEnemies.Count) / amountOfSubwaves;
        return (int)Mathf.Floor(amount);
    }
    
}
