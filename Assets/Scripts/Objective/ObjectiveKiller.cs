using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKiller : MonoBehaviour
{
    [SerializeField] int objectiveID;
    [SerializeField] ObjectiveList objectiveList;
    private Objective objective;

    private void OnEnable() {
        EnemiesAliveCounter.onEnemyAliveChange += setCompleted;
    }
    private void Start() {
        objective = objectiveList.getObjective(objectiveID);
    }

    public void setCompleted()
    {
        if(EnemiesAliveCounter.currentEnemiesCount == 0)
        {
            objectiveList.setObjectiveStatus(objective,ObjectiveStatus.DONE);
            gameObject.SetActive(false);
        }
    }
    private void OnDisable() {
        EnemiesAliveCounter.onEnemyAliveChange -= setCompleted;
    }
}
