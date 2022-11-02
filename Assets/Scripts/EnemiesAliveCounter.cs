using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAliveCounter : MonoBehaviour
{
    public static EnemiesAliveCounter Instance{get;private set;}
    public int enemiesCount;
    [SerializeField] ObjectiveKiller objectiveKiller;
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
        enemiesCount = transform.childCount;
        Debug.Log(enemiesCount);
    }
    public void decreaseEnemiesCount()
    {
        enemiesCount -= 1;
        Debug.Log("Enemies left:" + enemiesCount);
        if(enemiesCount == 0 && objectiveKiller.enabled == true)
        {
            objectiveKiller.setCompleted();
            objectiveKiller.enabled = false;
        }

    }
}
