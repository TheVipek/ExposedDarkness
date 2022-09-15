using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// spawn objects and keep in queue so we may reuse them
// optional position and rotation while spawning so we may do entry level monsters allocation




public class ObjectPooler : MonoBehaviour
{
    Queue<GameObject> pooledObjects;
    [SerializeField]List<Transform> objectTransform;
    
    void Start()
    {
        pooledObjects = new Queue<GameObject>();

    }

}
