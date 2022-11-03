using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class policeSirensTrigger : MonoBehaviour
{
   [SerializeField] List<AudioSource> policeCars;

    private void OnEnable() {
        triggerSirens();
    }
    public void triggerSirens()
    {
        for(int i=0;i<policeCars.Count;i++)
        {
            policeCars[i].Play();
            
        }
    }
}
