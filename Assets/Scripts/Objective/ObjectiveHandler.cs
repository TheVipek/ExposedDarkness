using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    [SerializeField] protected int objectiveID;
    protected Objective objective;
    protected ObjectiveList objectiveList;
    public virtual void Start()
    {
        objectiveList = ObjectiveList.Instance;
        objective = objectiveList.getObjective(objectiveID);
    }    
    public virtual void OnDisable()
    {
        if(objectiveList != null && (objective != null && objective.objectiveUI != null)) 
        {
            Debug.Log($"objectiveList :{objectiveList} ,objective:{objective}");
            objectiveList.setObjectiveStatus(objective,ObjectiveStatus.DONE);
        }
    }
}
