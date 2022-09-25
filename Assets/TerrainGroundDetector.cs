using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGroundDetector : MonoBehaviour
{
    [SerializeField] List<TerrainLayer> terrainLayers;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,-transform.up,out hit,1f))
        {
            if(hit.collider.tag == "Terrain")
            {
               // hit.collider.GetComponent
            }
        }
    }
}
