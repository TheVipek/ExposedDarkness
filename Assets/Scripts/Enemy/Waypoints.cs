using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint
{
    [SerializeField] Transform position;
    public Vector3 Position{get{ return position.position;}}
}

public class Waypoints : MonoBehaviour
{

    public Waypoint[] waypoints;
}

