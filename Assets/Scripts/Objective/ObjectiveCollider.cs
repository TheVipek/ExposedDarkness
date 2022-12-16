using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectiveCollider : ObjectiveHandler
{
    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Player") SetToCompleted();
        
    }
    
    
}
