using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemiesAliveCounter : MonoBehaviour
{
    public static EnemiesAliveCounter Instance{get;private set;}
    public static int enemiesCount;
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
    private void Start() {
        // getAliveEnemies();
    }
    // public void getAliveEnemies()
    // {
    //     for(int i=0 ;i< transform.childCount;i++)
    //     {
    //         if(transform.GetChild(i).gameObject.activeSelf == true) enemiesCount+=1;
    //     }
    // }
    public void increaseEnemiesCount()
    {
        enemiesCount+=1;
        if(onEnemyAliveChange!=null) onEnemyAliveChange();
        //Debug.Log()
    }
    public void decreaseEnemiesCount()
    {
        enemiesCount -= 1;
        Debug.Log("Enemies left:" + enemiesCount);
        if(onEnemyAliveChange!=null) onEnemyAliveChange();
        // if(enemiesCount == 0 && objectiveKiller.enabled == true)
        // {
        //     objectiveKiller.setCompleted();
        //     objectiveKiller.enabled = false;
        // }

    }
}
