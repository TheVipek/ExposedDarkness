using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemiesAliveCounter : MonoBehaviour
{
    public static int currentEnemiesCount;
    public static int maxEnemiesCount;
    public static event Action onEnemyAliveChange;
    
    public static void increaseEnemiesCount()
    {
        currentEnemiesCount+=1;
        maxEnemiesCount = currentEnemiesCount;
        if(onEnemyAliveChange!=null) onEnemyAliveChange();
    }
    public static void decreaseEnemiesCount()
    {
        currentEnemiesCount -= 1;
        Debug.Log("Enemies left:" + currentEnemiesCount);
        if(onEnemyAliveChange!=null) onEnemyAliveChange();
    }
}
