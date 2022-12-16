using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu]
public class RuntimeObjects : ScriptableObject
{
    public List<RuntimeObject> objects = new List<RuntimeObject>();
    public delegate void onObjectsChange(int objects);
    public event onObjectsChange OnObjectsChange;
    //public Action onObjectsChange;
    public void AddObject(RuntimeObject obj)
    {
        if(!objects.Contains(obj)) 
        {
            objects.Add(obj);
            if(OnObjectsChange != null) OnObjectsChange(objects.Count);
            
        }
    }
    public void RemoveObject(RuntimeObject obj)
    {
        if(objects.Contains(obj))
        {
            objects.Remove(obj);
            if(OnObjectsChange != null) OnObjectsChange(objects.Count);
        }
    }
}
