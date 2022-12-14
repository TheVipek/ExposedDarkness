using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeObject : MonoBehaviour
{
    public RuntimeObjects runtimeObjectAddTo;
    
    private void OnEnable() {
        runtimeObjectAddTo.AddObject(this);
    }
    private void OnDisable() {
        runtimeObjectAddTo.RemoveObject(this);        
    }
}
