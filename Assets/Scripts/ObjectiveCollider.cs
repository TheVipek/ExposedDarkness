using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollider : MonoBehaviour
{
    [SerializeField] int objectiveID;
    bool reached = false;
    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Player")
        {
            ObjectiveList objectiveList = ObjectiveList.instance;
            Objective objective = objectiveList.getObjective(objectiveID);
            objectiveList.setObjectiveStatus(objective,ObjectiveStatus.DONE);
            gameObject.SetActive(false);
            reached = true;
        }
        
    }
    
    
}
