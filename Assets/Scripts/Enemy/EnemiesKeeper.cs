using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesKeeper : MonoBehaviour
{
    [SerializeField] List<GameObject> Enemies = new List<GameObject>();
    void Start()
    {
        foreach (Transform item in gameObject.transform)
        {
            Enemies.Add(item.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
