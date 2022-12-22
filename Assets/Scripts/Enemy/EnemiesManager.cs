using System;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private int currentEnemiesCount;
    public int CurrentEnemiesCount{get{return currentEnemiesCount;}}
    // public static event Action onEnemyAliveChange;
    public delegate void onEnemyAliveChange(int currentEnemiesCount);
    public static event onEnemyAliveChange OnEnemyAliveChange;
    // public static EnemiesManager Instance {get; private set;}
    [SerializeField] RuntimeObjects enemiesAlive;
    private void Awake() {
        if(!enemiesAlive)  Debug.LogWarning($"Not all objects assigned in {GetType()}");
    }
    private void OnEnable() {
        enemiesAlive.OnObjectsChange += enemiesCountChange;
    }
    private void Start() {
        
    }
    private void OnDisable() {
        enemiesAlive.OnObjectsChange -= enemiesCountChange;
    }
    public void enemiesCountChange(int count)
    {
        currentEnemiesCount = count;
        if(OnEnemyAliveChange != null) OnEnemyAliveChange(currentEnemiesCount);
    }
    public bool isAnyEnemyAlive()
    {
        if(currentEnemiesCount > 0) return true;
        return false;
    }
}

