using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollider : MonoBehaviour
{
    [SerializeField] int objectiveID;
    bool reached = false;
    [SerializeField] ObjectiveList objectiveList;
    private Objective objective;
    private void Start() {
        objective = objectiveList.getObjective(objectiveID);
    }
    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Player")
        {
            objectiveList.setObjectiveStatus(objective,ObjectiveStatus.DONE);
            gameObject.SetActive(false);
            reached = true;
            
        }
        
    }
    
    
}
