using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public string objectiveDescription;
    protected ObjectiveManager objectiveManager;
    protected Objective objective;
    public Objective Objective{get{return objective;}}
    protected virtual void OnEnable()
    {

    }
    private void Start() {
        objectiveManager = ObjectiveManager.Instance;
        if(!objectiveManager) Debug.LogWarning($"Not all objects assigned in {GetType()}");
        objective = new Objective(GetInstanceID(),objectiveDescription);   
        Debug.Log($"objectiveId:{objective.Id},objectiveDescription:{objective.Description}");
        objectiveManager.AddObjective(objective); 
    }
    public virtual void SetToCompleted()
    {
        if(objectiveManager != null) 
        {
            Debug.Log($"objectiveID:{objective.Id}");
            objectiveManager.setObjectiveStatus(objective.Id,ObjectiveStatus.DONE);
            gameObject.SetActive(false);
        }

    }
    protected virtual void OnDisable() {
        objective = null;
    }
}
