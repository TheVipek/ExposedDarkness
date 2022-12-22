using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampsDischarge : MonoBehaviour
{
    [SerializeField] List<GameObject> lights;
    [SerializeField] AudioSource generator;
    bool reached = false;
    void lampDischarge()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].GetComponent<BlinkingLamp>().PowerOut();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && reached == false)
        {
            generator.Play();
            lampDischarge();
            reached = true;
        }
    }
}
