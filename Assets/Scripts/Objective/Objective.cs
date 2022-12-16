using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveStatus
{
    DOING,
    DONE
}
[System.Serializable]
public class Objective
{
    private int id;
    public int Id{get{return id;}}
    private string description;
    public string Description{get{return description;}}
    public ObjectiveStatus status = ObjectiveStatus.DOING;

    public Objective(int _id,string _description)
    {
        id = _id;
        description = _description;
    }
}
