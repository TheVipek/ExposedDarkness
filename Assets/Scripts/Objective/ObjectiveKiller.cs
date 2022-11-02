using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKiller : MonoBehaviour
{
    [SerializeField] int objectiveID;
    [SerializeField] ObjectiveList objectiveList;
    private Objective objective;
    
    private void Start() {
        objective = objectiveList.getObjective(objectiveID);
    }

    public void setCompleted()
    {
        objectiveList.setObjectiveStatus(objective,ObjectiveStatus.DONE);
    }
}
