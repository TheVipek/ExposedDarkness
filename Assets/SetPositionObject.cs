using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionObject : MonoBehaviour
{
    public void wantedObjectPosition(Transform _transform)
    {
        _transform.SetPositionAndRotation(transform.position,transform.rotation);
    }
}
