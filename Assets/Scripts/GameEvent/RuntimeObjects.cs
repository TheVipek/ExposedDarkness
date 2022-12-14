using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RuntimeObjects : ScriptableObject
{
    List<RuntimeObject> objects = new List<RuntimeObject>();

    public void AddObject(RuntimeObject obj)
    {
        if(!objects.Contains(obj)) objects.Add(obj);
    }
    public void RemoveObject(RuntimeObject obj)
    {
        if(objects.Contains(obj)) objects.Remove(obj);
    }
}
