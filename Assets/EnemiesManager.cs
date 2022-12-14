using System;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private int currentEnemiesCount;
    public int CurrentEnemiesCount{get{return currentEnemiesCount;}}
    private int maxEnemiesCount;
    public int MaxEnemiesCount{get{return maxEnemiesCount;}}
    public static event Action onEnemyAliveChange;
    private static EnemiesManager instance;
    public static EnemiesManager Instance {get; private set;}
    [SerializeField] RuntimeObjects enemiesAlive;
    private void Awake() {
        if(Instance != this && Instance != null) Destroy(this);
        else Instance = this;
    }
    private void OnEnable() {
        Debug.Log($"OnDisable:Instance is equal to {Instance}");
        Debug.Log($"OnEnable: currentEnemiesCount: {CurrentEnemiesCount}, maxEnemiesCount: {MaxEnemiesCount}");

    }
    private void Start() {
        Debug.Log($"Start: currentEnemiesCount: {CurrentEnemiesCount}, maxEnemiesCount: {MaxEnemiesCount}");
        
    }
    private void OnDisable() {
        Debug.Log($"OnDisable:Instance is equal to {Instance}");
    }
    public void increaseEnemiesCount()
    {
        Debug.Log("Increasing enemies count...");
        currentEnemiesCount+=1;
        maxEnemiesCount+=1; 
        if(onEnemyAliveChange!=null) onEnemyAliveChange();
    }
    public void decreaseEnemiesCount()
    {
        Debug.Log("Decreasing enemies count...");

        currentEnemiesCount -= 1;
       Debug.Log("Enemies left:" + currentEnemiesCount);
        if(onEnemyAliveChange!=null) onEnemyAliveChange();

        //RESET
        if(currentEnemiesCount == 0) maxEnemiesCount = 0;
    }
    public bool isAnyEnemyAlive()
    {
        if(currentEnemiesCount > 0) return true;
        return false;
    }
}

