using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    [SerializeField] int objectiveID;
    private Objective objective;
    private ObjectiveList objectiveList;
    private void Start()
    {
        objectiveList = ObjectiveList.Instance;
        objective = objectiveList.getObjective(objectiveID);
    }    
    private void OnDisable()
    {
        if(objectiveList != null) objectiveList.setObjectiveStatus(objective,ObjectiveStatus.DONE);
    }
}
