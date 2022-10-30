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
    public int id;
    public string description;
    public ObjectiveStatus status;

    [HideInInspector] public GameObject objectiveUI;

    
}
