using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemiesAliveCounter : MonoBehaviour
{
    public static EnemiesAliveCounter Instance{get;private set;}
    public static int currentEnemiesCount;
    public static int maxEnemiesCount;

    public static event Action onEnemyAliveChange;
    
    private void Awake() {
        if(Instance!= this && Instance !=null)
        {
            Destroy(this);
        }else
        {
            Instance = this;
        }
    }
    public void increaseEnemiesCount()
    {
        currentEnemiesCount+=1;
        //if(onEnemyAliveChange!=null) onEnemyAliveChange();
    }
    public void decreaseEnemiesCount()
    {
        currentEnemiesCount -= 1;
        //Debug.Log("Enemies left:" + enemiesCount);
        if(onEnemyAliveChange!=null) onEnemyAliveChange();
    }
}
